using System.Text.Json;
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public static class JsonGenerationTests
{
	[Fact]
	public static void GenerateGameJson_ShouldReturnValidJson()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.WithGameName("測試棋局")
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬二進三", MoveNotationType.TraditionalChinese);

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		string json = jsonService.GenerateGameJson(game);

		// Assert
		Assert.NotNull(json);
		Assert.NotEmpty(json);

		// Verify it's valid JSON
		JsonDocument jsonDoc = JsonDocument.Parse(json);
		Assert.NotNull(jsonDoc);

		// Verify structure
		Assert.True(jsonDoc.RootElement.TryGetProperty("gameName", out _));
		Assert.True(jsonDoc.RootElement.TryGetProperty("gameInfo", out _));
		Assert.True(jsonDoc.RootElement.TryGetProperty("boardState", out _));
		Assert.True(jsonDoc.RootElement.TryGetProperty("moveHistory", out _));
	}

	[Fact]
	public static async Task GenerateGameJsonAsync_ShouldReturnValidJson()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("炮８平５", MoveNotationType.TraditionalChinese);

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		string json = await jsonService.GenerateGameJsonAsync(game);

		// Assert
		Assert.NotNull(json);
		Assert.NotEmpty(json);

		JsonDocument jsonDoc = JsonDocument.Parse(json);
		Assert.NotNull(jsonDoc);
	}

	[Fact]
	public static void GenerateGameJson_WithDifferentNotationType_ShouldUseCorrectNotation()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		string jsonWithUcci = jsonService.GenerateGameJson(game, MoveNotationType.UCCI);

		// Assert
		Assert.Contains("h2e2", jsonWithUcci);
	}

	[Fact]
	public static void GenerateGameJsonBytes_ShouldReturnUtf8Bytes()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		byte[] jsonBytes = jsonService.GenerateGameJsonBytes(game);

		// Assert
		Assert.NotNull(jsonBytes);
		Assert.True(jsonBytes.Length > 0);

		// Verify it's valid UTF-8 JSON
		string json = System.Text.Encoding.UTF8.GetString(jsonBytes);
		JsonDocument jsonDoc = JsonDocument.Parse(json);
		Assert.NotNull(jsonDoc);
	}

	[Fact]
	public static void GenerateGameJson_ShouldIncludeGameInfo()
	{
		// Arrange
		XiangqiBuilder builder = new();

		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.WithRedPlayer(player =>
			{
				player.Name = "王斌";
				player.Team = "廣東";
			})
			.WithBlackPlayer(player =>
			{
				player.Name = "許銀川";
				player.Team = "廣東";
			})
			.WithCompetition(competition =>
			{
				competition.WithName("1999年全國象棋個人錦標賽");
				competition.WithLocation("北京");
				competition.WithGameDate(new DateTime(1999, 10, 15));
				competition.WithRound("第1輪");
			})
			.WithGameResult(GameResult.BlackWin)
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		string json = jsonService.GenerateGameJson(game);

		// Assert
		Assert.Contains("王斌", json);
		Assert.Contains("許銀川", json);
		Assert.Contains("廣東", json);
		Assert.Contains("1999年全國象棋個人錦標賽", json);
		Assert.Contains("北京", json);
		Assert.Contains("1999-10-15", json);
	}

	[Fact]
	public static void GenerateGameJson_ShouldIncludeMoveDetails()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		string json = jsonService.GenerateGameJson(game);
		JsonDocument jsonDoc = JsonDocument.Parse(json);

		// Assert
		var moveHistory = jsonDoc.RootElement.GetProperty("moveHistory");
		Assert.True(moveHistory.GetArrayLength() > 0);

		var firstMove = moveHistory[0];
		Assert.True(firstMove.TryGetProperty("roundNumber", out _));
		Assert.True(firstMove.TryGetProperty("side", out _));
		Assert.True(firstMove.TryGetProperty("notation", out _));
		Assert.True(firstMove.TryGetProperty("pieceMoved", out _));
		Assert.True(firstMove.TryGetProperty("from", out _));
		Assert.True(firstMove.TryGetProperty("to", out _));
		Assert.True(firstMove.TryGetProperty("isCapture", out _));
		Assert.True(firstMove.TryGetProperty("fenAfterMove", out _));
	}

	[Fact]
	public static void GenerateGameJson_WithEmptyGame_ShouldReturnValidJson()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		IJsonGenerationService jsonService = new DefaultJsonGenerationService();

		// Act
		string json = jsonService.GenerateGameJson(game);
		JsonDocument jsonDoc = JsonDocument.Parse(json);

		// Assert
		Assert.NotNull(json);
		var moveHistory = jsonDoc.RootElement.GetProperty("moveHistory");
		Assert.Equal(0, moveHistory.GetArrayLength());
	}
}
