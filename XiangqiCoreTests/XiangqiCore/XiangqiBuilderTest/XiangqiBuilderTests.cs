using XiangqiCore.Boards;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCoreTests.XiangqiCore.XiangqiBuilderTest;

public static class XiangqiBuilderTests
{
    [Fact]
    public static async Task ShouldCreateDefaultXiangqiGame_WhenCallingCreateWithDefaultMethodAsync()
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder.WithDefaultConfiguration().BuildAsync();

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
    public static async Task ShouldCreateDefaultXiangqiGameWithCustomFenString_WhenCallingUseCustomFenMethodAsync(string customFen)
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder.WithDefaultConfiguration().WithCustomFen(customFen).BuildAsync();

        // Assert
        xiangqiGame.InitialFenString.Should().Be(customFen);
    }

    [Theory]
    [InlineData("許銀川", "廣東隊")]
    [InlineData("Wang TianYi", "Hang Zhou")]
    public static async Task ShouldRecordRedPlayerWithNameAndTeam_WhenCallingHasRedPlayerMethodWithNameAndTeamAsync(string playerName, string teamName)
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder.WithDefaultConfiguration()
                                        .WithRedPlayer(player =>
                                        {
                                            player.Name = playerName;
                                            player.Team = teamName;
                                        })
                                        .BuildAsync();

        // Assert
        xiangqiGame.RedPlayer.Should().NotBeNull();

        xiangqiGame.RedPlayer.Name.Should().Be(playerName);
        xiangqiGame.RedPlayer.Team.Should().Be(teamName);
    }

    [Theory]
    [InlineData("許銀川", "廣東隊")]
    [InlineData("Wang TianYi", "Hang Zhou")]
    public static async Task ShouldRecordBlackPlayerWithNameAndTeam_WhenCallingHasBlackPlayerMethodWithNameAndTeamAsync(string playerName, string teamName)
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder.WithDefaultConfiguration()
                                        .WithBlackPlayer(player =>
                                        {
                                            player.Name = playerName;
                                            player.Team = teamName;
                                        })
                                        .BuildAsync();

        // Assert
        xiangqiGame.BlackPlayer.Should().NotBeNull();

        xiangqiGame.BlackPlayer.Name.Should().Be(playerName);
        xiangqiGame.BlackPlayer.Team.Should().Be(teamName);
    }

    [Fact]
    public static async Task ShouldCreateAGameWithCompetitionName_WhenCallingPlayedInCompetitionAsync()
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder.WithDefaultConfiguration()
                            .WithCompetition(option =>
                            {
                                option
                                    .WithGameDate(new DateTime(2017, 12, 1))
                                    .WithLocation("香港")
                                    .WithName("2017全港個人賽");
                            })
                            .BuildAsync();

        // Assert
        xiangqiGame.Competition.Name.Should().Be("2017全港個人賽");
        xiangqiGame.Competition.Location.Should().Be("香港");
        xiangqiGame.Competition.GameDate.Should().Be(new DateTime(2017, 12, 1));
    }

    [Theory]
    [InlineData(GameResult.RedWin, "1-0")]
    [InlineData(GameResult.BlackWin, "0-1")]
    [InlineData(GameResult.Draw, "1/2-1/2")]
    public static async Task ShouldCreateAGameWithCorrectResult_WhenCallingWithResultAsync(GameResult result, string expectedResultText)
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder
                                    .WithDefaultConfiguration()
                                    .WithGameResult(result)
                                    .BuildAsync();

        // Assert
        xiangqiGame.GameResultString.Should().Be(expectedResultText);
        xiangqiGame.GameResult.Should().Be(result);
    }

    [Fact]
    public static async Task ShouldCreateBoardInstanceWithSpecifiedPieceAndLocation_WhenCallingUseCustomPositionAsync()
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
        XiangqiGame xiangqiGame = await builder
                                    .WithDefaultConfiguration()
                                    .WithBoardConfig(config)
                                    .BuildAsync();

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
    [InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0", "1. 相三進五  炮２平４    2. 兵七進一  馬２進１\r\n  3. 馬八進七  車１平２    4. 車九平八  車２進４\r\n  5. 炮八平九  車２平４    6. 馬二進三  卒７進１\r\n  7. 炮二平一  馬８進７    8. 車一平二  車９平８\r\n  9. 車二進四  炮８平９   10. 車二進五  馬７退８\r\n 11. 車八進一  馬８進７   12. 車八平二  卒１進１\r\n 13. 車二進三  象７進５   14. 兵三進一  馬１進２\r\n 15. 馬三進四  車４平６   16. 兵三進一  車６平７\r\n 17. 兵一進一  炮９退１   18. 炮一平三  車７平６\r\n 19. 炮九退一  炮４進６   20. 炮九平七  炮９平３\r\n 21. 炮三平四  車６平７   22. 車二退三  炮４退６\r\n 23. 炮七平三  車７平５   24. 炮四平三  車５平６\r\n 25. 車二進三  炮４平１   26. 兵七進一  車６平３\r\n 27. 炮三進六  炮１平７   28. 馬四進五  車３平６\r\n 29. 馬五進三  車６進３   30. 馬七退五  馬２進３\r\n 31. 炮三進四  車６退４   32. 炮三退二  車６退１\r\n 33. 馬三退二  馬３進４   34. 馬五進三  車６進５\r\n 35. 車二退三  車６平５   36. 仕六進五  車５平７\r\n 37. 馬二進四  士４進５   38. 車二平四  車７退１\r\n 39. 車四進一  馬４退５   40. 炮三平五  馬５退３\r\n 41. 馬四進六  炮３平４   42. 車四平七  馬３退４\r\n 43. 炮五退二  馬４進５   44. 馬六進八  炮４進３\r\n 45. 馬八退七  炮４平７   46. 帥五平六  車７平１\r\n 47. 炮五平三  卒１進１   48. 車七進二  馬５退３\r\n 49. 馬七退五  車１平５   50. 馬五進三  炮７平８\r\n 51. 炮三平二  士５進６   52. 車七平六  士６進５\r\n 53. 馬三進一  車５平７   54. 兵一進一  炮８進２\r\n 55. 帥六進一  將５平６   56. 炮二平四  將６平５\r\n 57. 炮四平五  將５平６   58. 兵一進一  象５退７\r\n 59. 馬一進二  象３進５   60. 兵一進一  車７平５\r\n 61. 車六退二  馬３進２   62. 馬二退三  將６平５\r\n 63. 馬三退四  車５退２   64. 車六平八  卒１進１\r\n 65. 馬四進二  車５平４   66. 仕五進六  將５平４", 66)]
    [InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0", " 1. 相三進五  馬２進３    2. 兵三進一  卒３進１\r\n  3. 馬二進三  馬３進４    4. 仕四進五  炮８平５\r\n  5. 馬八進七  馬８進７    6. 馬三進二  炮２進３\r\n  7. 馬二進三  車９平８    8. 炮二平三  炮５進４\r\n  9. 車九進一  炮５退１   10. 車九平六  馬４進３\r\n 11. 車六進三  炮２平３   12. 車六退一  車１平２\r\n 13. 炮八平九  車２進６   14. 炮九退一  車８進３\r\n 15. 兵三進一  象７進５   16. 炮九進五  象５進７\r\n 17. 馬三進五  車８退２   18. 車六進六  將５進１\r\n 19. 馬五進七  炮３退４   20. 炮九進二  車２退５\r\n 21. 炮九平七  車２平３   22. 炮三進五  車３進１\r\n 23. 炮三進二  車３平８   24. 車一平四  象３進５\r\n 25. 車六平五  將５平４   26. 炮三平二  象５退７\r\n 27. 車五平四  車８退１   28. 車四進八  將４進１\r\n 29. 前車平六  將４平５   30. 馬七進五  後車進１\r\n 31. 車四退三  車８進７   32. 仕五退四  車８退３\r\n 33. 車四平七  前車平５   34. 仕六進五  車８平２\r\n 35. 車七平三  車５平６   36. 車三進二  將５退１\r\n 37. 車三進一  車６退５   38. 車三進一  車６平８\r\n 39. 車三退三  卒５進１   40. 車三退一  車２進８\r\n 41. 帥五平六  馬３進２   42. 帥六進一  馬２退３\r\n 43. 帥六退一  車２退８   44. 車三平五  將５平６\r\n 45. 車六退六  馬３進２   46. 帥六平五  車８進４\r\n 47. 車五平三  車８平６   48. 車三進三  將６進１\r\n 49. 車六進四  炮５退３   50. 車三退一  將６退１\r\n 51. 車三平五\r\n", 51)]
    [InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0", "1. 相三進五  炮２平６    2. 馬八進七  馬２進３\r\n  3. 車九平八  馬８進９    4. 馬二進四  炮８平７\r\n  5. 車一平二  車９平８    6. 兵七進一  車８進４\r\n  7. 炮二平一  卒９進１    8. 車二進五  馬９進８\r\n  9. 兵三進一  車１平２   10. 炮八進六  馬８進６\r\n 11. 馬四進二  士４進５   12. 炮八平七  車２進９\r\n 13. 馬七退八  馬３退１   14. 炮一進三  馬６進４\r\n 15. 相七進九  炮６平２   16. 馬八進六  炮７平５\r\n 17. 仕四進五  炮２進６   18. 帥五平四  馬４進３\r\n 19. 馬六進七  炮２退１   20. 馬二進一  炮５進４\r\n 21. 炮一進一  炮５平６   22. 炮一平五  象３進５\r\n 23. 炮七平八  炮２退３   24. 馬七退六  炮２平６\r\n 25. 帥四平五  後炮平５   26. 馬一進二  馬３退４\r\n 27. 炮五平四  將５平４   28. 炮八退三  馬１進３\r\n 29. 炮八平六  炮５退１   30. 炮六退一  卒３進１\r\n 31. 兵七進一  象５進３   32. 馬二進三  士５進６\r\n 33. 炮四退一  炮５進１   34. 相九退七  馬３進５\r\n 35. 炮六進二  炮６平１   36. 馬六進七  炮１平２\r\n 37. 帥五平四  將４進１   38. 炮六平三  炮５進２\r\n 39. 馬七退六  炮５平６   40. 炮三平四  馬５進４\r\n 41. 兵三進一  炮６退３   42. 馬三退四  士６進５\r\n 43. 兵一進一  後馬進６   44. 兵三平二  馬６退８\r\n 45. 炮四退四  馬８進７   46. 帥四平五  炮２進２\r\n 47. 馬六進八  炮２平６\r\n", 47)]
    public static async Task ShouldCreateGameWithMoveRecord_WhenCallingWithMoveRecordAsync(string startingFen, string moveRecord, int expectedRoundCount)
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = await builder
                                    .WithCustomFen(startingFen)
                                    .WithMoveRecord(moveRecord)
                                    .BuildAsync();

        // Assert
        int expectedNumberOfRounds = xiangqiGame
                                        .MoveHistory
                                        .GroupBy(x => x.RoundNumber)
                                        .Select(x => x.Key)
                                        .Count();

        expectedRoundCount.Should().Be(expectedRoundCount);
    }
}
