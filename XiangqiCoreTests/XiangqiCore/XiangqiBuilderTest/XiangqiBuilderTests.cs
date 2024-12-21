using XiangqiCore.Boards;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCoreTests.XiangqiCore.XiangqiBuilderTest;

public static class XiangqiBuilderTests
{
	public static IEnumerable<object[]> RandomisePositionFromFenTestData
	{
		get
		{
			// Randomise from Fen string
			yield return new object[] { 
				new RandomisePositionTestData(
					initialFen: "4k4/9/9/r8/9/9/9/9/9/5K3 w - - 0 0", 
					allowCheck: false, 
					pieceCounts: new(
						RedPieces: new Dictionary<PieceType, int>()
						{
							{ PieceType.King, 1 },
						},
						BlackPieces: new Dictionary<PieceType, int>()
						{
							{ PieceType.King, 1 },
							{ PieceType.Rook, 1 },
						}
					))};

			yield return new object[] {
				new RandomisePositionTestData(
					initialFen: "9/9/5k3/9/9/9/9/5A3/2C1A4/4K4 w - - 0 0",
					allowCheck: false,
					pieceCounts: new(
						RedPieces: new Dictionary<PieceType, int>()
						{
							{ PieceType.King, 1 },
							{ PieceType.Advisor, 2 },
							{ PieceType.Cannon, 1 },
						},
						BlackPieces: new Dictionary<PieceType, int>()
						{
							{ PieceType.King, 1 },
						}
					))};
		}
	}

	[Fact]
    public static async Task ShouldCreateDefaultXiangqiGame_WhenCallingCreateWithDefaultMethodAsync()
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = builder.WithDefaultConfiguration().Build();

        // Assert
        xiangqiGame.InitialFenString.Should().Be("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1");

        xiangqiGame.SideToMove.Should().Be(Side.Red);

        xiangqiGame.RedPlayer.Name.Should().Be("Unknown");
        xiangqiGame.RedPlayer.Team.Should().Be("Unknown");

