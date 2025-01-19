using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Services.PgnSaving;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public static class PgnGenerationTests
{
	[Fact]
	public static void GeneratePgn_ShouldCreateFilesCorrectly()
	{
		// Arrange
		string tempDirectory = FileHelper.CreateTempDirectory("XiangqiPgnCreationTests");

		try
		{
			// Act
			XiangqiBuilder builder = new();

			XiangqiGame game = builder
				.WithGameName("王斌先負許銀川")
				.WithMoveRecord(@"
  1. 炮二平五  馬８進７    2. 馬二進三  車９平８
  3. 車一平二  馬２進３    4. 馬八進九  卒３進１
  5. 車二進四  卒７進１    6. 炮八平七  馬３進２
  7. 車九進一  象３進５    8. 車九平六  炮８退１
  9. 兵九進一  士４進５   10. 兵五進一  車１平４
 11. 車六進八  士５退４   12. 兵五進一  卒５進１
 13. 車二進二  卒３進１   14. 炮七進二  馬２進４
 15. 炮七進五  將５進１   16. 炮七平九  炮８平６
 17. 車二平四  車８進６   18. 馬九進八  車８平７
 19. 馬八進九  車７平３   20. 馬三進五  卒５進１
 21. 炮五進二  馬４退５   22. 馬五退六  車３進３
 23. 炮五平二  炮６平９   24. 炮二進二  馬５進４
 25. 車四平八  車３退９   26. 炮九平六  炮２平３
 27. 炮六退四  炮３進７   28. 仕六進五  馬４進６
 29. 仕五進四  將５退１   30. 炮六退二  炮３退６
 31. 馬九進八  炮３退２   32. 炮六平五  馬６退５
 33. 馬八退七  炮３平５   34. 馬七退五  炮５進３
 35. 相三進五  炮５退１   36. 馬六進八  炮９平５
 37. 車八平六  馬７進６   38. 炮五進二  炮５進４
 39. 馬八進六
")
				.Build();

			IPgnSavingService pgnSavingService = new DefaultPgnSavingService();
			string filePath = Path.Combine(tempDirectory, $"{game.GameName}.pgn");

			// Act
			pgnSavingService.Save(filePath, game);

			// Assert
			Assert.True(File.Exists(filePath), "PGN file was not created.");
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
	public static async Task GeneratePgnAsync_ShouldCreateFilesCorrectly()
	{
		// Arrange
		string tempDirectory = FileHelper.CreateTempDirectory("XiangqiPgnAsyncCreationTests");

		try
		{
			// Act
			XiangqiBuilder builder = new();

			XiangqiGame game = builder
				.WithGameName("呂欽先和許銀川")
				.WithMoveRecord(@"
  1. 兵七進一  卒７進１    2. 馬八進七  馬８進７
  3. 炮八平九  馬２進３    4. 車九平八  車１平２
  5. 炮二平五  車９平８    6. 馬二進三  炮２進６
  7. 兵五進一  士６進５    8. 馬三進五  馬７進６
  9. 仕六進五  炮８平５   10. 兵五進一  馬６進５
 11. 馬七進五  炮５進２   12. 炮五進三  卒５進１
 13. 馬五進六  炮２退２   14. 相七進五  車８進２
 15. 車一進一  車８平４   16. 馬六進七  車４平３
 17. 車一平四  車３平２   18. 車四進二  卒５進１
 19. 車四平七  前車進１   20. 炮九進四

")
				.Build();

			string filePath = Path.Combine(tempDirectory, $"{game.GameName}.pgn");
			IPgnSavingService pgnSavingService = new DefaultPgnSavingService();

			// Act
			await pgnSavingService.SaveAsync(filePath, game, cancellationToken: default);

			// Assert
			Assert.True(File.Exists(filePath), "PGN file was not created.");
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
