using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Move;

namespace xiangqi_core_test.XiangqiCore.ExtensionTest;

public static class XiangqiGameExportExtensionsTests
{
	[Fact]
	public static void ToJson_ShouldReturnValidJsonString()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

		// Act
		string json = game.ToJson();

		// Assert
		Assert.NotNull(json);
		Assert.NotEmpty(json);
		Assert.Contains("gameName", json);
		Assert.Contains("炮二平五", json);
	}

	[Fact]
	public static void ToJson_WithNotationType_ShouldUseSpecifiedNotation()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		// Act
		string json = game.ToJson(MoveNotationType.UCCI);

		// Assert
		Assert.Contains("h2e2", json);
	}

	[Fact]
	public static async Task ToJsonAsync_ShouldReturnValidJsonString()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		// Act
		string json = await game.ToJsonAsync();

		// Assert
		Assert.NotNull(json);
		Assert.NotEmpty(json);
		Assert.Contains("gameName", json);
	}

	[Fact]
	public static void ToJsonBytes_ShouldReturnValidByteArray()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		// Act
		byte[] jsonBytes = game.ToJsonBytes();

		// Assert
		Assert.NotNull(jsonBytes);
		Assert.True(jsonBytes.Length > 0);

		// Verify it's valid UTF-8 JSON
		string json = System.Text.Encoding.UTF8.GetString(jsonBytes);
		Assert.Contains("gameName", json);
	}

	[Fact]
	public static async Task ToJsonBytesAsync_ShouldReturnValidByteArray()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		// Act
		byte[] jsonBytes = await game.ToJsonBytesAsync();

		// Assert
		Assert.NotNull(jsonBytes);
		Assert.True(jsonBytes.Length > 0);
	}

	[Fact]
	public static async Task SaveAsJsonAsync_ShouldCreateFile()
	{
		// Arrange
		string tempDirectory = Path.Combine(Path.GetTempPath(), $"XiangqiTests_{Guid.NewGuid()}");
		Directory.CreateDirectory(tempDirectory);

		try
		{
			XiangqiBuilder builder = new();
			XiangqiGame game = builder
				.WithDefaultConfiguration()
				.Build();

			game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
			game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

			string filePath = Path.Combine(tempDirectory, "test_game.json");

			// Act
			await game.SaveAsJsonAsync(filePath);

			// Assert
			Assert.True(File.Exists(filePath));
			
			string fileContent = await File.ReadAllTextAsync(filePath);
			Assert.Contains("gameName", fileContent);
			Assert.Contains("炮二平五", fileContent);
		}
		finally
		{
			Directory.Delete(tempDirectory, true);
		}
	}

	[Fact]
	public static void ToPgn_ShouldReturnValidPgnString()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

		// Act
		string pgn = game.ToPgn();

		// Assert
		Assert.NotNull(pgn);
		Assert.NotEmpty(pgn);
		Assert.Contains("[Game \"Chinese Chess\"]", pgn);
		Assert.Contains("炮二平五", pgn);
	}

	[Fact]
	public static void ToPgn_WithNotationType_ShouldUseSpecifiedNotation()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

		// Act
		string pgn = game.ToPgn(MoveNotationType.UCCI);

		// Assert - Just verify it uses UCCI format (coordinates)
		Assert.Contains("h2e2", pgn);
		// Verify it's using coordinate-style notation, not Chinese characters
		Assert.DoesNotContain("炮二平五", pgn);
	}

	[Fact]
	public static void ToPgnBytes_ShouldReturnValidByteArray()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		// Act
		byte[] pgnBytes = game.ToPgnBytes();

		// Assert
		Assert.NotNull(pgnBytes);
		Assert.True(pgnBytes.Length > 0);
	}

	[Fact]
	public static async Task SaveAsPgnAsync_ShouldCreateFile()
	{
		// Arrange
		string tempDirectory = Path.Combine(Path.GetTempPath(), $"XiangqiTests_{Guid.NewGuid()}");
		Directory.CreateDirectory(tempDirectory);

		try
		{
			XiangqiBuilder builder = new();
			XiangqiGame game = builder
				.WithDefaultConfiguration()
				.WithGameName("測試對局")
				.Build();

			game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
			game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

			string filePath = Path.Combine(tempDirectory, "test_game.pgn");

			// Act
			await game.SaveAsPgnAsync(filePath);

			// Assert
			Assert.True(File.Exists(filePath));
			
			// Read with appropriate encoding
			byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
			Assert.True(fileBytes.Length > 0);
		}
		finally
		{
			Directory.Delete(tempDirectory, true);
		}
	}

	[Fact]
	public static void MultipleCallsToToJson_ShouldUseSameServiceInstance()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game1 = builder.WithDefaultConfiguration().Build();
		XiangqiGame game2 = builder.WithDefaultConfiguration().Build();

		game1.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
		game2.MakeMove("馬二進三", MoveNotationType.TraditionalChinese);

		// Act - Multiple calls should reuse the same service instance
		string json1 = game1.ToJson();
		string json2 = game2.ToJson();
		string json3 = game1.ToJson();

		// Assert - All calls should succeed and return valid JSON
		Assert.NotNull(json1);
		Assert.NotNull(json2);
		Assert.NotNull(json3);
		Assert.Contains("炮二平五", json1);
		Assert.Contains("馬二進三", json2);
	}

	[Fact]
	public static void ToJson_WithComplexGame_ShouldIncludeAllData()
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder
			.WithDefaultConfiguration()
			.WithRedPlayer(player =>
			{
				player.Name = "測試紅方";
				player.Team = "紅隊";
			})
			.WithBlackPlayer(player =>
			{
				player.Name = "測試黑方";
				player.Team = "黑隊";
			})
			.WithCompetition(competition =>
			{
				competition.WithName("測試賽事");
				competition.WithLocation("測試地點");
				competition.WithGameDate(new DateTime(2024, 1, 1));
			})
			.WithGameResult(GameResult.RedWin)
			.Build();

		game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

		// Act
		string json = game.ToJson();

		// Assert
		Assert.Contains("測試紅方", json);
		Assert.Contains("測試黑方", json);
		Assert.Contains("紅隊", json);
		Assert.Contains("黑隊", json);
		Assert.Contains("測試賽事", json);
		Assert.Contains("測試地點", json);
		Assert.Contains("2024-01-01", json);
	}
}
