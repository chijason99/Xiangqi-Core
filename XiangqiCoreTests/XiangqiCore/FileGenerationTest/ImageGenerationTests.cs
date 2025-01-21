using System.Collections.Concurrent;
using System.Text;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Services.ImageSaving;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public class ImageGenerationTests
{
	[Theory]
	[InlineData(11, 10)]
	[InlineData(21, 20)]
	[InlineData(51, 50)]
	public static async Task GenerateImagesAsync_ShouldCreateFilesCorrectly(int toIndex, int expectedImageCount)
	{
		// Arrange
		string tempDirectory = FileHelper.CreateTempDirectory("XiangqiImageAsyncCreationTests");

		const string gameName = "马炮单缺仕例胜马单缺象";
		const string fen = "5kb2/4a4/5a3/3Nn4/9/6B2/9/3K1A3/C8/2B6 w - - 0 0";

		string csvFilePath = Path.Combine(tempDirectory, $"{gameName}.csv");
		ConcurrentBag<string> fens = [];

		IImageSavingService imageGenerationService = new DefaultImageSavingService();

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

				string filePath = Path.Combine(tempDirectory, $"{game.GameName}_{i}.jpg");

				await imageGenerationService.SaveAsync(
					filePath,
					game.CurrentFen,
					imageConfig: config,
					cancellationToken: cancellationToken);

				fens.Add($"{i},{game.InitialFenString}");

				Assert.True(File.Exists(filePath), "Image file was not created.");
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
	public static void GenerateImage_ShouldCreateFilesCorrectly()
	{
		// Arrange
		string tempDirectory = FileHelper.CreateTempDirectory("XiangqiImageCreationTests");
		IImageSavingService imageGenerationService = new DefaultImageSavingService();

		try
		{
			// Act
			Parallel.For(1, 21, i =>
			{
				XiangqiBuilder builder = new();

				XiangqiGame game = builder
					.WithGameName("三兵巧勝炮雙士")
					.WithStartingFen("3k5/2P1a4/3c1a3/4P4/3P5/9/9/9/9/4K4 w - - 0 0")
					.RandomisePosition()
					.Build();

				string filePath = Path.Combine(tempDirectory, $"{game.GameName}_{i}.jpg");

				ImageConfig config = new()
				{
					UseBlackAndWhitePieces = true,
					UseBlackAndWhiteBoard = true,
				};

				imageGenerationService.Save(filePath, game.CurrentFen, imageConfig: config);

				// Assert
				Assert.True(File.Exists(filePath), "Image file was not created.");
			});
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