        xiangqiGame.BlackPlayer.Name.Should().Be("Unknown");
        xiangqiGame.BlackPlayer.Team.Should().Be("Unknown");
    }

	[Theory]
	[InlineData("3akab2/5r3/4b1cCn/p3p3p/2p6/6N2/PrP1N3P/4BC3/9/2BAKA1R1 b - - 2 15")]
	[InlineData("r3kabr1/4a4/1cn1b1n2/p1p1p3p/6p2/1CP6/P3P1PcP/2N1C1N2/3R5/2BAKABR1 b - - 15 8")]
	[InlineData("1C2kabr1/4a4/r3b1n2/p1p1p3p/6pc1/2P1P4/PR4PR1/BCN6/4A4/4KAB1c b - - 5 22")]
	[InlineData("2bakn3/3ca4/2n1b4/p1p2R1Np/1r4p2/2P1p2c1/P5r1P/R3C1N2/3C5/2BAKAB2 b - - 11 22")]
	public static void ShouldCreateDefaultXiangqiGameWithCustomFenString_WhenCallingUseCustomFenMethod(string customFen)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDefaultConfiguration().WithStartingFen(customFen).Build();

		// Assert
		xiangqiGame.InitialFenString.Should().Be(customFen);
	}

	[Theory]
	[InlineData("許銀川", "廣東隊")]
	[InlineData("Wang TianYi", "Hang Zhou")]
	public static void ShouldRecordRedPlayerWithNameAndTeam_WhenCallingHasRedPlayerMethodWithNameAndTeam(string playerName, string teamName)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDefaultConfiguration()
										.WithRedPlayer(player =>
										{
											player.Name = playerName;
											player.Team = teamName;
										})
										.Build();

		// Assert
		xiangqiGame.RedPlayer.Should().NotBeNull();

		xiangqiGame.RedPlayer.Name.Should().Be(playerName);
		xiangqiGame.RedPlayer.Team.Should().Be(teamName);
	}

	[Theory]
	[InlineData("許銀川", "廣東隊")]
	[InlineData("Wang TianYi", "Hang Zhou")]
	public static void ShouldRecordBlackPlayerWithNameAndTeam_WhenCallingHasBlackPlayerMethodWithNameAndTeam(string playerName, string teamName)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDefaultConfiguration()
										.WithBlackPlayer(player =>
										{
											player.Name = playerName;
											player.Team = teamName;
										})
										.Build();

		// Assert
		xiangqiGame.BlackPlayer.Should().NotBeNull();

		xiangqiGame.BlackPlayer.Name.Should().Be(playerName);
		xiangqiGame.BlackPlayer.Team.Should().Be(teamName);
	}

	[Fact]
	public static void ShouldCreateAGameWithCompetitionName_WhenCallingPlayedInCompetition()
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDefaultConfiguration()
							.WithCompetition(option =>
							{
								option
									.WithGameDate(new DateTime(2017, 12, 1))
									.WithLocation("香港")
									.WithName("2017全港個人賽");
							})
							.Build();

		// Assert
		xiangqiGame.Competition.Name.Should().Be("2017全港個人賽");
		xiangqiGame.Competition.Location.Should().Be("香港");
		xiangqiGame.Competition.GameDate.Should().Be(new DateTime(2017, 12, 1));
	}

	[Theory]
	[InlineData("許銀川", "呂欽", GameResult.RedWin, "許銀川先勝呂欽")]
	[InlineData("傅光明", "胡榮華", GameResult.BlackWin, "傅光明先負胡榮華")]
	[InlineData("王天一", "鄭惟恫", GameResult.Draw, "王天一先和鄭惟恫")]
	[InlineData("趙國榮", "柳大華", GameResult.Unknown, "趙國榮對柳大華")]
	public static void ShouldCreateGameNameAutomatically_WhenDetailsNotProvided(string redPlayerName, string blackPlayerName, GameResult gameResult, string expectedGameName)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDefaultConfiguration()
							.WithRedPlayer(player =>
							{
								player.Name = redPlayerName;
							})
							.WithBlackPlayer(player =>
							{
								player.Name = blackPlayerName;
							})
							.WithGameResult(gameResult)
							.Build();

		// Assert
		xiangqiGame.GameName.Should().Be(expectedGameName);
	}

	[Theory]
	[InlineData("許銀川", "呂欽", GameResult.RedWin, "2003許銀川先勝呂欽")]
	[InlineData("傅光明", "胡榮華", GameResult.BlackWin, "全國個人賽精采對局: 傅光明先負胡榮華")]
	[InlineData("王天一", "鄭惟恫", GameResult.Draw, "財神杯十番: 王天一先和鄭惟恫")]
	[InlineData("趙國榮", "柳大華", GameResult.Unknown, "test")]
	public static void ShouldCreateCorrectGameName_WhenCallingWithGameName(string redPlayerName, string blackPlayerName, GameResult gameResult, string expectedGameName)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDefaultConfiguration()
							.WithRedPlayer(player =>
							{
								player.Name = redPlayerName;
							})
							.WithBlackPlayer(player =>
							{
								player.Name = blackPlayerName;
							})
							.WithGameName(expectedGameName)
							.WithGameResult(gameResult)
							.Build();

		// Assert
		xiangqiGame.GameName.Should().Be(expectedGameName);
	}

	[Theory]
	[InlineData(GameResult.RedWin, "1-0")]
	[InlineData(GameResult.BlackWin, "0-1")]
	[InlineData(GameResult.Draw, "1/2-1/2")]
	public static void ShouldCreateAGameWithCorrectResult_WhenCallingWithResult(GameResult result, string expectedResultText)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder
									.WithDefaultConfiguration()
									.WithGameResult(result)
									.Build();

		// Assert
		xiangqiGame.GameResultString.Should().Be(expectedResultText);
		xiangqiGame.GameResult.Should().Be(result);
	}

	[Fact]
	public static void ShouldCreateBoardInstanceWithSpecifiedPieceAndLocation_WhenCallingUseCustomPosition()
	{
		// Arrange
		XiangqiBuilder builder = new();
		EmptyPiece emptyPiece = new();

		Coordinate redRookCoordinate = new(5, 6);
		Coordinate redKingCoordinate = new(5, 1);
		Coordinate blackKingCoordinate = new(4, 10);
		Rook redRook = (Rook)PieceFactory.Create(PieceType.Rook, Side.Red, redRookCoordinate);
		King redKing = (King)PieceFactory.Create(PieceType.King, Side.Red, redKingCoordinate);
		King blackKing = (King)PieceFactory.Create(PieceType.King, Side.Black, blackKingCoordinate);

		BoardConfig config = new() { };

		config.AddPiece(PieceType.Rook, Side.Red, redRookCoordinate);
		config.AddPiece(PieceType.King, Side.Red, redKingCoordinate);
		config.AddPiece(PieceType.King, Side.Black, blackKingCoordinate);

		// Act
		XiangqiGame xiangqiGame = builder
									.WithDefaultConfiguration()
									.WithBoardConfig(config)
									.Build();

		// Assert
		xiangqiGame.Board.GetPieceAtPosition(redRookCoordinate).Should().NotBe(emptyPiece);
		xiangqiGame.Board.GetPieceAtPosition(redKingCoordinate).Should().NotBe(emptyPiece);
		xiangqiGame.Board.GetPieceAtPosition(blackKingCoordinate).Should().NotBe(emptyPiece);

		xiangqiGame.Board.GetPieceAtPosition(redRookCoordinate).Should().Be(redRook);
		xiangqiGame.Board.GetPieceAtPosition(redKingCoordinate).Should().Be(redKing);
		xiangqiGame.Board.GetPieceAtPosition(blackKingCoordinate).Should().Be(blackKing);
	}

	[Theory]
	[InlineData("r2akab1r/9/nCc1b1n1c/p1p1p1p1p/9/2P3P2/P3P3P/2N1BC3/9/R1BAKA1NR b - - 11 6", "6. ...  車９平８\r\n  7. 馬二進三  車８進４    8. 車九平八  車１平２\r\n  9. 車一平二  車８平６   10. 仕四進五  卒３進１\r\n 11. 兵七進一  炮３進５   12. 炮四平七  車６平３\r\n 13. 炮七進二  卒７進１   14. 車二進七  馬７進６\r\n 15. 炮七平九  卒７進１   16. 相五進三  馬６退４", 11)]
	[InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1", "1. 相三進五  炮２平４    2. 兵七進一  馬２進１\r\n  3. 馬八進七  車１平２    4. 車九平八  車２進４\r\n  5. 炮八平九  車２平４    6. 馬二進三  卒７進１\r\n  7. 炮二平一  馬８進７    8. 車一平二  車９平８\r\n  9. 車二進四  炮８平９   10. 車二進五  馬７退８\r\n 11. 車八進一  馬８進７   12. 車八平二  卒１進１\r\n 13. 車二進三  象７進５   14. 兵三進一  馬１進２\r\n 15. 馬三進四  車４平６   16. 兵三進一  車６平７\r\n 17. 兵一進一  炮９退１   18. 炮一平三  車７平６\r\n 19. 炮九退一  炮４進６   20. 炮九平七  炮９平３\r\n 21. 炮三平四  車６平７   22. 車二退三  炮４退６\r\n 23. 炮七平三  車７平５   24. 炮四平三  車５平６\r\n 25. 車二進三  炮４平１   26. 兵七進一  車６平３\r\n 27. 炮三進六  炮１平７   28. 馬四進五  車３平６\r\n 29. 馬五進三  車６進３   30. 馬七退五  馬２進３\r\n 31. 炮三進四  車６退４   32. 炮三退二  車６退１\r\n 33. 馬三退二  馬３進４   34. 馬五進三  車６進５\r\n 35. 車二退三  車６平５   36. 仕六進五  車５平７\r\n 37. 馬二進四  士４進５   38. 車二平四  車７退１\r\n 39. 車四進一  馬４退５   40. 炮三平五  馬５退３\r\n 41. 馬四進六  炮３平４   42. 車四平七  馬３退４\r\n 43. 炮五退二  馬４進５   44. 馬六進八  炮４進３\r\n 45. 馬八退七  炮４平７   46. 帥五平六  車７平１\r\n 47. 炮五平三  卒１進１   48. 車七進二  馬５退３\r\n 49. 馬七退五  車１平５   50. 馬五進三  炮７平８\r\n 51. 炮三平二  士５進６   52. 車七平六  士６進５\r\n 53. 馬三進一  車５平７   54. 兵一進一  炮８進２\r\n 55. 帥六進一  將５平６   56. 炮二平四  將６平５\r\n 57. 炮四平五  將５平６   58. 兵一進一  象５退７\r\n 59. 馬一進二  象３進５   60. 兵一進一  車７平５\r\n 61. 車六退二  馬３進２   62. 馬二退三  將６平５\r\n 63. 馬三退四  車５退２   64. 車六平八  卒１進１\r\n 65. 馬四進二  車５平４   66. 仕五進六  將５平４", 66)]
	[InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1", " 1. 相三進五  馬２進３    2. 兵三進一  卒３進１\r\n  3. 馬二進三  馬３進４    4. 仕四進五  炮８平５\r\n  5. 馬八進七  馬８進７    6. 馬三進二  炮２進３\r\n  7. 馬二進三  車９平８    8. 炮二平三  炮５進４\r\n  9. 車九進一  炮５退１   10. 車九平六  馬４進３\r\n 11. 車六進三  炮２平３   12. 車六退一  車１平２\r\n 13. 炮八平九  車２進６   14. 炮九退一  車８進３\r\n 15. 兵三進一  象７進５   16. 炮九進五  象５進７\r\n 17. 馬三進五  車８退２   18. 車六進六  將５進１\r\n 19. 馬五進七  炮３退４   20. 炮九進二  車２退５\r\n 21. 炮九平七  車２平３   22. 炮三進五  車３進１\r\n 23. 炮三進二  車３平８   24. 車一平四  象３進５\r\n 25. 車六平五  將５平４   26. 炮三平二  象５退７\r\n 27. 車五平四  車８退１   28. 車四進八  將４進１\r\n 29. 前車平六  將４平５   30. 馬七進五  後車進１\r\n 31. 車四退三  車８進７   32. 仕五退四  車８退３\r\n 33. 車四平七  前車平５   34. 仕六進五  車８平２\r\n 35. 車七平三  車５平６   36. 車三進二  將５退１\r\n 37. 車三進一  車６退５   38. 車三進一  車６平８\r\n 39. 車三退三  卒５進１   40. 車三退一  車２進８\r\n 41. 帥五平六  馬３進２   42. 帥六進一  馬２退３\r\n 43. 帥六退一  車２退８   44. 車三平五  將５平６\r\n 45. 車六退六  馬３進２   46. 帥六平五  車８進４\r\n 47. 車五平三  車８平６   48. 車三進三  將６進１\r\n 49. 車六進四  炮５退３   50. 車三退一  將６退１\r\n 51. 車三平五\r\n", 51)]
	[InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1", "1. 相三進五  炮２平６    2. 馬八進七  馬２進３\r\n  3. 車九平八  馬８進９    4. 馬二進四  炮８平７\r\n  5. 車一平二  車９平８    6. 兵七進一  車８進４\r\n  7. 炮二平一  卒９進１    8. 車二進五  馬９進８\r\n  9. 兵三進一  車１平２   10. 炮八進六  馬８進６\r\n 11. 馬四進二  士４進５   12. 炮八平七  車２進９\r\n 13. 馬七退八  馬３退１   14. 炮一進三  馬６進４\r\n 15. 相七進九  炮６平２   16. 馬八進六  炮７平５\r\n 17. 仕四進五  炮２進６   18. 帥五平四  馬４進３\r\n 19. 馬六進七  炮２退１   20. 馬二進一  炮５進４\r\n 21. 炮一進一  炮５平６   22. 炮一平五  象３進５\r\n 23. 炮七平八  炮２退３   24. 馬七退六  炮２平６\r\n 25. 帥四平五  後炮平５   26. 馬一進二  馬３退４\r\n 27. 炮五平四  將５平４   28. 炮八退三  馬１進３\r\n 29. 炮八平六  炮５退１   30. 炮六退一  卒３進１\r\n 31. 兵七進一  象５進３   32. 馬二進三  士５進６\r\n 33. 炮四退一  炮５進１   34. 相九退七  馬３進５\r\n 35. 炮六進二  炮６平１   36. 馬六進七  炮１平２\r\n 37. 帥五平四  將４進１   38. 炮六平三  炮５進２\r\n 39. 馬七退六  炮５平６   40. 炮三平四  馬５進４\r\n 41. 兵三進一  炮６退３   42. 馬三退四  士６進５\r\n 43. 兵一進一  後馬進６   44. 兵三平二  馬６退８\r\n 45. 炮四退四  馬８進７   46. 帥四平五  炮２進２\r\n 47. 馬六進八  炮２平６\r\n", 47)]
	public static void ShouldCreateGameWithMoveRecord_WhenCallingWithMoveRecord(string startingFen, string moveRecord, int expectedRoundCount)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder
									.WithStartingFen(startingFen)
									.WithMoveRecord(moveRecord)
									.Build();

		// Assert
		int expectedNumberOfRounds = xiangqiGame
										.MoveHistory
										.GroupBy(x => x.RoundNumber)
										.Select(x => x.Key)
										.Count();

		expectedRoundCount.Should().Be(expectedRoundCount);
	}

	[Fact]
	public static void ShouldCreateGameWithCorrectInfo_WhenCallingWithDpxqRecord()
	{
		// Arrange
		string sample = @"标题: 杭州环境集团队 王天一 和 四川成都懿锦金弈队 武俊强
分类: 全国象棋甲级联赛
赛事: 2023年腾讯棋牌天天象棋全国象棋甲级联赛
轮次: 决赛
布局: E42 对兵互进右马局
红方: 杭州环境集团队 王天一
黑方: 四川成都懿锦金弈队 武俊强
结果: 和棋
日期: 2023.12.10
地点: 重庆丰都
组别: 杭州-四川
台次: 第04台
评论: 中国象棋协会
作者: 张磊
备注: 第3局
记时规则: 5分＋3秒
红方用时: 6分
黑方用时: 6分钟
棋局类型: 全局
棋局性质: 超快棋
红方团体: 杭州环境集团队
红方姓名: 王天一
黑方团体: 四川成都懿锦金弈队
黑方姓名: 武俊强
棋谱主人: 东萍公司
棋谱价值: 0
浏览次数: 3086
来源网站: 1791148
 
　　第3局
 
【主变: 和棋】
　　1.　兵七进一　　卒７进１　
　　2.　马八进七　　马８进７　
　　3.　马二进一　　象３进５　
　　4.　炮八平九　　马２进３　
　　5.　车九平八　　车１平２　
　　6.　炮二平四　　马７进８　
　　7.　炮九进四　　车９进１　
　　8.　车八进六　　车９平６　
　　9.　仕四进五　　炮２退１　
　　10. 炮九平七　　卒９进１　
　　11. 相三进五　　车６进３　
　　12. 车一平三　　马８进９　
　　13. 车八退二　　炮２平８　
　　14. 车八进五　　马３退２　
　　15. 兵三进一　　卒７进１　
　　16. 相五进三　　马２进１　
　　17. 炮七平九　　车６平１　
　　18. 相三退五　　后炮平３　
　　19. 炮九平八　　车１平２　
　　20. 炮八平九　　车２平１　
　　21. 炮九平八　　车１平２　
　　22. 炮八平九　　象５进７　
　　23. 车三进三　　马９退８　
　　24. 车三进一　　炮８平３　
　　25. 马七退九　　车２平１　
　　26. 炮九平八　　车１平２　
　　27. 炮八平九　　车２平１　
　　28. 炮九平八　　象７退５　
　　29. 炮八退四　　车１进２　
　　30. 炮八平七　　马１进２　
　　31. 炮七进五　　车１进２　
　　32. 炮七平八　　车１平４　
　　33. 相七进九　　马２进３　
　　34. 车三平二　　车４退４　
　　35. 马一进三　　马８退６　
　　36. 车二平四　　马６进８　
　　37. 车四进一　　车４平６　
　　38. 马三进四　　卒５进１　
　　39. 炮八平七　　马８进６　
　　40. 炮七退四　　炮３进５　
　　41. 马四进二　　士４进５　
　　42. 相九退七
 
棋谱由 http://www.dpxq.com/ 生成";

		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder
									.WithDpxqGameRecord(sample)
									.Build();

		// Assert
		xiangqiGame.Competition.Location.Should().Be("重庆丰都");
		xiangqiGame.Competition.Name.Should().Be("2023年腾讯棋牌天天象棋全国象棋甲级联赛");
		xiangqiGame.Competition.GameDate.Should().Be(new DateTime(2023, 12, 10));

		xiangqiGame.RedPlayer.Name.Should().Be("王天一");
		xiangqiGame.RedPlayer.Team.Should().Be("杭州环境集团队");
		xiangqiGame.BlackPlayer.Name.Should().Be("武俊强");
		xiangqiGame.BlackPlayer.Team.Should().Be("四川成都懿锦金弈队");

		xiangqiGame.GameResult.Should().Be(GameResult.Draw);
		xiangqiGame.MoveHistory.Should().HaveCount(41 * 2 + 1);
		xiangqiGame.MoveHistory.Last().FenAfterMove.Should().Be("4kab2/4a4/4b4/7N1/4p3p/2P2n3/2c1P4/4BC3/4A4/2BAK4 b - - 3 42");
	}

	[Fact]
	public static void ShouldCreateGameWithCommentsCorrectly_WhenDpxqRecordContainsExtraMovesInComments()
	{
		// Arrange

		string sample2 = @"标题: 南方 许银川 胜 北方 刘殿中
分类: 其他大师或以上级别大赛
赛事: 2005年“大新杯”南北棋王对抗赛
轮次: 第01轮
布局: B20 中炮对左三步虎
红方: 南方 许银川
黑方: 北方 刘殿中
结果: 红方胜
日期: 2005.11.12
地点: 佛山顺德乐从
评论: 孙志伟
结束方式: 认负
记时规则: 30分包干
棋局类型: 全局
棋局性质: 慢棋
红方团体: 南方
红方姓名: 许银川
黑方团体: 北方
黑方姓名: 刘殿中
棋谱主人: mdf800921
棋谱价值: 4
浏览次数: 7883
来源网站: www.dpxq.com
 
 
【主变: 红胜】
1.炮二平五 马８进７

2.马二进三 车９平８

3.兵七进一 卒７进１

4.马八进七 炮８平９

5.炮八平九

这是2005年全国象棋个人赛之后举办的“大新杯”南北棋王对抗赛第1轮，南方许银川与北方刘殿中的一局角斗。双方以中炮七路马对三步虎列阵。红平边炮准备抢出左车，不落俗套的走法。一般多走车一进一或炮八进二，双方另有不同攻守。
　　5. …………　　马２进３

6.车九平八 车１平２

7.车八进六 车８进８ ①
　　黑方进车红方下二路。意在防止红方车一进一出动右车。似不如改走炮2平1，红如车八进三(如车八平七，炮1退1，黑有反击之势)，则马3退2，车一进一，车8进5，兵五进一，炮1平5，黑可对抗。
　　8.炮九进四 士４进５

9.车一进一 车８退２
黑方退车，保持变化的走法。如改走车8平9，则马三退一，红方多兵占优。
　　10.马七进六 车８平７

11.车一进一 卒７进１

12.炮五平九 炮２平１

13.车八进三 马３退２

14.相三进五 马２进３

15.前炮退二 马７进８

16.后炮平七 马８进６

17.马六进四

红方跃进骑河马，巧妙地牵制了黑方过河的车马卒三子，为扩先取势创造了有利条件。
　　17. …………　　象３进５

18.炮九平八 车７平６

19.炮八平四 卒７进１ ②
　　黑方不如改走卒7平6，红如接走马四进六，士5进4，兵七进一，马3进1，要比实战走法为好。
　　20.马四进六 士５进４

21.炮四平一

如图形势，红方平炮邀兑黑方边炮，可使黑方右翼马炮失去策应之势，是扩大主动的简明有力之着。
　　21. …………　　炮９进３

22.兵一进一 卒７进１

23.兵七进一

红方冲兵，在黑方右翼发难，攻击点十分准确。
　　23. …………　　车６平５

24.兵七进一 车５平３

25.兵七进一 车３进１

26.马六退七

红方回马关车，是以退为进的老练走法。
　　26. …………　　士４退５

27.车一平三 炮１进１

28.车三进一 卒５进１
黑如改走车3进1，则车五平八，黑方亦难应付。
　　29.车三进三 炮１退３

30.兵七进一

红方不平车吃卒，而冲七路兵直逼九宫，紧凑有力的走法。
　　30. …………　　车３退１

31.车三平九 炮１平４

32.兵七平六

如图形势，红方平兵搞死黑炮，为取胜奠定了基础。
　　32. …………　　卒５进１

33.兵六进一 将５平４

34.车九平一 士５进４

35.车一平六 士６进５

36.兵一进一 车３平９

37.兵一平二 车９平８

38.兵二平一 车８平６

39.车六退一 车６退１

40.兵九进一 卒５平４

41.马七进六

黑方少子不敌，遂停钟认负。
 
变①接主变第7回合

{ 1rbakabr1 / 9 / 1cn3n1c / pRp1p3p / 6p2 / 2P6 / P3P1P1P / C1N1C1N2 / 9 / 2BAKAB1R b}
	7. …………　　炮２平１

8.车八进三

如车八平七，炮1退1，黑有反击之势。
　　8. …………　　马３退２

9.车一进一 车８进５

10.兵五进一 炮１平５
黑可对抗。


变②接主变第19回合

{ 4kab2 / 4a4 / c1n1b3c / 2p1p3p / 5N3 / 2P2Cp2 / P3Pr2P / 2C1B1N1R / 9 / 2BAKA3 b}
	19. …………　　卒７平６

20.马四进六 士５进４

21.兵七进一 马３进１
要比实战走法为好。


棋谱由 http://www.dpxq.com/ 生成";

		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithDpxqGameRecord(sample2).Build();

		// Assert
		xiangqiGame.Competition.Location.Should().Be("佛山顺德乐从");
		xiangqiGame.Competition.Name.Should().Be("2005年“大新杯”南北棋王对抗赛");
		xiangqiGame.Competition.GameDate.Should().Be(new DateTime(2005, 11, 12));

		xiangqiGame.RedPlayer.Name.Should().Be("许银川");
		xiangqiGame.RedPlayer.Team.Should().Be("南方");
		xiangqiGame.BlackPlayer.Name.Should().Be("刘殿中");
		xiangqiGame.BlackPlayer.Team.Should().Be("北方");

		xiangqiGame.GameResult.Should().Be(GameResult.RedWin);
		xiangqiGame.MoveHistory.Should().HaveCount(40 * 2 + 1);
	}

	[Theory]
	[MemberData(nameof(RandomisePositionFromFenTestData))]
	public static void ShouldRandomizePiecePosition_WhenCallingRandomisePiecePosition(RandomisePositionTestData testData)
	{
		// Arrange
		XiangqiBuilder builder = new();

		// Act
		XiangqiGame xiangqiGame = builder.WithStartingFen(testData.InitialFen)
			.RandomisePosition(allowCheck: testData.AllowCheck)
			.Build();

		// Assert
		Piece[,] boardPosition = xiangqiGame.BoardPosition;

		Assert.Multiple(() =>
		{
			// Verify the number of pieces
			foreach (var pieceType in Enum.GetValues<PieceType>())
			{
				int expectedRedCount = testData.PieceCounts.GetPieceCount(pieceType, Side.Red);
				int expectedBlackCount = testData.PieceCounts.GetPieceCount(pieceType, Side.Black);

				int actualRedCount = boardPosition.GetPiecesByType(pieceType, Side.Red).Count();
				int actualBlackCount = boardPosition.GetPiecesByType(pieceType, Side.Black).Count();

				actualBlackCount.Should().Be(expectedBlackCount);
				actualRedCount.Should().Be(expectedRedCount);
			}
		});
	}
}

public record RandomisePositionTestData(bool allowCheck, PieceCounts pieceCounts)
{
	public RandomisePositionTestData(string initialFen, bool allowCheck, PieceCounts pieceCounts): this(allowCheck, pieceCounts) 
	{ 
		InitialFen = initialFen; 
	}

	public RandomisePositionTestData(BoardConfig boardConfig, bool allowCheck, PieceCounts pieceCounts) : this(allowCheck, pieceCounts) 
	{ 
		BoardConfig = boardConfig; 
	}

	public bool AllowCheck { get; init; } = allowCheck;
	public PieceCounts PieceCounts { get; init; } = pieceCounts;
	public string InitialFen { get; init; }
	public BoardConfig? BoardConfig { get; init; }
}