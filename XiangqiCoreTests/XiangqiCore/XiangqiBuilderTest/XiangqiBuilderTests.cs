﻿using XiangqiCore.Boards;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCoreTests.XiangqiCore.XiangqiBuilderTest;

public static class XiangqiBuilderTests
{
    [Fact]
    public static void ShouldCreateDefaultXiangqiGame_WhenCallingCreateWithDefaultMethod()
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration().Build();

        // Assert
        xiangqiGame.InitialFenString.Should().Be("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0");

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
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration().UseCustomFen(customFen).Build();

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
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration()
                                        .HasRedPlayer(player =>
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
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration()
                                        .HasBlackPlayer(player =>
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
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration()
                            .PlayedInCompetition(option =>
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
    [InlineData(GameResult.RedWin, "1-0")]
    [InlineData(GameResult.BlackWin, "0-1")]
    [InlineData(GameResult.Draw, "1/2-1/2")]
    public static void ShouldCreateAGameWithCorrectResult_WhenCallingWithResult(GameResult result, string expectedResultText)
    {
        // Arrange
        XiangqiBuilder builder = new();

        // Act
        XiangqiGame xiangqiGame = builder
                                    .UseDefaultConfiguration()
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
                                    .UseDefaultConfiguration()
                                    .UseBoardConfig(config)
                                    .Build();

        // Assert
        xiangqiGame.Board.GetPieceAtPosition(redRookCoordinate).Should().NotBe(emptyPiece);
        xiangqiGame.Board.GetPieceAtPosition(redKingCoordinate).Should().NotBe(emptyPiece);
        xiangqiGame.Board.GetPieceAtPosition(blackKingCoordinate).Should().NotBe(emptyPiece);

        xiangqiGame.Board.GetPieceAtPosition(redRookCoordinate).Should().Be(redRook);
        xiangqiGame.Board.GetPieceAtPosition(redKingCoordinate).Should().Be(redKing);
        xiangqiGame.Board.GetPieceAtPosition(blackKingCoordinate).Should().Be(blackKing);
    }
}
