using System.Text.RegularExpressions;
using xiangqi_core_test.XiangqiCore.MoveParserTest;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Services.MoveParsing;
using XiangqiCore.Services.PgnGeneration;

namespace xiangqi_core_test.XiangqiCore.XiangqiGameTest;
public static class XiangqiGameTests
{
    private const string _startingFen1 = MoveParserTestHelper.StartingFen1;

    public static IEnumerable<object []> MoveMethodWithCoordinatesTestData
    {
        get
        {
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(2, 10), new(2, 4), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 4), new(7, 1), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 8), new(6, 6), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(3, 7), new(3, 6), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 8), new(3, 10), ExpectedResult: true) };

            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 10), new(4, 10), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(8, 10), new(7, 9), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 9), new(6, 9), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 4), new(7, 8), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(4, 5), new(3, 7), ExpectedResult: false) };
        }
    }

	public static IEnumerable<object []> UndoMoveMethodTestData
	{
		get
		{
			yield return new object [] { new UndoMoveMethodTestData(
				"r3kabr1/4a4/4bc2n/p1p1C1p1p/2c6/6P2/P1P1P3P/C1N5B/9/1RBAKA2R w - - 0 0",
				"\r\n  1. 車一進一  炮３進３    2. 車一平八  車１平４\r\n  3. 炮九進四  車８進６    4. 炮九進三  車４進２\r\n  5. 車八進八  車４退２    6. 炮九平六  車８平５\r\n  7. 仕四進五  車５退３    8. 炮六平四  士５退４\r\n  9. 炮四平六  炮６退１   10. 車八進八",
				NumberOfMovesToUndo: 3,
				ExpectedFen: "1R1akCb2/9/4bc2n/2p1r1p1p/9/6P2/P1P5P/2c5B/4A4/1RBAK4 w - - 1 8",
				ExpectedMoveHistoryCount: 16,
				ExpectedRoundNumber: 8) };

			yield return new object[] { new UndoMoveMethodTestData(
				"rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0",
				"1. 炮二平五  馬８進７    2. 馬二進三  卒７進１\r\n  3. 車一平二  車９平８    4. 車二進六  馬２進３\r\n  5. 兵七進一  炮８平９    6. 車二平三  炮９退１\r\n  7. 馬八進九  車１進１    8. 炮八平七  車１平６\r\n  9. 車三退一  炮９平７   10. 車三平八  車８進８\r\n 11. 炮五平六  炮２平１   12. 相七進五  馬７進６\r\n 13. 仕六進五  炮１進４   14. 馬九進七  炮１平５\r\n 15. 炮七退一  車８進１   16. 馬三進五  馬６進５\r\n 17. 兵七進一  車８平７   18. 兵七進一  馬３退１\r\n 19. 馬七進五  車７平８   20. 車九進六  炮７平９\r\n 21. 馬五進六  炮９進５   22. 馬六進七  車６平４\r\n 23. 車九進二  炮９進３   24. 車八退二  車８退１\r\n 25. 相五退三  車８進１   26. 相三進五  車８退１\r\n 27. 相五退三  車８進１   28. 相三進五  車８退５\r\n 29. 相五退三  車８進５   30. 相三進五  車８退３\r\n 31. 相五退三  車８平７   32. 仕五進四  車７進３\r\n 33. 帥五進一  馬５退７   34. 車八平六  車４平３\r\n 35. 車九平七  象３進１   36. 炮七平八  士６進５\r\n 37. 炮六平五  象７進５   38. 炮五進五  將５平６\r\n 39. 車六平四  士５進６   40. 車四進四  將６平５\r\n 41. 車四進一",
				NumberOfMovesToUndo: 13,
				ExpectedFen: "2bakab2/R1r6/9/2P1p3p/9/6n2/3R5/3C1A3/2C1K4/5Ar1c w - - 0 34",
				ExpectedMoveHistoryCount: 68,
				ExpectedRoundNumber: 8) };

			yield return new object[] { new UndoMoveMethodTestData(
				"rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0",
				" 1. 相三進五  馬２進３    2. 兵七進一  卒７進１\r\n  3. 馬八進七  馬８進７    4. 車九進一  炮８平９\r\n  5. 馬二進三  車９平８    6. 車一平二  象３進５\r\n  7. 炮二進四  車１進１    8. 車九平四  車１平４\r\n  9. 車四進三  車４進３   10. 仕四進五  卒３進１\r\n 11. 炮二平九  車８進９   12. 馬三退二  馬７進６\r\n 13. 炮九平一  炮９平６   14. 車四平二  卒３進１\r\n 15. 車二平七  炮２退２   16. 炮八進四  炮２平３\r\n 17. 炮八平七  馬３退１   18. 炮七平九  車４進４\r\n 19. 相七進九  馬６進７   20. 馬七進六  車４退２\r\n 21. 相九退七  車４平５   22. 車七進二  馬１進３\r\n 23. 炮九平五  士４進５   24. 車七平六  車５退１\r\n 25. 炮一退一  炮６進３   26. 炮五平三  馬７進５\r\n 27. 相七進五  炮６平４   28. 馬二進四  炮４平２\r\n 29. 車六平八  馬３退１   30. 車八平七  車５進２\r\n 31. 炮一進三  車５退４   32. 車七退二  士５退４\r\n 33. 炮三進二  馬１進２   34. 車七退四  象５進３",
				NumberOfMovesToUndo: 68,
				ExpectedFen: "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0",
				ExpectedMoveHistoryCount: 0,
				ExpectedRoundNumber: 0) };
		}
	}

	[Theory]
	[MemberData(nameof(MoveMethodWithCoordinatesTestData))]
	public static void MoveMethod_ShouldAlterTheBoardCorrectly(MoveMethodTestData testData)
	{
		// Arrange
		XiangqiBuilder builder = new();

		XiangqiGame game = builder
							.WithStartingFen(testData.StartingFen)
							.Build();

		bool expectedResult = testData.ExpectedResult;

		// Act
		bool actualResult = game.MakeMove(testData.StartingPosition, testData.Destination);

		// Assert
		actualResult.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1", "2baka3/9/1cn1bc1n1/pC2p1p1N/2p6/6P2/P1P1P3P/2N5C/9/2BAKAB2 w - - 0 12", "1. 馬八進七  卒３進１    2. 兵三進一  馬２進３\r\n  3. 馬二進三  車１進１    4. 炮二平一  象７進５\r\n  5. 車一平二  炮８平６    6. 車九進一  車１平７\r\n  7. 馬三進二  車７平８    8. 車九平二  馬８進６\r\n  9. 馬二進一  車８進７   10. 車二進一  車９進２\r\n 11. 炮八進四  車９平８   12. 車二進六  馬６進８")]
	[InlineData("2baka2r/5n3/1cn1bc3/p3p1p1N/2p6/6P2/P1P1P3P/1CN5C/7R1/2BAKAB2 b - - 0 10", "2bak4/4a4/1c2b4/4C3N/C5P2/2p3n2/P1n1Pc2P/2N1B4/4A4/4KAB2 b - - 4 20", "10. ... 車９進２\r\n 11. 炮八進四  車９平８   12. 車二進六  馬６進８\r\n 13. 炮八平三  馬３進４   14. 炮三平九  炮６進４\r\n 15. 炮九退一  馬４進３   16. 兵三進一  馬８進６\r\n 17. 炮一平五  馬６進７   18. 炮五進四  士６進５\r\n 19. 仕六進五  卒３進１   20. 相七進五")]
	[InlineData("2bak4/4a4/1c2b4/4C4/C5P2/3p1cnN1/P1n1P3P/2N1B4/4A4/4KAB2 w - - 7 21", "2bak4/4a4/c3b4/C8/5P3/2B6/Pp6P/3A5/N5n2/3K1AB2 w - - 0 33", "22. 馬二進三  馬７進８\r\n 23. 兵三平四  馬８進９   24. 馬三退四  馬９退７\r\n 25. 帥五平六  炮２平４   26. 仕五進六  卒４平５\r\n 27. 炮五平六  卒５平６   28. 相五進七  卒６進１\r\n 29. 炮九平七  卒６平５   30. 炮七退二  卒５平４\r\n 31. 炮七平八  卒４平３   32. 馬七退九  炮４平１\r\n 33. 炮六平九  卒３平２")]
	public static void MoveMethodShouldCreateCorrectMoveHistory_WhenProvidedGameNotation(string startingFen, string finalFen, string gameRecord)
	{
		// Arrange
		XiangqiBuilder builder = new();

		XiangqiGame game = builder
							.WithStartingFen(startingFen)
							.Build();

		DefaultMoveParsingService moveParsingService = new();

		List<string> moves = moveParsingService.ParseGameRecord(gameRecord);

		// Act
		foreach (string move in moves)
		{
			bool isMoveSuccessful = game.MakeMove(move, MoveNotationType.TraditionalChinese);
			isMoveSuccessful.Should().BeTrue();
		}

		// Assert
		game.CurrentFen.Should().Be(finalFen);
	}

	[Theory]
	[InlineData("2bak4/4a4/1c2b4/4C4/C5P2/3p1cnN1/P1n1P3P/2N1B4/4A4/4KAB2 w - - 7 21", "22. 馬二進三  馬７進８\r\n 23. 兵三平四  馬８進９   24. 馬三退四  馬９退７\r\n 25. 帥五平六  炮２平４   26. 仕五進六  卒４平５\r\n 27. 炮五平六  卒５平６   28. 相五進七  卒６進１\r\n 29. 炮九平七  卒６平５   30. 炮七退二  卒５平４\r\n 31. 炮七平八  卒４平３   32. 馬七退九  炮４平１\r\n 33. 炮六平九  卒３平２", 22)]
	[InlineData("2baka2r/2r2n3/1c2b1c2/p1C1p1p1p/9/5NP2/PRn1P2CP/2N5R/9/2BAKAB2 b - - 0 13", "13. 車３進２   14. 車八平七  車３進３\r\n 15. 炮二平七  車９平８   16. 馬四進六  車８進４\r\n 17. 車一平六  炮２平４   18. 馬六進八  車８平３\r\n 19. 炮七進一  炮４平２   20. 車六進六  士４進５\r\n 21. 馬七進六  卒７進１   22. 相三進五  車３平２\r\n 23. 馬八退六  炮２平４", 13)]
	[InlineData("5ab2/3k5/5a3/p4N3/2b2CP2/3n5/1c1n4P/4B4/3NA4/2BA1K3 b - - 0 42", "42. 士６進５", 42)]
	public static void ExportMoveHistory_ShouldReturnCorrectPgnGameRecordString(string startingFen, string gameRecord, int initialMoveRoundNumber)
	{
		// Arrange
		XiangqiBuilder builder = new();

		XiangqiGame game = builder
							.WithStartingFen(startingFen)
							.Build();

		DefaultMoveParsingService moveParsingService = new();

		List<string> moves = moveParsingService.ParseGameRecord(gameRecord);

		foreach (string move in moves)
		{
			bool isMoveSuccessful = game.MakeMove(move, MoveNotationType.TraditionalChinese);
			isMoveSuccessful.Should().BeTrue();
		}

		int startingRoundNumberCounter = initialMoveRoundNumber;
		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();

		// Act
		string pgnGameRecord = pgnGenerationService.ExportMoveHistory(game);

		// Assert
		foreach (string move in moves)
		{
			pgnGameRecord.Should().Contain(move);
		}

		MatchCollection roundNumbers = Regex.Matches(pgnGameRecord, @"\d+\.");

		foreach (Match roundNumber in roundNumbers)
		{
			roundNumber.Value.Should().Be($"{startingRoundNumberCounter}.");
			startingRoundNumberCounter++;
		}
	}

	[Theory]
	[InlineData("1990全國個人賽", "趙國榮", "萬春林", "黑龍江", "上海", GameResult.RedWin, "杭州", "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0", 1990, 10, 16, "  1. 炮二平五  馬８進７    2. 馬二進三  車９平８\r\n  3. 車一平二  炮８進４    4. 兵三進一  炮２平５\r\n  5. 兵七進一  馬２進３    6. 馬八進七  車１平２\r\n  7. 車九平八  車２進４    8. 炮八平九  車２平８\r\n  9. 車八進六  炮５平６   10. 車八平七  象７進５\r\n 11. 兵七進一  炮８平７   12. 車二平一  車８平３\r\n 13. 車七退一  象５進３   14. 馬七進六  車８進４\r\n 15. 馬六進五  馬３進５   16. 炮五進四  炮６平３\r\n 17. 炮五退一  炮３進７   18. 仕六進五  炮７平１\r\n 19. 兵五進一  炮１退２   20. 車一平二  車８進５\r\n 21. 馬三退二  炮３平２   22. 馬二進三  炮１平５\r\n 23. 兵五進一  象３退５   24. 炮九平五  士４進５\r\n 25. 馬三進四  卒７進１   26. 兵三進一  象５進７\r\n 27. 兵五平四  象７退５   28. 兵四平三  炮２退６\r\n 29. 兵三進一  馬７退８   30. 兵一進一  卒１進１\r\n 31. 兵三平二  馬８進７   32. 兵二平三  馬７退８\r\n 33. 炮五平四  炮２進２   34. 兵三平二  將５平４\r\n 35. 兵二進一  炮２退４   36. 炮四平一  馬８進６\r\n 37. 兵二平三  士５進４   38. 炮一進四  炮２平５\r\n 39. 炮一平六  將４平５   40. 炮六平二  將５平４\r\n 41. 兵三平四  馬６退８   42. 炮二退三  象５進７\r\n 43. 相三進五  炮５進４   44. 兵一進一  士４退５\r\n 45. 兵一平二  馬８進９   46. 兵四平三  象３進５\r\n 47. 兵三平二  馬９進８   48. 馬四進二  炮５平２\r\n 49. 兵二平三  炮２退３   50. 炮二平一  卒１進１\r\n 51. 炮一進六  將４進１   52. 仕五進六  卒１平２\r\n 53. 兵三進一  卒２進１   54. 馬二退三  炮２進２\r\n 55. 仕四進五  士５進６   56. 兵三平四  士６進５\r\n 57. 炮一退六  卒２進１   58. 馬三進四  炮２平５\r\n 59. 帥五平四  卒２平３   60. 相五進三  將４退１\r\n 61. 炮一退一  卒３平４   62. 仕五進六  炮５平２\r\n 63. 馬四進六  炮２退３   64. 兵四平五  士６退５\r\n 65. 馬六進五  炮２平４   66. 馬五退三  炮４平７\r\n 67. 炮一退一  炮７平４   68. 炮一平五  炮４進１\r\n 69. 馬三退五  炮４進１   70. 炮五進二  炮４退２\r\n 71. 馬五進七  炮４平３   72. 炮五進三  將４進１\r\n 73. 帥四平五  象５進３   74. 炮五退五\r\n")]
	public static void ExportGameAsPgn_ShouldReturnCorrectPgnString(string comp, string redPlayer, string blackPlayer, string redTeam, string blackTeam, GameResult result, string venue, string startingFen, int year, int month, int day, string moveRecord)
	{
		// Arrange
		XiangqiBuilder builder = new();
		DateTime competitionDate = new(year, month, day);

		XiangqiGame game = builder
			.WithStartingFen(startingFen)
			.WithRedPlayer(player =>
			{
				player.Name = redPlayer;
				player.Team = redTeam;
			})
			.WithBlackPlayer(player =>
			{
				player.Name = blackPlayer;
				player.Team = blackTeam;
			})
			.WithCompetition(option =>
			{
				option
					.WithGameDate(competitionDate)
					.WithLocation(venue)
					.WithName(comp);
			})
			.WithGameResult(result)
			.Build();

		IPgnGenerationService pgnGenerationService = new DefaultPgnGenerationService();

		// Act
		string pgnString = pgnGenerationService.GeneratePgnString(game);

		// Assert
		pgnString.Should().Contain("[Game \"Chinese Chess\"]");
		pgnString.Should().Contain($"[Event \"{comp}\"]");
		pgnString.Should().Contain($"[Site \"{venue}\"]");
		pgnString.Should().Contain($"[Red \"{redPlayer}\"]");
		pgnString.Should().Contain($"[Black \"{blackPlayer}\"]");
		pgnString.Should().Contain($"[RedTeam \"{redTeam}\"]");
		pgnString.Should().Contain($"[BlackTeam \"{blackTeam}\"]");
		pgnString.Should().Contain($"[FEN \"{startingFen}\"]");
		pgnString.Should().Contain($"[Site \"{venue}\"]");
		pgnString.Should().Contain($"[Result \"{EnumHelper<GameResult>.GetDisplayName(result)}\"]");
	}

	[Theory]
	[MemberData(nameof(UndoMoveMethodTestData))]
	public static void GameStateShouldBeModifiedCorrectly_WhenCallingUndoMoveMethod(UndoMoveMethodTestData data)
	{
		// Arrange
		XiangqiBuilder builder = new();
        
		XiangqiGame game = builder
			.WithStartingFen(data.StartingFen)
			.WithMoveRecord(data.MoveRecord)
			.Build();

		int totalMoves = game.GetMoveHistory().Count;
		int moveNumberToNavigateTo = totalMoves - data.NumberOfMovesToUndo;
		
		game.NavigateToMove(moveNumberToNavigateTo);
		
		// Act
		game.DeleteSubsequentMoves();

		// Assert
		game.CurrentFen.Should().Be(data.ExpectedFen);
		game.GetMoveHistory().Count.Should().Be(data.ExpectedMoveHistoryCount);
	}
}

public record MoveMethodTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);

public record UndoMoveMethodTestData(
	string StartingFen, 
	string MoveRecord, 
	int NumberOfMovesToUndo, 
	string ExpectedFen, 
	int ExpectedMoveHistoryCount,
	int ExpectedRoundNumber);