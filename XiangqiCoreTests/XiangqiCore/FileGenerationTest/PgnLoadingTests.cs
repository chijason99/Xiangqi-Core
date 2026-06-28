using FluentAssertions;
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.PgnGeneration;
using XiangqiCore.Services.PgnLoading;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public class PgnLoadingTests
{
	[Fact]
	public void LoadFromString_ShouldRecreateGameExportedByCore()
	{
		// Arrange
		XiangqiGame sourceGame = CreateSampleGame();
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		string pgn = pgnGenerationService.GeneratePgnString(sourceGame);

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromString(pgn);

		// Assert
		loadedGame.RedPlayer.Name.Should().Be(sourceGame.RedPlayer.Name);
		loadedGame.BlackPlayer.Name.Should().Be(sourceGame.BlackPlayer.Name);
		loadedGame.Competition.Name.Should().Be(sourceGame.Competition.Name);
		loadedGame.Competition.Location.Should().Be(sourceGame.Competition.Location);
		loadedGame.Competition.GameDate.Should().Be(sourceGame.Competition.GameDate);
		loadedGame.GameResult.Should().Be(sourceGame.GameResult);
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	[Fact]
	public void LoadFromFile_ShouldReadGbEncodedPgnExportedByCore()
	{
		// Arrange
		XiangqiGame sourceGame = CreateSampleGame();
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		string filePath = Path.Combine(Path.GetTempPath(), $"xiangqi-pgn-loading-{Guid.NewGuid():N}.pgn");
		File.WriteAllBytes(filePath, pgnGenerationService.GeneratePgn(sourceGame));

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromFile(filePath);

		// Assert
		loadedGame.RedPlayer.Name.Should().Be(sourceGame.RedPlayer.Name);
		loadedGame.BlackPlayer.Name.Should().Be(sourceGame.BlackPlayer.Name);
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	[Fact]
	public void LoadFromString_ShouldDetectUcciWhenMoveNumbersAreAttached()
	{
		// Arrange
		XiangqiGame sourceGame = CreateSampleGame();
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		string pgn = pgnGenerationService
			.GeneratePgnString(sourceGame, MoveNotationType.UCCI)
			.Replace(". ", ".", StringComparison.Ordinal);

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromString(pgn);

		// Assert
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	[Fact]
	public void LoadFromString_ShouldFallBackWhenChineseNotationTagIsAmbiguous()
	{
		// Arrange
		XiangqiGame sourceGame = CreateSampleGame();
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		string pgn = "[Notation \"Chinese\"]\n" + pgnGenerationService.GeneratePgnString(sourceGame, MoveNotationType.SimplifiedChinese);

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromString(pgn);

		// Assert
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	[Fact]
	public void LoadFromString_ShouldDetectEnglishWithoutNotationTag()
	{
		// Arrange
		XiangqiGame sourceGame = CreateSampleGame();
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		string pgn = RemoveNotationTag(pgnGenerationService.GeneratePgnString(sourceGame, MoveNotationType.English));

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromString(pgn);

		// Assert
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	[Fact]
	public void LoadFromString_ShouldPreferUciWhenMovesUseRankTenWithoutNotationTag()
	{
		// Arrange
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		const string pgnBody = """
		                       [Event "Rank ten test"]
		                       [Result "*"]
		                       
		                       1. b3e3 h10g8 2. b1c3 i10h10 3. g4g5 g7g6 *
		                       """;
		XiangqiGame sourceGame = pgnLoadingService.LoadFromString("[Notation \"UCI\"]\n" + pgnBody);

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromString(pgnBody);

		// Assert
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	[Fact]
	public void LoadFromString_ShouldPreferUcciWhenMovesUseZeroRankWithoutNotationTag()
	{
		// Arrange
		XiangqiGame sourceGame = CreateSampleGame();
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();
		IPgnLoadingService pgnLoadingService = new DefaultPgnLoadingService();
		string pgn = RemoveNotationTag(pgnGenerationService.GeneratePgnString(sourceGame, MoveNotationType.UCCI));

		// Act
		XiangqiGame loadedGame = pgnLoadingService.LoadFromString(pgn);

		// Assert
		loadedGame.GetMoveHistory().Should().HaveCount(sourceGame.GetMoveHistory().Count);
		loadedGame.CurrentFen.Should().Be(sourceGame.CurrentFen);
	}

	private static XiangqiGame CreateSampleGame()
	{
		XiangqiGame game = new XiangqiBuilder()
			.WithDefaultConfiguration()
			.WithRedPlayer(player =>
			{
				player.Name = "王天一";
				player.Team = "杭州队";
			})
			.WithBlackPlayer(player =>
			{
				player.Name = "郑惟桐";
				player.Team = "成都队";
			})
			.WithCompetition(competition =>
			{
				competition
					.WithName("全国象棋甲级联赛")
					.WithLocation("重庆")
					.WithGameDate(new DateTime(2024, 10, 1));
			})
			.WithGameResult(GameResult.Unknown)
			.Build();

		ApplyMoves(game);

		return game;
	}

	private static void ApplyMoves(XiangqiGame game)
	{
		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese).Should().BeTrue();
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese).Should().BeTrue();
		game.MakeMove("馬二進三", MoveNotationType.TraditionalChinese).Should().BeTrue();
		game.MakeMove("車９平８", MoveNotationType.TraditionalChinese).Should().BeTrue();
		game.MakeMove("兵七進一", MoveNotationType.TraditionalChinese).Should().BeTrue();
		game.MakeMove("卒７進１", MoveNotationType.TraditionalChinese).Should().BeTrue();
	}

	private static string RemoveNotationTag(string pgn)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(pgn);

		return string.Join(
			'\n',
			pgn.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
				.Where(line => !line.StartsWith("[Notation ", StringComparison.OrdinalIgnoreCase)));
	}
}
