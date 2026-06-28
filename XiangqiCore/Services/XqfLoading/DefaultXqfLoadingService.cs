using System.Buffers.Binary;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using XiangqiCore.Boards;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Services.XqfLoading;

public sealed partial class DefaultXqfLoadingService : IXqfLoadingService
{
	private const int HeaderLength = 0x400;
	private const int MoveRecordLength = 8;
	private const int HeaderDataLength = 0x200;
	private const byte Xqf10Version = 10;
	private const byte EncryptedVariationFlag = 0x40;
	private const byte EncryptedCommentFlag = 0x20;
	private const byte EncryptedHasNextFlag = 0x80;
	private const string DefaultStartingFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";
	private static readonly Encoding XqfGb18030Encoding = CreateXqfEncoding("GB18030");
	private static readonly Encoding XqfBig5Encoding = CreateXqfEncoding("Big5");
	private static readonly TextFieldDefinition[] TextFieldDefinitions =
	[
		new(0x50, 64),
		new(0xD0, 64),
		new(0x110, 16),
		new(0x120, 16),
		new(0x130, 16),
		new(0x140, 16)
	];

	private static ReadOnlySpan<byte> EncryptionMask => "[(C) Copyright Mr. Dong Shiwei.]"u8;

	private static readonly PieceDescriptor[] PieceOrder =
	[
		new(PieceType.Rook, Side.Red),
		new(PieceType.Knight, Side.Red),
		new(PieceType.Bishop, Side.Red),
		new(PieceType.Advisor, Side.Red),
		new(PieceType.King, Side.Red),
		new(PieceType.Advisor, Side.Red),
		new(PieceType.Bishop, Side.Red),
		new(PieceType.Knight, Side.Red),
		new(PieceType.Rook, Side.Red),
		new(PieceType.Cannon, Side.Red),
		new(PieceType.Cannon, Side.Red),
		new(PieceType.Pawn, Side.Red),
		new(PieceType.Pawn, Side.Red),
		new(PieceType.Pawn, Side.Red),
		new(PieceType.Pawn, Side.Red),
		new(PieceType.Pawn, Side.Red),
		new(PieceType.Rook, Side.Black),
		new(PieceType.Knight, Side.Black),
		new(PieceType.Bishop, Side.Black),
		new(PieceType.Advisor, Side.Black),
		new(PieceType.King, Side.Black),
		new(PieceType.Advisor, Side.Black),
		new(PieceType.Bishop, Side.Black),
		new(PieceType.Knight, Side.Black),
		new(PieceType.Rook, Side.Black),
		new(PieceType.Cannon, Side.Black),
		new(PieceType.Cannon, Side.Black),
		new(PieceType.Pawn, Side.Black),
		new(PieceType.Pawn, Side.Black),
		new(PieceType.Pawn, Side.Black),
		new(PieceType.Pawn, Side.Black),
		new(PieceType.Pawn, Side.Black)
	];

	/// <inheritdoc />
	public XiangqiGame LoadFromFile(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		if (!File.Exists(filePath))
			throw new FileNotFoundException("The XQF file could not be found.", filePath);

		return Load(File.ReadAllBytes(filePath));
	}

	/// <inheritdoc />
	public XiangqiGame Load(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		if (!stream.CanRead)
			throw new ArgumentException("The supplied XQF stream must be readable.", nameof(stream));

		return Load(FileHelper.ReadAllBytes(stream));
	}

	internal XiangqiGame Load(ReadOnlySpan<byte> fileContents)
	{
		if (fileContents.Length < HeaderLength + MoveRecordLength)
		{
			throw new InvalidDataException(
				"The XQF file is too short to contain a valid header and move section.");
		}

		XqfDocument document = ParseDocument(fileContents);
		XiangqiGame game = CreateGame(document);

		ApplyMoves(game, document.Moves);

		return game;
	}

