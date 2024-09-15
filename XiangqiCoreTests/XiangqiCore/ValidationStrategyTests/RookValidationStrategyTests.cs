using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class RookValidationStrategyTests
{
    public static IEnumerable<object[]> RookValidationTestData
    {
        get
        {
            // Valid moves for rook
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(9, 5), ExpectedResult: true) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(1, 5), ExpectedResult: true) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(2, 5), ExpectedResult: true) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(5, 10), ExpectedResult: true) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(5, 7), ExpectedResult: true) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(5, 2), ExpectedResult: true) };

            // Invalid moves for rook
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(3, 6), ExpectedResult: false) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(2, 3), ExpectedResult: false) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(3, 10), ExpectedResult: false) };
            yield return new object[] { new RookValidationTestData(new Coordinate(5, 5), new Coordinate(2, 10), ExpectedResult: false) };
        }
    }

    public static IEnumerable<object[]> RookObstacleValidationTestData
    {
        get
        {
            const string startingFen = "r3kab2/3na4/2c1b1c1n/p5p2/9/2p2rP1C/P3P3P/N3BCN2/4A4/1R2KABR1 b - - 8 13";

            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(6, 5), new Coordinate(7, 5), ExpectedResult: true) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(6, 5), new Coordinate(5, 5), ExpectedResult: true) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(6, 5), new Coordinate(6, 3), ExpectedResult: true) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(2, 1), new Coordinate(2, 9), ExpectedResult: true) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(2, 1), new Coordinate(4, 1), ExpectedResult: true) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(1, 10), new Coordinate(1, 8), ExpectedResult: true) };

            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(6, 5), new Coordinate(9, 5), ExpectedResult: false) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(6, 5), new Coordinate(1, 5), ExpectedResult: false) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(6, 5), new Coordinate(6, 2), ExpectedResult: false) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(2, 1), new Coordinate(9, 1), ExpectedResult: false) };
            yield return new object[] { new RookObstacleValidationTestData(startingFen, new Coordinate(1, 10), new Coordinate(1, 5), ExpectedResult: false) };
        }
    }

    [Theory]
    [MemberData(nameof(RookValidationTestData))]
    public static async Task ValidateMoveLogicForPieceShouldReturnExpectedResult_WhenGivenMoves_ForRookAsync(RookValidationTestData rookValidationTestData) 
    { 
        // Arrange
        XiangqiBuilder builder = new ();

        Coordinate startingPosition = rookValidationTestData.StartingPosition;
        Coordinate destination = rookValidationTestData.Destination;

        XiangqiGame game = await builder
                            .WithEmptyBoard()
                            .WithBoardConfig(config => config.AddPiece(PieceType.Rook, Side.Red, startingPosition))
                            .BuildAsync();
        // Act
        Rook rook = (Rook)game.BoardPosition.GetPieceAtPosition(startingPosition);

        bool isMoveValid = rook.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, startingPosition, destination);
        bool expectedResult = rookValidationTestData.ExpectedResult;
        
        // Assert
        isMoveValid.Should().Be(expectedResult);
    }


    [Theory]
    [MemberData(nameof(RookObstacleValidationTestData))]
    public static async Task ValidateMoveLogicForPieceShouldReturnExpectedResult_WhenGivenMoves_ForRook_WithObstaclesAsync(RookObstacleValidationTestData rookValidationTestData)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate startingPosition = rookValidationTestData.StartingPosition;
        Coordinate destination = rookValidationTestData.Destination;
        string startingFen = rookValidationTestData.StartingFen;

        XiangqiGame game = await builder
                            .WithCustomFen(startingFen)
                            .WithBoardConfig(config => config.AddPiece(PieceType.Rook, Side.Red, startingPosition))
                            .BuildAsync();
        // Act
        Rook rook = (Rook)game.BoardPosition.GetPieceAtPosition(startingPosition);

        bool isMoveValid = rook.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, startingPosition, destination);
        bool expectedResult = rookValidationTestData.ExpectedResult;

        // Assert
        isMoveValid.Should().Be(expectedResult);
    }
}

public record RookValidationTestData(Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);

public record RookObstacleValidationTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);
