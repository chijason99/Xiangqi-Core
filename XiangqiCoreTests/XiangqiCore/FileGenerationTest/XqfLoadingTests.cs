using System.Buffers.Binary;
using System.Text;
using FluentAssertions;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Services.XqfLoading;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public class XqfLoadingTests
{
	[Fact]
	public void LoadFromStream_WhenClassicMainlineFileIsValid_ShouldLoadGame()
	{
		// Arrange
		IXqfLoadingService xqfLoadingService = new DefaultXqfLoadingService();
		XiangqiGame sourceGame = CreateStandardMainlineGame();
		byte[] fileContents = XqfFixtureBuilder.CreateClassicMainlineFile();

		// Act
		using MemoryStream stream = new(fileContents);
		XiangqiGame loadedGame = xqfLoadingService.Load(stream);

		// Assert
		loadedGame.GameName.Should().Be(sourceGame.GameName);
		loadedGame.RedPlayer.Name.Should().Be("王天一");
		loadedGame.BlackPlayer.Name.Should().Be("郑惟桐");
		loadedGame.Competition.Name.Should().Be("全国象棋甲级联赛");
		loadedGame.Competition.Location.Should().Be("重庆");
		loadedGame.Competition.GameDate.Should().Be(new DateTime(2024, 10, 1));
		loadedGame.GameResult.Should().Be(GameResult.Unknown);
		loadedGame.InitialFenString.Should().Be(sourceGame.InitialFenString);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
		AssertMoveHistoryMatches(loadedGame, sourceGame);
	}

	[Fact]
	public void LoadFromStream_WhenEncryptedMainlineFileIsValid_ShouldLoadGame()
	{
		// Arrange
		IXqfLoadingService xqfLoadingService = new DefaultXqfLoadingService();
		XiangqiGame sourceGame = CreateStandardMainlineGame();
		byte[] fileContents = XqfFixtureBuilder.CreateEncryptedMainlineFile();

		// Act
		using MemoryStream stream = new(fileContents);
		XiangqiGame loadedGame = xqfLoadingService.Load(stream);

		// Assert
		loadedGame.InitialFenString.Should().Be(sourceGame.InitialFenString);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
		AssertMoveHistoryMatches(loadedGame, sourceGame);
	}

	[Fact]
	public void LoadFromStream_WhenEncryptedRootRecordContainsOpaqueData_ShouldLoadGame()
	{
		// Arrange
		IXqfLoadingService xqfLoadingService = new DefaultXqfLoadingService();
		XiangqiGame sourceGame = CreateStandardMainlineGame();
		byte[] fileContents = XqfFixtureBuilder.CreateEncryptedMainlineFileWithOpaqueRootRecord();

		// Act
		using MemoryStream stream = new(fileContents);
		XiangqiGame loadedGame = xqfLoadingService.Load(stream);

		// Assert
		loadedGame.RedPlayer.Name.Should().Be("陳強安");
		loadedGame.BlackPlayer.Name.Should().Be("林俊傑");
		loadedGame.Competition.Name.Should().Be("1-1 陳強安先勝林俊傑");
		loadedGame.InitialFenString.Should().Be(sourceGame.InitialFenString);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
		AssertMoveHistoryMatches(loadedGame, sourceGame);
	}

	[Fact]
	public void LoadFromStream_WhenEncryptedCustomSetupStartsWithBlackMove_ShouldHonorMainline()
	{
		// Arrange
		IXqfLoadingService xqfLoadingService = new DefaultXqfLoadingService();
		XiangqiGame sourceGame = CreateCustomBlackToMoveGame();
		byte[] fileContents = XqfFixtureBuilder.CreateEncryptedCustomSetupFile();

		// Act
		using MemoryStream stream = new(fileContents);
		XiangqiGame loadedGame = xqfLoadingService.Load(stream);

		// Assert
		loadedGame.InitialFenString.Should().Be(sourceGame.InitialFenString);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
		AssertMoveHistoryMatches(loadedGame, sourceGame);
	}

	[Fact]
	public void LoadFromStream_WhenEncryptedFileContainsVariations_ShouldThrow()
	{
		// Arrange
		IXqfLoadingService xqfLoadingService = new DefaultXqfLoadingService();
		byte[] fileContents = XqfFixtureBuilder.CreateEncryptedVariationFile();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			xqfLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<NotSupportedException>()
			.WithMessage("*encrypted variation records*mainline-only*");
	}

	[Fact]
	public void LoadFromStream_WhenLegacyVersionFileIsUsed_ShouldThrowHelpfulMessage()
	{
		// Arrange
		IXqfLoadingService xqfLoadingService = new DefaultXqfLoadingService();
		byte[] fileContents = XqfFixtureBuilder.CreateLegacyVersionFile();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			xqfLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<NotSupportedException>()
			.WithMessage("*legacy version 9*XQF 1.0*");
	}

	private static XiangqiGame CreateStandardMainlineGame()
	{
		XiangqiGame game = new XiangqiBuilder()
			.WithDefaultConfiguration()
			.WithGameName("王天一对郑惟桐")
			.WithRedPlayer(player => player.Name = "王天一")
			.WithBlackPlayer(player => player.Name = "郑惟桐")
			.WithCompetition(competition =>
			{
				competition
					.WithName("全国象棋甲级联赛")
					.WithLocation("重庆")
					.WithGameDate(new DateTime(2024, 10, 1));
			})
			.Build();

		game.MakeMove(new Coordinate(2, 3), new Coordinate(5, 3)).Should().BeTrue();
		game.MakeMove(new Coordinate(8, 10), new Coordinate(7, 8)).Should().BeTrue();
		game.MakeMove(new Coordinate(2, 1), new Coordinate(3, 3)).Should().BeTrue();
		game.MakeMove(new Coordinate(9, 10), new Coordinate(8, 10)).Should().BeTrue();
		game.MakeMove(new Coordinate(7, 4), new Coordinate(7, 5)).Should().BeTrue();
		game.MakeMove(new Coordinate(7, 7), new Coordinate(7, 6)).Should().BeTrue();

		return game;
	}

	private static XiangqiGame CreateCustomBlackToMoveGame()
	{
		const string startingFen = "r3k4/9/9/9/9/9/9/9/9/3K5 b - - 0 1";

		XiangqiGame game = new XiangqiBuilder()
			.WithStartingFen(startingFen)
			.Build();

		game.MakeMove(new Coordinate(1, 10), new Coordinate(1, 9)).Should().BeTrue();

		return game;
	}

	private static void AssertMoveHistoryMatches(XiangqiGame actualGame, XiangqiGame expectedGame)
	{
		IReadOnlyList<MoveHistoryObject> actualMoves = actualGame.GetMoveHistory();
		IReadOnlyList<MoveHistoryObject> expectedMoves = expectedGame.GetMoveHistory();

		actualMoves.Should().HaveCount(expectedMoves.Count);

		for (int index = 0; index < expectedMoves.Count; index++)
		{
			actualMoves[index].MovingSide.Should().Be(expectedMoves[index].MovingSide);
			actualMoves[index].StartingPosition.Should().Be(expectedMoves[index].StartingPosition);
			actualMoves[index].Destination.Should().Be(expectedMoves[index].Destination);
			actualMoves[index].PieceMoved.Should().Be(expectedMoves[index].PieceMoved);
			actualMoves[index].PieceCaptured.Should().Be(expectedMoves[index].PieceCaptured);
			actualMoves[index].IsCapture.Should().Be(expectedMoves[index].IsCapture);
			actualMoves[index].FenBeforeMove.Should().Be(expectedMoves[index].FenBeforeMove);
			actualMoves[index].FenAfterMove.Should().Be(expectedMoves[index].FenAfterMove);
		}
	}

	private sealed class XqfFixtureBuilder
	{
		private static readonly Encoding Gb18030 = CreateGb18030Encoding();
		private static readonly Encoding Big5 = CreateBig5Encoding();
		private static readonly CoordinateMove[] StandardMoves =
		[
			new(new Coordinate(2, 3), new Coordinate(5, 3), "炮二平五"),
			new(new Coordinate(8, 10), new Coordinate(7, 8), null),
			new(new Coordinate(2, 1), new Coordinate(3, 3), null),
			new(new Coordinate(9, 10), new Coordinate(8, 10), null),
			new(new Coordinate(7, 4), new Coordinate(7, 5), null),
			new(new Coordinate(7, 7), new Coordinate(7, 6), null)
		];

		public static byte[] CreateClassicMainlineFile()
		{
			byte[] header = CreateHeader(10, useDefaultSetup: true);
			WriteTextField(header, 0x50, 64, "王天一对郑惟桐");
			WriteTextField(header, 0xD0, 64, "全国象棋甲级联赛");
			WriteTextField(header, 0x110, 16, "2024.10.01");
			WriteTextField(header, 0x120, 16, "重庆");
			WriteTextField(header, 0x130, 16, "王天一");
			WriteTextField(header, 0x140, 16, "郑惟桐");
			header[0x33] = 0;

			List<byte> moveSection = [];
			WriteClassicRootRecord(moveSection, hasNext: true);

			for (int i = 0; i < StandardMoves.Length; i++)
			{
				CoordinateMove move = StandardMoves[i];
				byte tag = i == StandardMoves.Length - 1 ? (byte)0x00 : (byte)0xF0;
				WriteClassicMoveRecord(moveSection, move.From, move.To, tag, move.Comment);
			}

			return [.. header, .. moveSection];
		}

		public static byte[] CreateEncryptedMainlineFile()
		{
			byte[] header = CreateHeader(
				version: 12,
				useDefaultSetup: true,
				arg0: 0x0F,
				keyBytes: [0x21, 0x32, 0x43, 0x54],
				saltBytes: [0x03, 0x01, 0x02, 0x04]);

			List<byte> moveSection = [];
			EncryptionInfo encryptionInfo = CreateEncryptionInfo(header);
			WriteEncryptedRootRecord(moveSection, encryptionInfo, hasNext: true);

			for (int i = 0; i < StandardMoves.Length; i++)
			{
				CoordinateMove move = StandardMoves[i];
				byte tag = (byte)(i == StandardMoves.Length - 1 ? 0x00 : 0x80);

				if (!string.IsNullOrEmpty(move.Comment))
					tag |= 0x20;

				WriteEncryptedMoveRecord(moveSection, encryptionInfo, move.From, move.To, tag, move.Comment);
			}

			return [.. header, .. moveSection];
		}

		public static byte[] CreateEncryptedMainlineFileWithOpaqueRootRecord()
		{
			byte[] header = CreateHeader(
				version: 18,
				useDefaultSetup: true,
				arg0: 0xEE,
				keyBytes: [0xA8, 0xB6, 0x36, 0x6C],
				saltBytes: [0x22, 0xBC, 0x39, 0xE9]);
			WriteTextField(header, 0x50, 64, "1-1 陳強安先勝林俊傑", Big5);
			WriteTextField(header, 0x130, 16, "陳強安", Big5);
			WriteTextField(header, 0x140, 16, "林俊傑", Big5);

			List<byte> moveSection = [];
			EncryptionInfo encryptionInfo = CreateEncryptionInfo(header);
			WriteEncryptedOpaqueRootRecord(moveSection, encryptionInfo, [0x58, 0x51, 0x96, 0x6B]);

			for (int i = 0; i < StandardMoves.Length; i++)
			{
				CoordinateMove move = StandardMoves[i];
				byte tag = (byte)(i == StandardMoves.Length - 1 ? 0x00 : 0x80);

				if (!string.IsNullOrEmpty(move.Comment))
					tag |= 0x20;

				WriteEncryptedMoveRecord(moveSection, encryptionInfo, move.From, move.To, tag, move.Comment);
			}

			return [.. header, .. moveSection];
		}

		public static byte[] CreateEncryptedCustomSetupFile()
		{
			byte[] header = CreateHeader(
				version: 12,
				useDefaultSetup: false,
				arg0: 0x0F,
				keyBytes: [0x11, 0x22, 0x33, 0x44],
				saltBytes: [0x04, 0x01, 0x05, 0x02]);

			byte[] piecePositions = Enumerable.Repeat(byte.MaxValue, 32).ToArray();
			piecePositions[4] = ToXqfSquare(new Coordinate(4, 1));
			piecePositions[16] = ToXqfSquare(new Coordinate(1, 10));
			piecePositions[20] = ToXqfSquare(new Coordinate(5, 10));
			WritePiecePositions(header, piecePositions);

			List<byte> moveSection = [];
			EncryptionInfo encryptionInfo = CreateEncryptionInfo(header);
			WriteEncryptedRootRecord(moveSection, encryptionInfo, hasNext: true);
			WriteEncryptedMoveRecord(
				moveSection,
				encryptionInfo,
				new Coordinate(1, 10),
				new Coordinate(1, 9),
				tag: 0x00,
				comment: null);

			return [.. header, .. moveSection];
		}

		public static byte[] CreateEncryptedVariationFile()
		{
			byte[] header = CreateHeader(
				version: 12,
				useDefaultSetup: true,
				arg0: 0x0F,
				keyBytes: [0x21, 0x32, 0x43, 0x54],
				saltBytes: [0x03, 0x01, 0x02, 0x04]);

			List<byte> moveSection = [];
			EncryptionInfo encryptionInfo = CreateEncryptionInfo(header);
			WriteEncryptedRootRecord(moveSection, encryptionInfo, hasNext: true);
			WriteEncryptedMoveRecord(
				moveSection,
				encryptionInfo,
				StandardMoves[0].From,
				StandardMoves[0].To,
				tag: 0x40,
				comment: null);

			return [.. header, .. moveSection];
		}

		public static byte[] CreateLegacyVersionFile()
		{
			byte[] header = CreateHeader(version: 9, useDefaultSetup: true);
			return [.. header, .. new byte[8]];
		}

		private static byte[] CreateHeader(
			byte version,
			bool useDefaultSetup,
			byte arg0 = 0,
			byte[]? keyBytes = null,
			byte[]? saltBytes = null)
		{
			byte[] header = new byte[0x400];
			header[0] = (byte)'X';
			header[1] = (byte)'Q';
			header[2] = version;
			header[3] = arg0;
			header[0x40] = useDefaultSetup ? (byte)0 : (byte)2;

			if (keyBytes is not null)
				Array.Copy(keyBytes, 0, header, 8, keyBytes.Length);

			if (saltBytes is not null)
				Array.Copy(saltBytes, 0, header, 12, saltBytes.Length);

			return header;
		}

		private static EncryptionInfo CreateEncryptionInfo(byte[] header)
		{
			int pieceOffset = GetPieceOffset(header);
			int sourceOffset = GetSourceOffset(header, pieceOffset);
			int destinationOffset = GetDestinationOffset(header, sourceOffset);
			int commentOffset = GetCommentOffset(header);
			int[] encryptionStream = CreateEncryptionStream(header);

			return new EncryptionInfo(sourceOffset, destinationOffset, commentOffset, pieceOffset, encryptionStream);
		}

		private static void WritePiecePositions(byte[] header, IReadOnlyList<byte> piecePositions)
		{
			EncryptionInfo encryptionInfo = CreateEncryptionInfo(header);

			for (int i = 0; i < piecePositions.Count; i++)
			{
				int sourceIndex = (encryptionInfo.PieceOffset + 1 + i) % piecePositions.Count;
				header[0x10 + i] = unchecked((byte)(piecePositions[sourceIndex] + encryptionInfo.PieceOffset));
			}
		}

		private static void WriteClassicRootRecord(List<byte> buffer, bool hasNext)
		{
			buffer.Add(0x18);
			buffer.Add(0x20);
			buffer.Add(hasNext ? (byte)0xF0 : (byte)0x00);
			buffer.Add(0xFF);
			buffer.AddRange([0x00, 0x00, 0x00, 0x00]);
		}

		private static void WriteClassicMoveRecord(
			List<byte> buffer,
			Coordinate from,
			Coordinate to,
			byte tag,
			string? comment)
		{
			byte[] commentBytes = string.IsNullOrEmpty(comment) ? [] : Gb18030.GetBytes(comment);

			buffer.Add(unchecked((byte)(ToXqfSquare(from) + 24)));
			buffer.Add(unchecked((byte)(ToXqfSquare(to) + 32)));
			buffer.Add(tag);
			buffer.Add(0x00);

			Span<byte> commentLengthBytes = stackalloc byte[4];
			BinaryPrimitives.WriteInt32LittleEndian(commentLengthBytes, commentBytes.Length);
			buffer.AddRange(commentLengthBytes.ToArray());
			buffer.AddRange(commentBytes);
		}

		private static void WriteEncryptedRootRecord(List<byte> buffer, EncryptionInfo encryptionInfo, bool hasNext)
		{
			byte tag = hasNext ? (byte)0x80 : (byte)0x00;

			WriteEncryptedBytes(
				buffer,
				encryptionInfo,
				[
					unchecked((byte)(24 + encryptionInfo.SourceOffset)),
					unchecked((byte)(32 + encryptionInfo.DestinationOffset)),
					tag,
					0xFF
				]);
		}

		private static void WriteEncryptedOpaqueRootRecord(
			List<byte> buffer,
			EncryptionInfo encryptionInfo,
			IReadOnlyList<byte> rootRecordBytes)
		{
			WriteEncryptedBytes(buffer, encryptionInfo, rootRecordBytes);
		}

		private static void WriteEncryptedMoveRecord(
			List<byte> buffer,
			EncryptionInfo encryptionInfo,
			Coordinate from,
			Coordinate to,
			byte tag,
			string? comment)
		{
			WriteEncryptedBytes(
				buffer,
				encryptionInfo,
				[
					unchecked((byte)(ToXqfSquare(from) + 24 + encryptionInfo.SourceOffset)),
					unchecked((byte)(ToXqfSquare(to) + 32 + encryptionInfo.DestinationOffset)),
					tag,
					0x00
				]);

			if ((tag & 0x20) == 0)
				return;

			byte[] commentBytes = string.IsNullOrEmpty(comment) ? [] : Gb18030.GetBytes(comment);
			Span<byte> commentLengthBytes = stackalloc byte[4];
			BinaryPrimitives.WriteInt32LittleEndian(
				commentLengthBytes,
				commentBytes.Length + encryptionInfo.CommentOffset);

			WriteEncryptedBytes(buffer, encryptionInfo, commentLengthBytes.ToArray());

			if (commentBytes.Length > 0)
				WriteEncryptedBytes(buffer, encryptionInfo, commentBytes);
		}

		private static void WriteEncryptedBytes(List<byte> buffer, EncryptionInfo encryptionInfo, IReadOnlyList<byte> bytes)
		{
			for (int i = 0; i < bytes.Count; i++)
			{
				buffer.Add(unchecked((byte)(bytes[i] + encryptionInfo.EncryptionStream[encryptionInfo.StreamIndex])));
				encryptionInfo.StreamIndex = (encryptionInfo.StreamIndex + 1) % encryptionInfo.EncryptionStream.Length;
			}
		}

		private static int[] CreateEncryptionStream(IReadOnlyList<byte> header)
		{
			int[] args = new int[4];
			int[] encryptionStream = new int[32];
			byte[] mask = Encoding.ASCII.GetBytes("[(C) Copyright Mr. Dong Shiwei.]");

			for (int i = 0; i < args.Length; i++)
				args[i] = header[8 + i] | (header[12 + i] & header[3]);

			for (int i = 0; i < encryptionStream.Length; i++)
				encryptionStream[i] = args[i % args.Length] & mask[i];

			return encryptionStream;
		}

		private static int GetPieceOffset(IReadOnlyList<byte> header)
			=> unchecked((byte)(Square54Plus221(header[13]) * header[13]));

		private static int GetSourceOffset(IReadOnlyList<byte> header, int pieceOffset)
			=> unchecked((byte)(Square54Plus221(header[14]) * pieceOffset));

		private static int GetDestinationOffset(IReadOnlyList<byte> header, int sourceOffset)
			=> unchecked((byte)(Square54Plus221(header[15]) * sourceOffset));

		private static int GetCommentOffset(IReadOnlyList<byte> header)
			=> ((header[12] * 256) + header[13]) % 32000 + 767;

		private static int Square54Plus221(int value) => (value * value * 54) + 221;

		private static byte ToXqfSquare(Coordinate coordinate)
			=> unchecked((byte)(((coordinate.Column - 1) * 10) + (coordinate.Row - 1)));

		private static void WriteTextField(byte[] header, int offset, int fieldLength, string value)
			=> WriteTextField(header, offset, fieldLength, value, Gb18030);

		private static void WriteTextField(byte[] header, int offset, int fieldLength, string value, Encoding encoding)
		{
			byte[] bytes = encoding.GetBytes(value);
			int length = Math.Min(bytes.Length, fieldLength - 1);
			header[offset] = (byte)length;
			Array.Copy(bytes, 0, header, offset + 1, length);
		}

		private static Encoding CreateGb18030Encoding()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			return Encoding.GetEncoding("GB18030");
		}

		private static Encoding CreateBig5Encoding()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			return Encoding.GetEncoding("Big5");
		}

		private sealed record CoordinateMove(Coordinate From, Coordinate To, string? Comment);

		private sealed class EncryptionInfo(
			int sourceOffset,
			int destinationOffset,
			int commentOffset,
			int pieceOffset,
			int[] encryptionStream)
		{
			public int SourceOffset { get; } = sourceOffset;
			public int DestinationOffset { get; } = destinationOffset;
			public int CommentOffset { get; } = commentOffset;
			public int PieceOffset { get; } = pieceOffset;
			public int[] EncryptionStream { get; } = encryptionStream;
			public int StreamIndex { get; set; }
		}
	}
}