	private static XiangqiGame CreateGame(XqfDocument document)
	{
		XiangqiBuilder builder = new();
		string startingFen = GetStartingFen(document);

		builder.WithStartingFen(startingFen);

		if (!string.IsNullOrWhiteSpace(document.GameName))
			builder.WithGameName(document.GameName);

		string? eventName = !string.IsNullOrWhiteSpace(document.EventName)
			? document.EventName
			: document.GameName;

		string? site = document.Site;
		DateTime? gameDate = ParseGameDate(document.Date);

		builder.WithCompetition(competition =>
		{
			if (!string.IsNullOrWhiteSpace(eventName))
				competition.WithName(eventName);

			if (!string.IsNullOrWhiteSpace(site))
				competition.WithLocation(site);

			if (gameDate.HasValue)
				competition.WithGameDate(gameDate.Value);
		});

		builder.WithRedPlayer(player =>
		{
			if (!string.IsNullOrWhiteSpace(document.RedPlayer))
				player.Name = document.RedPlayer;
		});

		builder.WithBlackPlayer(player =>
		{
			if (!string.IsNullOrWhiteSpace(document.BlackPlayer))
				player.Name = document.BlackPlayer;
		});

		builder.WithGameResult(ParseGameResult(document.Result));

		return builder.Build();
	}

	private static string GetStartingFen(XqfDocument document)
	{
		if (document.UsesDefaultStartingPosition)
		{
			Side sideToMove = DetermineSideToMove(new Board(DefaultStartingFen), document.Moves);

			if (sideToMove == Side.Red)
				return DefaultStartingFen;

			string defaultBoardFen = DefaultStartingFen.Split(' ', 2, StringSplitOptions.TrimEntries)[0];
			return defaultBoardFen.AppendGameInfoToFen(sideToMove, 1, 0);
		}

		BoardConfig boardConfig = new();
		PopulateBoardConfig(boardConfig, document);
		Board board = new(boardConfig);
		Side customSideToMove = DetermineSideToMove(board, document.Moves);

		return board.GetFenFromPosition.AppendGameInfoToFen(customSideToMove, 1, 0);
	}

	private static void PopulateBoardConfig(BoardConfig boardConfig, XqfDocument document)
	{
		for (int i = 0; i < document.PiecePositions.Count; i++)
		{
			byte piecePosition = document.PiecePositions[i];

			if (piecePosition == byte.MaxValue)
				continue;

			PieceDescriptor piece = PieceOrder[i];
			boardConfig.AddPiece(piece.PieceType, piece.Side, ConvertPosition(piecePosition));
		}
	}

	private static Side DetermineSideToMove(Board board, IReadOnlyList<XqfMove> moves)
	{
		if (moves.Count == 0)
			return Side.Red;

		Piece piece = board.GetPieceAtPosition(moves[0].From);

		if (piece is EmptyPiece)
		{
			throw new InvalidDataException(
				$"The first XQF move starts from an empty square: {moves[0].From}.");
		}

		return piece.Side;
	}

	private static void ApplyMoves(XiangqiGame game, IReadOnlyList<XqfMove> moves)
	{
		for (int index = 0; index < moves.Count; index++)
		{
			XqfMove move = moves[index];

			if (!game.MakeMove(move.From, move.To))
			{
				throw new InvalidDataException(
					$"The XQF contains a move that cannot be applied at ply {index + 1}: {move.From} -> {move.To}.");
			}
		}
	}

	private static XqfDocument ParseDocument(ReadOnlySpan<byte> fileContents)
	{
		ReadOnlySpan<byte> header = fileContents[..HeaderDataLength];

		if (header[0] != (byte)'X' || header[1] != (byte)'Q')
			throw new InvalidDataException("The file does not start with the expected XQF header signature ('XQ').");

		byte version = header[2];

		if (version < Xqf10Version)
		{
			throw new NotSupportedException(
				$"This XQF file uses unsupported legacy version {version}. Xiangqi-Core supports XQF 1.0 (version 10) and newer mainline-only XQStudio variants.");
		}

		Encoding textEncoding = DetectTextEncoding(header);
		byte[] piecePositions = DecodePiecePositions(header, version);
		List<XqfMove> moves = version < 11
			? ParseClassicMoves(fileContents[HeaderLength..])
			: ParseEncryptedMoves(fileContents[HeaderLength..], header);

		return new XqfDocument(
			Version: version,
			UsesDefaultStartingPosition: header[0x40] < 2,
			Result: header[0x33],
			GameName: ReadStringField(header, 0x50, 64, textEncoding),
			EventName: ReadStringField(header, 0xD0, 64, textEncoding),
			Date: ReadStringField(header, 0x110, 16, textEncoding),
			Site: ReadStringField(header, 0x120, 16, textEncoding),
			RedPlayer: ReadStringField(header, 0x130, 16, textEncoding),
			BlackPlayer: ReadStringField(header, 0x140, 16, textEncoding),
			PiecePositions: piecePositions,
			Moves: moves);
	}

