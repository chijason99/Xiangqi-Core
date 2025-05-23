﻿using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Services.GifSaving;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public static class GifGenerationTests
{

	[Fact]
	public static async Task GenerateGifAsync_ShouldCreateFilesCorrectly()
	{
		// Arrange
		string tempDirectory = FileHelper.CreateTempDirectory("XiangqiGifAsyncCreationTests");

		try
		{
			// Act
			XiangqiBuilder builder = new();

			XiangqiGame game = builder
				.WithGameName("王嘉良先負胡榮華")
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
			IGifSavingService gifSavingService = new DefaultGifSavingService();

			ImageConfig config = new()
			{
				UseMoveIndicator = true,
			};

			await gifSavingService.SaveAsync(
				filePath,
				game,
				config,
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

	[Fact]
	public static void GenerateGif_ShouldCreateFilesCorrectly()
	{
		// Arrange
		string tempDirectory = FileHelper.CreateTempDirectory("XiangqiGifCreationTests");
		IGifSavingService gifSavingService = new DefaultGifSavingService();

		try
		{
			// Act
			XiangqiBuilder builder = new();

			XiangqiGame game = builder
				.WithGameName("許銀川先和陶漢明")
				.WithMoveRecord(@"
1. 炮二平五  馬８進７    2. 兵三進一  車９平８
  3. 馬二進三  卒３進１    4. 車一平二  馬２進３
  5. 炮八平七  馬３進２    6. 馬三進四  象３進５
  7. 馬四進五  炮８平９    8. 車二進九  馬７退８
  9. 馬五退七  士４進５   10. 馬七退五  車１平４
 11. 兵七進一  車４進５   12. 炮七退一  車４平３
 13. 馬八進九  車３平４   14. 炮七平三  炮９進４
 15. 兵九進一  馬８進９   16. 車九進一  炮９進２
 17. 仕六進五  車４平３   18. 車九平七  車３進３
 19. 馬九退七  卒９進１   20. 馬七進六  卒９進１
 21. 炮五平九  卒９平８   22. 相七進五  卒８平７
 23. 相五進三  炮９退２   24. 馬六進五  炮２平１
 25. 炮九進四  炮１進３   26. 相三退五  馬９進８
 27. 前馬進三  馬８進６   28. 馬三退四  炮１平６
 29. 馬五進四  炮６平４   30. 仕五進六  炮４退４
 31. 兵五進一  士５進６   32. 仕四進五  炮９退５
 33. 馬四退三  馬２進４   34. 炮九平一  炮９平５
 35. 馬三進四  馬４退２   36. 炮三進二  馬２進３
 37. 炮三平六  馬３退４   38. 馬四退六  炮４進５
 39. 馬六進四  將５平４   40. 炮一退一  炮４退５
 41. 炮一平六  將４平５   42. 兵五進一  炮５進３
 43. 馬四進六  士６退５   44. 馬六退五  炮４進２
 45. 炮六退二  炮４退２   46. 炮六平五  將５平４
 47. 馬五進七  炮４進２   48. 仕五進四  將４平５
 49. 仕六退五  將５平４
")
				.Build();

			string filePath = Path.Combine(tempDirectory, $"{game.GameName}.gif");

			ImageConfig config = new()
			{
				UseMoveIndicator = true,
				FrameDelayInSecond = 1
			};

			gifSavingService.Save(filePath, game, config);

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