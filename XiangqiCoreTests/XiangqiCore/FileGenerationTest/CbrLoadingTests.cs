using System.Buffers.Binary;
using System.Text;
using FluentAssertions;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Services.CbrLoading;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public class CbrLoadingTests
{
	[Fact]
	public void LoadFromStream_WhenCbrContainsMainlineMetadataAndCustomSetup_ShouldLoadGame()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		XiangqiGame sourceGame = CreateCustomSetupGame();
		byte[] fileContents = CbrFixtureBuilder.CreateCustomSetupFile();

		// Act
		using MemoryStream stream = new(fileContents);
		XiangqiGame loadedGame = cbrLoadingService.Load(stream);

		// Assert
		loadedGame.GameName.Should().Be("自訂殘局");
		loadedGame.RedPlayer.Name.Should().Be("紅方");
		loadedGame.BlackPlayer.Name.Should().Be("黑方");
		loadedGame.Competition.Name.Should().Be("殘局測試");
		loadedGame.GameResult.Should().Be(GameResult.BlackWin);
		loadedGame.InitialFenString.Should().Be("r3k4/9/9/9/9/9/9/9/9/3K5 b - - 0 1");
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
		AssertMoveHistoryMatches(loadedGame, sourceGame);
	}

	[Fact]
	public void LoadFromStream_WhenCbrContainsVariations_ShouldLoadSafeMainlineOnly()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		XiangqiGame mainlineGame = CreateVariationMainlineGame();
		byte[] fileContents = CbrFixtureBuilder.CreateVariationFile();

		// Act
		using MemoryStream stream = new(fileContents);
		XiangqiGame loadedGame = cbrLoadingService.Load(stream);

		// Assert
		AssertMoveHistoryMatches(loadedGame, mainlineGame);
		loadedGame.CurrentFen.Should().Be(mainlineGame.CurrentFen);
		loadedGame.GetAllVariationsForCurrentMove().Should().BeEmpty();
	}

	[Fact]
	public void LoadFromFile_WhenProvidedSampleCbrIsValid_ShouldLoadSampleMainline()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		XiangqiGame sourceGame = CreateSampleMainlineGame();
		string filePath = Path.Combine(AppContext.BaseDirectory, "Assets", "先負王者象棋 縣冠 定式實戰第一盤 1.2.3.cbr");

		// Act
		XiangqiGame loadedGame = cbrLoadingService.LoadFromFile(filePath);

		// Assert
		loadedGame.GameName.Should().Be("先負王者象棋 縣冠 定式實戰第一盤 1.2.3");
		loadedGame.GameResult.Should().Be(GameResult.Unknown);
		loadedGame.InitialFenString.Should().Be(sourceGame.InitialFenString);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
		AssertMoveHistoryMatches(loadedGame, sourceGame);
	}

	[Fact]
	public void LoadFromStream_WhenMoveCommentLengthIsInvalid_ShouldThrowHelpfulMessage()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		byte[] fileContents = CbrFixtureBuilder.CreateFileWithInvalidCommentLength();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			cbrLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*odd UTF-16 byte length*move comment*");
	}

	[Fact]
	public void LoadFromStream_WhenHeaderSignatureIsInvalid_ShouldThrowHelpfulMessage()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		byte[] fileContents = CbrFixtureBuilder.CreateFileWithInvalidHeaderSignature();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			cbrLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*expected CBR header signature*");
	}

	[Fact]
	public void LoadFromStream_WhenFileIsTruncatedBeforeMoveSection_ShouldThrowHelpfulMessage()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		byte[] fileContents = CbrFixtureBuilder.CreateTruncatedHeaderFile();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			cbrLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*too short*Expected at least*");
	}

	[Fact]
	public void LoadFromStream_WhenInitialAnnotationIsTruncated_ShouldThrowHelpfulMessage()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		byte[] fileContents = CbrFixtureBuilder.CreateFileWithTruncatedInitialAnnotation();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			cbrLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*ended unexpectedly while reading pre-move annotation*");
	}

	[Fact]
	public void LoadFromStream_WhenTrailingBytesContainNonZeroData_ShouldThrowHelpfulMessage()
	{
		// Arrange
		ICbrLoadingService cbrLoadingService = new DefaultCbrLoadingService();
		byte[] fileContents = CbrFixtureBuilder.CreateFileWithUnexpectedTrailingData();

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			cbrLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*unexpected non-zero trailing data*");
	}

	private static XiangqiGame CreateCustomSetupGame()
	{
		const string startingFen = "r3k4/9/9/9/9/9/9/9/9/3K5 b - - 0 1";

		XiangqiGame game = new XiangqiBuilder()
			.WithStartingFen(startingFen)
			.WithGameName("自訂殘局")
			.WithRedPlayer(player => player.Name = "紅方")
			.WithBlackPlayer(player => player.Name = "黑方")
			.WithCompetition(competition => competition.WithName("殘局測試"))
			.WithGameResult(GameResult.BlackWin)
			.Build();

		game.MakeMove(new Coordinate(1, 10), new Coordinate(1, 9)).Should().BeTrue();
		return game;
	}

	private static XiangqiGame CreateVariationMainlineGame()
	{
		XiangqiGame game = new XiangqiBuilder()
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove(new Coordinate(2, 3), new Coordinate(5, 3)).Should().BeTrue();
		game.MakeMove(new Coordinate(8, 10), new Coordinate(7, 8)).Should().BeTrue();
		game.MakeMove(new Coordinate(2, 1), new Coordinate(3, 3)).Should().BeTrue();

		return game;
	}

	private static XiangqiGame CreateSampleMainlineGame()
	{
		XiangqiGame game = new XiangqiBuilder()
			.WithDefaultConfiguration()
			.WithGameName("先負王者象棋 縣冠 定式實戰第一盤 1.2.3")
			.Build();

		(Coordinate From, Coordinate To)[] moves =
		[
			(new Coordinate(8, 3), new Coordinate(5, 3)),
			(new Coordinate(8, 8), new Coordinate(5, 8)),
			(new Coordinate(8, 1), new Coordinate(7, 3)),
			(new Coordinate(8, 10), new Coordinate(7, 8)),
			(new Coordinate(9, 1), new Coordinate(8, 1)),
			(new Coordinate(9, 10), new Coordinate(9, 9)),
			(new Coordinate(2, 1), new Coordinate(3, 3)),
			(new Coordinate(9, 9), new Coordinate(4, 9)),
			(new Coordinate(7, 4), new Coordinate(7, 5)),
			(new Coordinate(2, 10), new Coordinate(3, 8)),
			(new Coordinate(3, 4), new Coordinate(3, 5)),
			(new Coordinate(1, 10), new Coordinate(1, 9)),
			(new Coordinate(3, 1), new Coordinate(1, 3)),
			(new Coordinate(4, 9), new Coordinate(4, 4)),
			(new Coordinate(4, 1), new Coordinate(5, 2)),
			(new Coordinate(1, 9), new Coordinate(6, 9)),
			(new Coordinate(1, 1), new Coordinate(4, 1)),
			(new Coordinate(4, 4), new Coordinate(3, 4)),
			(new Coordinate(4, 1), new Coordinate(4, 3)),
			(new Coordinate(6, 9), new Coordinate(6, 4)),
			(new Coordinate(2, 3), new Coordinate(2, 1)),
			(new Coordinate(3, 4), new Coordinate(2, 4)),
			(new Coordinate(2, 1), new Coordinate(3, 1)),
			(new Coordinate(6, 4), new Coordinate(7, 4)),
			(new Coordinate(8, 1), new Coordinate(8, 3)),
			(new Coordinate(7, 7), new Coordinate(7, 6)),
			(new Coordinate(7, 5), new Coordinate(7, 6)),
			(new Coordinate(7, 4), new Coordinate(7, 6)),
			(new Coordinate(3, 3), new Coordinate(4, 5)),
			(new Coordinate(5, 8), new Coordinate(4, 8)),
			(new Coordinate(4, 5), new Coordinate(5, 7)),
			(new Coordinate(7, 8), new Coordinate(5, 7)),
			(new Coordinate(5, 3), new Coordinate(5, 7)),
			(new Coordinate(4, 8), new Coordinate(7, 8)),
			(new Coordinate(5, 7), new Coordinate(5, 5)),
			(new Coordinate(2, 4), new Coordinate(5, 4)),
			(new Coordinate(8, 3), new Coordinate(8, 5)),
			(new Coordinate(7, 8), new Coordinate(7, 3)),
			(new Coordinate(3, 1), new Coordinate(3, 7)),
			(new Coordinate(3, 10), new Coordinate(1, 8)),
			(new Coordinate(4, 3), new Coordinate(2, 3)),
			(new Coordinate(7, 3), new Coordinate(7, 5)),
			(new Coordinate(2, 3), new Coordinate(2, 8)),
			(new Coordinate(5, 4), new Coordinate(5, 5)),
			(new Coordinate(2, 8), new Coordinate(3, 8)),
			(new Coordinate(7, 6), new Coordinate(7, 7)),
			(new Coordinate(8, 5), new Coordinate(8, 6)),
			(new Coordinate(7, 5), new Coordinate(8, 5)),
			(new Coordinate(7, 1), new Coordinate(9, 3)),
			(new Coordinate(1, 7), new Coordinate(1, 6)),
			(new Coordinate(3, 5), new Coordinate(3, 6)),
			(new Coordinate(1, 8), new Coordinate(3, 6)),
			(new Coordinate(8, 6), new Coordinate(3, 6)),
			(new Coordinate(8, 5), new Coordinate(8, 1))
		];

		foreach ((Coordinate from, Coordinate to) in moves)
			game.MakeMove(from, to).Should().BeTrue();

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

	private sealed class CbrFixtureBuilder
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
		private const int MoveSectionOffset = 0x8A6;
		private static readonly Encoding Utf16Encoding = Encoding.Unicode;

		public static byte[] CreateCustomSetupFile()
		{
			byte[] fileContents = CreateBaseFile();
			byte[] board = new byte[90];
			SetPiece(board, new Coordinate(4, 1), 0x15);
			SetPiece(board, new Coordinate(1, 10), 0x21);
			SetPiece(board, new Coordinate(5, 10), 0x25);

			WriteString(fileContents, TitleOffset, TitleLength, "自訂殘局");
			WriteString(fileContents, EventOffset, EventLength, "殘局測試");
			WriteString(fileContents, RedPlayerOffset, RedPlayerLength, "紅方");
			WriteString(fileContents, BlackPlayerOffset, BlackPlayerLength, "黑方");
			fileContents[ResultOffset] = 2;
			BinaryPrimitives.WriteUInt16LittleEndian(fileContents.AsSpan(SideToMoveOffset, sizeof(ushort)), 0);
			board.CopyTo(fileContents, BoardOffset);

			List<byte> moveSection = [];
			WriteInitialAnnotationMarker(moveSection, 0);
			WriteVariationList(
				moveSection,
				[
					new CbrMoveRecord(
						new Coordinate(1, 10),
						new Coordinate(1, 9),
						"唯一著法",
						[])
				]);

			return [.. fileContents, .. moveSection];
		}

		public static byte[] CreateVariationFile()
		{
			byte[] fileContents = CreateBaseFile();
			CreateStandardBoard().CopyTo(fileContents, BoardOffset);
			BinaryPrimitives.WriteUInt16LittleEndian(fileContents.AsSpan(SideToMoveOffset, sizeof(ushort)), 1);

			List<byte> moveSection = [];
			CbrMoveRecord mainlineThirdMove = new(
				new Coordinate(2, 1),
				new Coordinate(3, 3),
				null,
				[]);
			CbrMoveRecord mainlineReply = new(
				new Coordinate(8, 10),
				new Coordinate(7, 8),
				"主線",
				[mainlineThirdMove]);
			CbrMoveRecord variationReply = new(
				new Coordinate(2, 10),
				new Coordinate(3, 8),
				"變著",
				[]);
			CbrMoveRecord firstMove = new(
				new Coordinate(2, 3),
				new Coordinate(5, 3),
				null,
				[mainlineReply, variationReply]);

			WriteInitialAnnotationMarker(moveSection, 1);
			WriteUtf16Text(moveSection, "開局註解");
			WriteVariationList(moveSection, [firstMove]);

			return [.. fileContents, .. moveSection];
		}

		public static byte[] CreateFileWithInvalidCommentLength()
		{
			byte[] fileContents = CreateBaseFile();
			CreateStandardBoard().CopyTo(fileContents, BoardOffset);
			BinaryPrimitives.WriteUInt16LittleEndian(fileContents.AsSpan(SideToMoveOffset, sizeof(ushort)), 1);

			List<byte> moveSection = [];
			WriteInitialAnnotationMarker(moveSection, 0);
			moveSection.Add(0x05);
			moveSection.Add(0x00);
			moveSection.Add(ToBoardIndex(new Coordinate(2, 3)));
			moveSection.Add(ToBoardIndex(new Coordinate(5, 3)));
			moveSection.AddRange([0x03, 0x00, 0x00, 0x00]);
			moveSection.AddRange([0x41, 0x00, 0x42]);

			return [.. fileContents, .. moveSection];
		}

		public static byte[] CreateFileWithInvalidHeaderSignature()
		{
			byte[] fileContents = CreateCustomSetupFile();
			fileContents[0] = (byte)'X';
			return fileContents;
		}

		public static byte[] CreateTruncatedHeaderFile()
			=> CreateBaseFile();

		public static byte[] CreateFileWithTruncatedInitialAnnotation()
		{
			byte[] fileContents = CreateBaseFile();
			CreateStandardBoard().CopyTo(fileContents, BoardOffset);
			BinaryPrimitives.WriteUInt16LittleEndian(fileContents.AsSpan(SideToMoveOffset, sizeof(ushort)), 1);

			List<byte> moveSection = [];
			WriteInitialAnnotationMarker(moveSection, 1);
			moveSection.AddRange([0x04, 0x00, 0x00, 0x00]);
			moveSection.AddRange([0x00, 0x4E]);

			return [.. fileContents, .. moveSection];
		}

		public static byte[] CreateFileWithUnexpectedTrailingData()
		{
			byte[] fileContents = CreateBaseFile();
			CreateStandardBoard().CopyTo(fileContents, BoardOffset);
			BinaryPrimitives.WriteUInt16LittleEndian(fileContents.AsSpan(SideToMoveOffset, sizeof(ushort)), 1);

			List<byte> moveSection = [];
			WriteInitialAnnotationMarker(moveSection, 0);
			WriteVariationList(
				moveSection,
				[
					new CbrMoveRecord(
						new Coordinate(2, 3),
						new Coordinate(5, 3),
						null,
						[])
				]);
			moveSection.Add(0x7F);

			return [.. fileContents, .. moveSection];
		}

		private static byte[] CreateBaseFile()
		{
			byte[] fileContents = new byte[MoveSectionOffset];
			Encoding.ASCII.GetBytes("CCBridge Record\0").CopyTo(fileContents, 0);
			return fileContents;
		}

		private static byte[] CreateStandardBoard()
		{
			byte[] board = new byte[90];

			SetPiece(board, new Coordinate(1, 1), 0x11);
			SetPiece(board, new Coordinate(2, 1), 0x12);
			SetPiece(board, new Coordinate(3, 1), 0x13);
			SetPiece(board, new Coordinate(4, 1), 0x14);
			SetPiece(board, new Coordinate(5, 1), 0x15);
			SetPiece(board, new Coordinate(6, 1), 0x14);
			SetPiece(board, new Coordinate(7, 1), 0x13);
			SetPiece(board, new Coordinate(8, 1), 0x12);
			SetPiece(board, new Coordinate(9, 1), 0x11);
			SetPiece(board, new Coordinate(2, 3), 0x16);
			SetPiece(board, new Coordinate(8, 3), 0x16);
			SetPiece(board, new Coordinate(1, 4), 0x17);
			SetPiece(board, new Coordinate(3, 4), 0x17);
			SetPiece(board, new Coordinate(5, 4), 0x17);
			SetPiece(board, new Coordinate(7, 4), 0x17);
			SetPiece(board, new Coordinate(9, 4), 0x17);

			SetPiece(board, new Coordinate(1, 10), 0x21);
			SetPiece(board, new Coordinate(2, 10), 0x22);
			SetPiece(board, new Coordinate(3, 10), 0x23);
			SetPiece(board, new Coordinate(4, 10), 0x24);
			SetPiece(board, new Coordinate(5, 10), 0x25);
			SetPiece(board, new Coordinate(6, 10), 0x24);
			SetPiece(board, new Coordinate(7, 10), 0x23);
			SetPiece(board, new Coordinate(8, 10), 0x22);
			SetPiece(board, new Coordinate(9, 10), 0x21);
			SetPiece(board, new Coordinate(2, 8), 0x26);
			SetPiece(board, new Coordinate(8, 8), 0x26);
			SetPiece(board, new Coordinate(1, 7), 0x27);
			SetPiece(board, new Coordinate(3, 7), 0x27);
			SetPiece(board, new Coordinate(5, 7), 0x27);
			SetPiece(board, new Coordinate(7, 7), 0x27);
			SetPiece(board, new Coordinate(9, 7), 0x27);

			return board;
		}

		private static void WriteInitialAnnotationMarker(List<byte> buffer, int marker)
		{
			Span<byte> markerBytes = stackalloc byte[sizeof(int)];
			BinaryPrimitives.WriteInt32LittleEndian(markerBytes, marker);
			buffer.AddRange(markerBytes.ToArray());
		}

		private static void WriteUtf16Text(List<byte> buffer, string value)
		{
			byte[] bytes = Utf16Encoding.GetBytes(value);
			Span<byte> lengthBytes = stackalloc byte[sizeof(int)];
			BinaryPrimitives.WriteInt32LittleEndian(lengthBytes, bytes.Length);
			buffer.AddRange(lengthBytes.ToArray());
			buffer.AddRange(bytes);
		}

		private static void WriteVariationList(List<byte> buffer, IReadOnlyList<CbrMoveRecord> variations)
		{
			for (int index = 0; index < variations.Count; index++)
			{
				CbrMoveRecord move = variations[index];
				bool hasChildren = move.Continuations.Count > 0;
				bool hasSibling = index < variations.Count - 1;
				bool hasComment = !string.IsNullOrEmpty(move.Comment);

				byte mark = 0x00;

				if (!hasChildren)
					mark |= 0x01;

				if (hasSibling)
					mark |= 0x02;

				if (hasComment)
					mark |= 0x04;

				buffer.Add(mark);
				buffer.Add(0x00);
				buffer.Add(ToBoardIndex(move.From));
				buffer.Add(ToBoardIndex(move.To));

				if (hasComment)
				{
					byte[] commentBytes = Utf16Encoding.GetBytes(move.Comment!);
					byte[] commentLengthBytes = new byte[sizeof(int)];
					BinaryPrimitives.WriteInt32LittleEndian(commentLengthBytes, commentBytes.Length);
					buffer.AddRange(commentLengthBytes);
					buffer.AddRange(commentBytes);
				}

				if (hasChildren)
					WriteVariationList(buffer, move.Continuations);
			}
		}

		private static void WriteString(byte[] fileContents, int offset, int maxLength, string value)
		{
			byte[] bytes = Utf16Encoding.GetBytes(value);
			Array.Copy(bytes, 0, fileContents, offset, Math.Min(bytes.Length, maxLength));
		}

		private static void SetPiece(byte[] board, Coordinate coordinate, byte pieceCode)
			=> board[ToBoardIndex(coordinate)] = pieceCode;

		private static byte ToBoardIndex(Coordinate coordinate)
			=> checked((byte)(((10 - coordinate.Row) * 9) + (coordinate.Column - 1)));

		private sealed record CbrMoveRecord(
			Coordinate From,
			Coordinate To,
			string? Comment,
			IReadOnlyList<CbrMoveRecord> Continuations);
	}
}
