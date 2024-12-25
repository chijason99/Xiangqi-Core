using System.Collections.Concurrent;
using System.Reflection;
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
					imageConfig: config,
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
}
