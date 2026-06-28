using System.Buffers.Binary;
using System.IO;
using System.Text;
using XiangqiCore.Boards;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Services.CbrLoading;

public sealed class DefaultCbrLoadingService : ICbrLoadingService
{
	private const int TitleOffset = 0x0B4;
	private const int TitleLength = 128;
	private const int EventOffset = 0x2B4;
	private const int EventLength = 64;
	private const int RedPlayerOffset = 0x434;
	private const int RedPlayerLength = 64;
	private const int BlackPlayerOffset = 0x514;
	private const int BlackPlayerLength = 64;
	private const int ResultOffset = 0x81C;
	private const int SideToMoveOffset = 0x844;
	private const int BoardOffset = 0x848;
	private const int BoardLength = 90;
	private const int MoveSectionOffset = 0x8A6;
	private const byte EndOfLineFlag = 0x01;
	private const byte HasVariationSiblingFlag = 0x02;
	private const byte HasCommentFlag = 0x04;
	private static readonly Encoding Utf16Encoding = Encoding.Unicode;

	private static ReadOnlySpan<byte> CbrSignature => "CCBridge Record\0"u8;

	/// <inheritdoc />
	public XiangqiGame LoadFromFile(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		if (!File.Exists(filePath))
			throw new FileNotFoundException("The CBR file could not be found.", filePath);

		return Load(File.ReadAllBytes(filePath));
	}

	/// <inheritdoc />
	public XiangqiGame Load(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		if (!stream.CanRead)
			throw new ArgumentException("The supplied CBR stream must be readable.", nameof(stream));

		return Load(FileHelper.ReadAllBytes(stream));
	}

	internal XiangqiGame Load(ReadOnlySpan<byte> fileContents)
	{
		CbrDocument document = ParseDocument(fileContents);
		XiangqiGame game = CreateGame(document);

		ApplyMoves(game, document.MainlineMoves);

		return game;
	}

	internal string? ReadGameTitle(ReadOnlySpan<byte> fileContents)
	{
		ValidateHeader(fileContents);
		return ReadHeaderString(fileContents, TitleOffset, TitleLength, "title");
	}