	private static byte[] DecodePiecePositions(ReadOnlySpan<byte> header, byte version)
	{
		byte[] encodedPiecePositions = header.Slice(0x10, 32).ToArray();

		if (version < 11)
			return encodedPiecePositions;

		int pieceOffset = GetPieceOffset(header);
		byte[] decodedPositions = new byte[32];

		if (version < 12)
		{
			for (int i = 0; i < decodedPositions.Length; i++)
				decodedPositions[i] = unchecked((byte)(encodedPiecePositions[i] - pieceOffset));

			return decodedPositions;
		}

		for (int i = 0; i < encodedPiecePositions.Length; i++)
		{
			int targetIndex = (pieceOffset + 1 + i) % encodedPiecePositions.Length;
			decodedPositions[targetIndex] = unchecked((byte)(encodedPiecePositions[i] - pieceOffset));
		}

		return decodedPositions;
	}

	private static List<XqfMove> ParseClassicMoves(ReadOnlySpan<byte> moveSection)
	{
		List<XqfMove> moves = [];
		int offset = 0;
		bool skippedRootRecord = false;

		while (true)
		{
			if (moveSection.Length - offset < MoveRecordLength)
				throw new InvalidDataException("The XQF move section ended unexpectedly.");

			byte source = moveSection[offset];
			byte destination = moveSection[offset + 1];
			byte tag = moveSection[offset + 2];
			int commentLength = BinaryPrimitives.ReadInt32LittleEndian(moveSection.Slice(offset + 4, 4));
			offset += MoveRecordLength;

			if (commentLength < 0 || moveSection.Length - offset < commentLength)
				throw new InvalidDataException("The XQF contains an invalid annotation length.");

			if (tag is not 0x00 and not 0xF0)
			{
				throw new NotSupportedException(
					$"The XQF move tag 0x{tag:X2} is not supported in classic XQF files.");
			}

			XqfMoveRecord moveRecord = new(
				GetRawSourceSquareValue(source, 0),
				GetRawDestinationSquareValue(destination, 0),
				(tag & 0xF0) != 0);

			if (!skippedRootRecord)
			{
				// XQF move trees always start with a non-played root node. Real-world files do not
				// consistently zero the root node's source/destination bytes, so the loader must skip
				// the first record instead of validating it as a playable move.
				skippedRootRecord = true;
			}
			else
			{
				moves.Add(new XqfMove(ConvertPosition(moveRecord.Source), ConvertPosition(moveRecord.Destination)));
			}

			offset += commentLength;

			if (!moveRecord.HasNext)
				break;
		}

		return moves;
	}

	private static List<XqfMove> ParseEncryptedMoves(ReadOnlySpan<byte> moveSection, ReadOnlySpan<byte> header)
	{
		List<XqfMove> moves = [];
		int sourceOffset = GetSourceOffset(header, GetPieceOffset(header));
		int destinationOffset = GetDestinationOffset(header, sourceOffset);
		int commentOffset = GetCommentOffset(header);
		int[] encryptionStream = CreateEncryptionStream(header);
		int encryptionIndex = 0;
		int offset = 0;
		bool skippedRootRecord = false;
		Span<byte> moveBytes = stackalloc byte[4];
		Span<byte> commentLengthBytes = stackalloc byte[sizeof(int)];

		while (true)
		{
			ReadEncryptedBytes(moveSection, ref offset, moveBytes, encryptionStream, ref encryptionIndex);
			byte tag = moveBytes[2];

			if ((tag & EncryptedVariationFlag) != 0)
			{
				throw new NotSupportedException(
					"This XQF file contains encrypted variation records, which are not supported yet. Only mainline-only XQF files can be imported.");
			}

			int commentLength = 0;

			if ((tag & EncryptedCommentFlag) != 0)
			{
				ReadEncryptedBytes(
					moveSection,
					ref offset,
					commentLengthBytes,
					encryptionStream,
					ref encryptionIndex);

				commentLength = BinaryPrimitives.ReadInt32LittleEndian(commentLengthBytes) - commentOffset;

				if (commentLength < 0)
					throw new InvalidDataException("The XQF contains an invalid encrypted annotation length.");
			}

			XqfMoveRecord moveRecord = new(
				GetRawSourceSquareValue(moveBytes[0], sourceOffset),
				GetRawDestinationSquareValue(moveBytes[1], destinationOffset),
				(tag & EncryptedHasNextFlag) != 0);

			if (!skippedRootRecord)
			{
				// XQStudio version-18 files can store opaque bytes in the root node while still using
				// the same mainline move encoding for every real move that follows.
				skippedRootRecord = true;
			}
			else
			{
				moves.Add(new XqfMove(ConvertPosition(moveRecord.Source), ConvertPosition(moveRecord.Destination)));
			}

			if (commentLength > 0)
				SkipEncryptedBytes(moveSection, ref offset, commentLength, encryptionStream, ref encryptionIndex);

			if (!moveRecord.HasNext)
				break;
		}

		return moves;
	}

