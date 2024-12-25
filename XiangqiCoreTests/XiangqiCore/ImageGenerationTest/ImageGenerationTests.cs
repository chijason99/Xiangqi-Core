using System.Collections.Concurrent;
using System.Text;
using XiangqiCore.Game;
using XiangqiCore.Misc.Images;

namespace xiangqi_core_test.XiangqiCore.ImageGenerationTest;

public static class ImageGenerationTests
{
	[Theory]
	[InlineData(6, 5)]
	[InlineData(11, 10)]
	[InlineData(101, 100)]
	public static async Task GenerateImagesAsync_ShouldNotThrowAnyError(int toIndex, int expectedImageCount)
	{
		// Arrange
		string tempDirectory = Path.Combine(Path.GetTempPath(), "XiangqiImageTests");
		Directory.CreateDirectory(tempDirectory);

		const string gameName = "马炮单缺仕例胜马单缺象";
		const string fen = "5kb2/4a4/5a3/3Nn4/9/6B2/9/3K1A3/C8/2B6 w - - 0 0";

		string csvFilePath = Path.Combine(tempDirectory, $"{gameName}.csv");
		ConcurrentBag<string> fens = [];

		try
		{
			// Act
			await Parallel.ForAsync(fromInclusive: 1, toExclusive: toIndex, async (i, cancellationToken) =>
			{
				XiangqiBuilder builder = new();

				XiangqiGame game = builder.WithStartingFen(fen)
					.WithGameName(gameName)
					.RandomisePosition()
					.Build();

				ImageConfig config = new()
				{
					UseWesternPieces = true,
					UseBlackAndWhitePieces = true,
					UseBlackAndWhiteBoard = true,
				};

				await game.GenerateImageAsync(
					Path.Combine(tempDirectory, $"{gameName}_{i}.jpg"), 
					config: config,
					cancellationToken: cancellationToken);
				fens.Add($"{i},{game.InitialFenString}");
			});

			StringBuilder csvContent = new();
			csvContent.AppendLine("Index,FEN");

			foreach (string fenLine in fens.OrderBy(x => int.Parse(x.Split(',')[0])))
				csvContent.AppendLine(fenLine);

			await File.WriteAllTextAsync(csvFilePath, csvContent.ToString(), cancellationToken: default);

			// Assert
			Assert.True(File.Exists(csvFilePath), "CSV file was not created.");
			Assert.Equal(expectedImageCount, Directory.GetFiles(tempDirectory, "*.jpg").Length);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
		finally
		{
			Directory.Delete(tempDirectory, true);
		}
	}

	[Fact]
	public static async Task GenerateGifAsync_ShouldNotThrowAnyError()
	{
		// Arrange
		string tempDirectory = Path.Combine(Path.GetTempPath(), "XiangqiImageTests");
		Directory.CreateDirectory(tempDirectory);

		try
		{
			// Act
			XiangqiBuilder builder = new();

			XiangqiGame game = builder
				.WithGameName("五六炮进三兵对屏风马进3卒")
				.WithRedPlayer(config =>
				{
					config.Name = "东北王嘉良";
				})
				.WithBlackPlayer(config =>
				{
					config.Name = "上海胡荣华";
				})
				.WithCompetition(config =>
				{
					config.WithName("东北联队、上海队象棋友谊赛");
					config.WithLocation("上海");
					config.WithGameDate(DateTime.Parse("1962-7-7"));
				})
				.WithMoveRecord(@"
  1. 炮二平五  馬８進７    2. 兵三進一  卒３進１
  3. 馬二進三  馬２進３    4. 馬八進九  象７進５
  5. 炮八平六  炮８退１    6. 車一平二  炮８平２
  7. 兵五進一  卒１進１    8. 馬九退七  車１進３
  9. 馬七進六  炮２進４   10. 馬六進四  馬３進４
 11. 兵五進一  馬４進３   12. 炮六進五  卒５進１
 13. 炮六平三  卒５進１   14. 炮五退一  馬３進２
 15. 馬四退五  車１平４   16. 炮五進三  後炮平５
 17. 炮五進四  馬２退４   18. 帥五進一  士６進５
 19. 車九進一  炮２退１   20. 車二進三  車９平６
 21. 車九平六  炮２平５   22. 馬五進四  炮５退１
 23. 車六平七  馬４退５   24. 相七進五  馬５進６
 25. 帥五平四  車６進５   26. 車七進二  車６退３
 27. 車七平四  車４平６   28. 車四平五  馬６退７")
				.Build();

			string filePath = Path.Combine(tempDirectory, $"{game.GameName}.gif");

			ImageConfig config = new()
			{
				UseMoveIndicator = true,
			};

			await game.GenerateGifAsync(
				filePath,
				config,
				frameDelayInSecond: 1,
				cancellationToken: default);

			// Assert
			Assert.True(File.Exists(filePath), "GIF file was not created.");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
		finally
		{
			Directory.Delete(tempDirectory, true);
		}
	}
}
