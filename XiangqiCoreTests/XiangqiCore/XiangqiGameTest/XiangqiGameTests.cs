using System.Text.RegularExpressions;
using xiangqi_core_test.XiangqiCore.MoveParserTest;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
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

		List<string> moves = GameRecordParser.Parse(gameRecord);

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

		List<string> moves = GameRecordParser.Parse(gameRecord);

		foreach (string move in moves)
		{
			bool isMoveSuccessful = game.MakeMove(move, MoveNotationType.TraditionalChinese);
			isMoveSuccessful.Should().BeTrue();
		}

		int startingRoundNumberCounter = initialMoveRoundNumber;

		// Act
		string pgnGameRecord = game.ExportMoveHistory();

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
		IPgnGenerationService pgnService = new DefaultPgnGenerationService();

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

		// Act
		string pgnString = game.GeneratePgn(pgnService);

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
}

public record MoveMethodTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);