	private static XiangqiGame CreateGame(CbrDocument document)
	{
		XiangqiBuilder builder = new();
		builder.WithStartingFen(CreateStartingFen(document));

		if (!string.IsNullOrWhiteSpace(document.Title))
			builder.WithGameName(document.Title);

		builder.WithCompetition(competition =>
		{
			if (!string.IsNullOrWhiteSpace(document.EventName))
				competition.WithName(document.EventName);
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

	private static string CreateStartingFen(CbrDocument document)
	{
		BoardConfig boardConfig = new();

		for (int index = 0; index < document.BoardState.Count; index++)
		{
			byte pieceCode = document.BoardState[index];

			if (pieceCode == 0x00)
				continue;

			PieceDescriptor pieceDescriptor = GetPieceDescriptor(pieceCode);
			boardConfig.AddPiece(pieceDescriptor.PieceType, pieceDescriptor.Side, ConvertBoardIndex(index));
		}

		Board board = new(boardConfig);
		return board.GetFenFromPosition.AppendGameInfoToFen(document.SideToMove, 1, 0);
	}

	private static void ApplyMoves(XiangqiGame game, IReadOnlyList<CbrMove> moves)
	{
		for (int index = 0; index < moves.Count; index++)
		{
			CbrMove move = moves[index];
			if (!game.MakeMove(move.From, move.To))
			{
				throw new InvalidDataException(
					$"The CBR contains a move that cannot be applied at ply {index + 1}: {move.From} -> {move.To}.");
			}
		}
	}

	private static CbrDocument ParseDocument(ReadOnlySpan<byte> fileContents)
	{
		ValidateHeader(fileContents);

		int offset = MoveSectionOffset;
		int preMoveAnnotationMarker = ReadInt32LittleEndian(fileContents, ref offset, "pre-move annotation marker");
		string? initialAnnotation = null;

		if (preMoveAnnotationMarker != 0)
		{
			int annotationLength = ReadInt32LittleEndian(fileContents, ref offset, "pre-move annotation length");
			initialAnnotation = ReadUtf16Text(fileContents, ref offset, annotationLength, "pre-move annotation");
		}

		IReadOnlyList<CbrMoveNode> rootMoves = offset < fileContents.Length
			? ParseVariationList(fileContents, ref offset)
			: [];

		ValidateTrailingBytes(fileContents[offset..]);
		List<CbrMove> mainlineMoves = ExtractMainlineMoves(rootMoves);

		return new CbrDocument(
			Title: ReadHeaderString(fileContents, TitleOffset, TitleLength, "title"),
			EventName: ReadHeaderString(fileContents, EventOffset, EventLength, "event"),
			RedPlayer: ReadHeaderString(fileContents, RedPlayerOffset, RedPlayerLength, "red player"),
			BlackPlayer: ReadHeaderString(fileContents, BlackPlayerOffset, BlackPlayerLength, "black player"),
			Result: fileContents[ResultOffset],
			SideToMove: ReadSideToMove(fileContents),
			BoardState: fileContents.Slice(BoardOffset, BoardLength).ToArray(),
			InitialAnnotation: initialAnnotation,
			RootMoves: rootMoves,
			MainlineMoves: mainlineMoves);
	}

	private static void ValidateHeader(ReadOnlySpan<byte> fileContents)
	{
		int minimumLength = MoveSectionOffset + sizeof(int);

		if (fileContents.Length < minimumLength)
		{
			throw new InvalidDataException(
				$"The CBR file is too short. Expected at least {minimumLength} bytes but found {fileContents.Length}.");
		}

		if (!fileContents[..CbrSignature.Length].SequenceEqual(CbrSignature))
			throw new InvalidDataException("The file does not start with the expected CBR header signature ('CCBridge Record\\0').");
	}

	private static IReadOnlyList<CbrMoveNode> ParseVariationList(ReadOnlySpan<byte> fileContents, ref int offset)
	{
		List<CbrMoveNode> variations = [];
		bool hasSibling;

		do
		{
			variations.Add(ParseMove(fileContents, ref offset, out hasSibling));
		} while (hasSibling);

		return variations;
	}

	private static List<CbrMove> ExtractMainlineMoves(IReadOnlyList<CbrMoveNode> rootMoves)
	{
		List<CbrMove> mainlineMoves = [];

		if (rootMoves.Count == 0)
			return mainlineMoves;

		CbrMoveNode? current = rootMoves[0];

		while (current is not null)
		{
			mainlineMoves.Add(new CbrMove(current.From, current.To));

			// Xiangqi-Core can safely import the move mainline today, but it has no dedicated
			// representation for CBR comments/annotations. Variation and comment data remain parsed
			// in the document model here so future work can surface them without redoing the binary parser.
			current = current.Continuations.Count > 0 ? current.Continuations[0] : null;
		}

		return mainlineMoves;
	}

	private static CbrMoveNode ParseMove(ReadOnlySpan<byte> fileContents, ref int offset, out bool hasSibling)
	{
		byte mark = ReadByte(fileContents, ref offset, "move mark");
		_ = ReadByte(fileContents, ref offset, "reserved move byte");
		byte fromIndex = ReadByte(fileContents, ref offset, "move source square");
		byte toIndex = ReadByte(fileContents, ref offset, "move destination square");

		string? comment = null;

		if ((mark & HasCommentFlag) != 0)
		{
			int commentLength = ReadInt32LittleEndian(fileContents, ref offset, "move comment length");
			comment = ReadUtf16Text(fileContents, ref offset, commentLength, "move comment");
		}

		IReadOnlyList<CbrMoveNode> continuations = (mark & EndOfLineFlag) == 0
			? ParseVariationList(fileContents, ref offset)
			: [];

		hasSibling = (mark & HasVariationSiblingFlag) != 0;

		return new CbrMoveNode(
			From: ConvertBoardIndex(fromIndex),
			To: ConvertBoardIndex(toIndex),
			Comment: comment,
			Continuations: continuations);
	}

	private static string? ReadHeaderString(ReadOnlySpan<byte> fileContents, int offset, int maxLength, string fieldName)
	{
		ReadOnlySpan<byte> fieldBytes = ReadSlice(fileContents, offset, maxLength, fieldName);
		int stringLength = GetTerminatedUtf16Length(fieldBytes);

		if (stringLength == 0)
			return null;

		string value = Utf16Encoding.GetString(fieldBytes[..stringLength]).Trim('\0', ' ', '\r', '\n', '\t');
		return string.IsNullOrWhiteSpace(value) ? null : value;
	}

	private static int GetTerminatedUtf16Length(ReadOnlySpan<byte> fieldBytes)
	{
		int evenLength = fieldBytes.Length - (fieldBytes.Length % 2);

		for (int index = 0; index < evenLength - 1; index += 2)
		{
			if (fieldBytes[index] == 0x00 && fieldBytes[index + 1] == 0x00)
				return index;
		}

		return evenLength;
	}

	private static string? ReadUtf16Text(ReadOnlySpan<byte> fileContents, ref int offset, int length, string fieldName)
	{
		if (length < 0)
			throw new InvalidDataException($"The CBR contains a negative {fieldName} length.");

		if ((length & 1) != 0)
			throw new InvalidDataException($"The CBR contains an odd UTF-16 byte length for {fieldName}: {length}.");

		ReadOnlySpan<byte> textBytes = ReadSlice(fileContents, offset, length, fieldName);
		offset += length;

		if (textBytes.Length == 0)
			return null;

		string value = Utf16Encoding.GetString(textBytes).TrimEnd('\0');
		return string.IsNullOrWhiteSpace(value) ? null : value;
	}

	private static Side ReadSideToMove(ReadOnlySpan<byte> fileContents)
	{
		ReadOnlySpan<byte> sideBytes = ReadSlice(fileContents, SideToMoveOffset, sizeof(ushort), "side-to-move");
		ushort encodedSide = BinaryPrimitives.ReadUInt16LittleEndian(sideBytes);
		return encodedSide == 1 ? Side.Red : Side.Black;
	}

	private static PieceDescriptor GetPieceDescriptor(byte pieceCode)
		=> pieceCode switch
		{
			0x11 => new(PieceType.Rook, Side.Red),
			0x12 => new(PieceType.Knight, Side.Red),
			0x13 => new(PieceType.Bishop, Side.Red),
			0x14 => new(PieceType.Advisor, Side.Red),
			0x15 => new(PieceType.King, Side.Red),
			0x16 => new(PieceType.Cannon, Side.Red),
			0x17 => new(PieceType.Pawn, Side.Red),
			0x21 => new(PieceType.Rook, Side.Black),
			0x22 => new(PieceType.Knight, Side.Black),
			0x23 => new(PieceType.Bishop, Side.Black),
			0x24 => new(PieceType.Advisor, Side.Black),
			0x25 => new(PieceType.King, Side.Black),
			0x26 => new(PieceType.Cannon, Side.Black),
			0x27 => new(PieceType.Pawn, Side.Black),
			_ => throw new InvalidDataException($"The CBR contains an unknown board piece code: 0x{pieceCode:X2}.")
		};

	private static Coordinate ConvertBoardIndex(int boardIndex)
	{
		if (boardIndex is < 0 or >= BoardLength)
			throw new InvalidDataException($"The CBR contains an invalid board index: {boardIndex}.");

		int column = (boardIndex % 9) + 1;
		int row = 10 - (boardIndex / 9);

		return new Coordinate(column, row);
	}

	private static GameResult ParseGameResult(byte result)
		=> result switch
		{
			1 => GameResult.RedWin,
			2 => GameResult.BlackWin,
			3 or 4 => GameResult.Draw,
			_ => GameResult.Unknown
		};

	private static byte ReadByte(ReadOnlySpan<byte> fileContents, ref int offset, string fieldName)
	{
		ReadOnlySpan<byte> valueBytes = ReadSlice(fileContents, offset, sizeof(byte), fieldName);
		offset += sizeof(byte);
		return valueBytes[0];
	}

	private static int ReadInt32LittleEndian(ReadOnlySpan<byte> fileContents, ref int offset, string fieldName)
	{
		ReadOnlySpan<byte> valueBytes = ReadSlice(fileContents, offset, sizeof(int), fieldName);
		offset += sizeof(int);
		return BinaryPrimitives.ReadInt32LittleEndian(valueBytes);
	}

	private static ReadOnlySpan<byte> ReadSlice(ReadOnlySpan<byte> fileContents, int offset, int length, string fieldName)
	{
		if (length < 0)
			throw new InvalidDataException($"The CBR requested a negative length for {fieldName}.");

		if (offset < 0 || fileContents.Length - offset < length)
		{
			throw new InvalidDataException(
				$"The CBR ended unexpectedly while reading {fieldName} at offset 0x{offset:X}.");
		}

		return fileContents.Slice(offset, length);
	}

	private static void ValidateTrailingBytes(ReadOnlySpan<byte> trailingBytes)
	{
		if (trailingBytes.IndexOfAnyExcept((byte)0x00) >= 0)
			throw new InvalidDataException("The CBR contains unexpected non-zero trailing data after the move tree.");
	}

	private readonly record struct PieceDescriptor(PieceType PieceType, Side Side);

	private sealed record CbrDocument(
		string? Title,
		string? EventName,
		string? RedPlayer,
		string? BlackPlayer,
		byte Result,
		Side SideToMove,
		IReadOnlyList<byte> BoardState,
		string? InitialAnnotation,
		IReadOnlyList<CbrMoveNode> RootMoves,
		IReadOnlyList<CbrMove> MainlineMoves);

	private readonly record struct CbrMove(
		Coordinate From,
		Coordinate To);

	private sealed record CbrMoveNode(
		Coordinate From,
		Coordinate To,
		string? Comment,
		IReadOnlyList<CbrMoveNode> Continuations);
}