	private static void ReadEncryptedBytes(
		ReadOnlySpan<byte> moveSection,
		ref int offset,
		Span<byte> buffer,
		IReadOnlyList<int> encryptionStream,
		ref int encryptionIndex)
	{
		int length = buffer.Length;

		if (length < 0 || moveSection.Length - offset < length)
			throw new InvalidDataException("The XQF move section ended unexpectedly.");

		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = unchecked((byte)(moveSection[offset + i] - encryptionStream[encryptionIndex]));
			encryptionIndex = (encryptionIndex + 1) % encryptionStream.Count;
		}

		offset += length;
	}

	private static void SkipEncryptedBytes(
		ReadOnlySpan<byte> moveSection,
		ref int offset,
		int length,
		IReadOnlyList<int> encryptionStream,
		ref int encryptionIndex)
	{
		if (length < 0 || moveSection.Length - offset < length)
			throw new InvalidDataException("The XQF move section ended unexpectedly.");

		offset += length;
		encryptionIndex = (encryptionIndex + (length % encryptionStream.Count)) % encryptionStream.Count;
	}

	private static int[] CreateEncryptionStream(ReadOnlySpan<byte> header)
	{
		int[] args = new int[4];
		int[] encryptionStream = new int[32];
		byte arg0 = header[3];

		for (int i = 0; i < args.Length; i++)
			args[i] = header[8 + i] | (header[12 + i] & arg0);

		for (int i = 0; i < encryptionStream.Length; i++)
			encryptionStream[i] = args[i % args.Length] & EncryptionMask[i];

		return encryptionStream;
	}

	private static int GetPieceOffset(ReadOnlySpan<byte> header)
	{
		byte source = header[13];
		return unchecked((byte)(Square54Plus221(source) * source));
	}

	private static int GetSourceOffset(ReadOnlySpan<byte> header, int pieceOffset)
		=> unchecked((byte)(Square54Plus221(header[14]) * pieceOffset));

	private static int GetDestinationOffset(ReadOnlySpan<byte> header, int sourceOffset)
		=> unchecked((byte)(Square54Plus221(header[15]) * sourceOffset));

	private static int GetCommentOffset(ReadOnlySpan<byte> header)
		=> ((header[12] * 256) + header[13]) % 32000 + 767;

	private static int Square54Plus221(int value) => (value * value * 54) + 221;

	private static byte GetRawSourceSquareValue(byte encodedSquare, int offset)
		=> unchecked((byte)(encodedSquare - 24 - offset));

	private static byte GetRawDestinationSquareValue(byte encodedSquare, int offset)
		=> unchecked((byte)(encodedSquare - 32 - offset));

	private static Coordinate ConvertPosition(byte position)
	{
		if (position >= 90)
			throw new InvalidDataException($"The XQF contains an invalid board coordinate value: {position}.");

		int column = (position / 10) + 1;
		int row = (position % 10) + 1;

		return new Coordinate(column, row);
	}

	private static string? ReadStringField(ReadOnlySpan<byte> header, int offset, int fieldLength, Encoding encoding)
	{
		byte length = header[offset];

		if (length == 0)
			return null;

		int contentLength = Math.Min(length, fieldLength - 1);
		string value = encoding.GetString(header.Slice(offset + 1, contentLength)).TrimEnd('\0', ' ');

		return string.IsNullOrWhiteSpace(value) ? null : value;
	}

	private static Encoding DetectTextEncoding(ReadOnlySpan<byte> header)
	{
		foreach (TextFieldDefinition textField in TextFieldDefinitions)
		{
			byte length = header[textField.Offset];
			if (length == 0)
				continue;

			int contentLength = Math.Min(length, textField.FieldLength - 1);
			ReadOnlySpan<byte> valueBytes = header.Slice(textField.Offset + 1, contentLength);
			string gb18030Value = XqfGb18030Encoding.GetString(valueBytes);
			if (!ContainsPrivateUseCharacters(gb18030Value))
				continue;

			try
			{
				string big5Value = XqfBig5Encoding.GetString(valueBytes);
				if (!ContainsPrivateUseCharacters(big5Value))
					return XqfBig5Encoding;
			}
			catch (DecoderFallbackException)
			{
			}
		}

		return XqfGb18030Encoding;
	}

	private static bool ContainsPrivateUseCharacters(string value)
	{
		foreach (char character in value)
		{
			if (char.GetUnicodeCategory(character) == UnicodeCategory.PrivateUse)
				return true;
		}

		return false;
	}

	private static GameResult ParseGameResult(byte result)
		=> result switch
		{
			1 => GameResult.RedWin,
			2 => GameResult.BlackWin,
			3 => GameResult.Draw,
			_ => GameResult.Unknown
		};

	private static DateTime? ParseGameDate(string? dateValue)
	{
		if (string.IsNullOrWhiteSpace(dateValue))
			return null;

		string normalizedDate = dateValue.Trim();
		string[] supportedFormats =
		[
			"yyyy.MM.dd",
			"yyyy.M.d",
			"yyyy-MM-dd",
			"yyyy-M-d",
			"yyyy/MM/dd",
			"yyyy/M/d",
			"yyyyMMdd",
			"yyyy年M月d日"
		];

		if (DateTime.TryParseExact(
			    normalizedDate,
			    supportedFormats,
			    CultureInfo.InvariantCulture,
			    DateTimeStyles.None,
			    out DateTime parsedDate))
		{
			return parsedDate;
		}

		Match dateMatch = YearMonthDayRegex().Match(normalizedDate);

		if (!dateMatch.Success)
			return null;

		if (!int.TryParse(dateMatch.Groups["year"].Value, out int year) ||
		    !int.TryParse(dateMatch.Groups["month"].Value, out int month) ||
		    !int.TryParse(dateMatch.Groups["day"].Value, out int day))
		{
			return null;
		}

		return DateTime.TryParse(
			$"{year:D4}-{month:D2}-{day:D2}",
			CultureInfo.InvariantCulture,
			DateTimeStyles.None,
			out DateTime fallbackParsedDate)
			? fallbackParsedDate
			: null;
	}

	private static Encoding CreateXqfEncoding(string codePageName)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		return Encoding.GetEncoding(
			codePageName,
			EncoderFallback.ExceptionFallback,
			DecoderFallback.ExceptionFallback);
	}

	[GeneratedRegex(@"(?<year>\d{4})\D+(?<month>\d{1,2})\D+(?<day>\d{1,2})")]
	private static partial Regex YearMonthDayRegex();

	private readonly record struct PieceDescriptor(PieceType PieceType, Side Side);

	private readonly record struct XqfMove(Coordinate From, Coordinate To);

	private readonly record struct XqfMoveRecord(byte Source, byte Destination, bool HasNext);

	private readonly record struct TextFieldDefinition(int Offset, int FieldLength);

	private sealed record XqfDocument(
		byte Version,
		bool UsesDefaultStartingPosition,
		byte Result,
		string? GameName,
		string? EventName,
		string? Date,
		string? Site,
		string? RedPlayer,
		string? BlackPlayer,
		IReadOnlyList<byte> PiecePositions,
		IReadOnlyList<XqfMove> Moves);
}
