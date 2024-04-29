using XiangqiCore.Extension;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class CannonValidationStrategyTests
{
    private const string startingFen1 = "3akab2/1r5r1/2n1b2c1/p3p3p/5n3/2R6/P3P3P/C1C1B1N2/7R1/3AKAB2 b - - 0 17";
    private const string startingFen2 = "1n1akab2/6c2/4bn3/C3N3p/9/9/P3P1pcP/4C4/4N4/2BAKAB2 w - - 1 18";
    
    public static IEnumerable<object[]> CannonValidationTestData
    {
        get
        {
            // Valid Moves
            yield return new object[] { new CannonValidationTestData(new Coordinate(8, 8), new Coordinate(6, 8), ExpectedResult: true) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(3, 3), new Coordinate(3, 8), ExpectedResult: true) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(1, 3), new Coordinate(1, 7), ExpectedResult: true) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(3, 3), new Coordinate(6, 3), ExpectedResult: true) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(8, 8), new Coordinate(8, 1), ExpectedResult: true) };

            // Invalid Moves
            yield return new object[] { new CannonValidationTestData(new Coordinate(8, 8), new Coordinate(6, 3), ExpectedResult: false) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(4, 3), new Coordinate(3, 8), ExpectedResult: false) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(1, 10), new Coordinate(5, 7), ExpectedResult: false) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(7, 3), new Coordinate(1, 2), ExpectedResult: false) };
            yield return new object[] { new CannonValidationTestData(new Coordinate(8, 8), new Coordinate(9, 1), ExpectedResult: false) };
        }
    }

    public static IEnumerable<object[]> CannonObstacleValidationTestData
    {
        get
        {
            // Valid Moves
            yield return new object[] { new CannonObstacleValidationTestData(startingFen1, new Coordinate(3, 3), new Coordinate(3, 8), ExpectedResult: true) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen1, new Coordinate(1, 3), new Coordinate(1, 7), ExpectedResult: true) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen2, new Coordinate(8, 4), new Coordinate(5, 4), ExpectedResult: true) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen2, new Coordinate(7, 9), new Coordinate(7, 1), ExpectedResult: true) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen2, new Coordinate(1, 7), new Coordinate(9, 7), ExpectedResult: true) };

            // Invalid Moves
            yield return new object[] { new CannonObstacleValidationTestData(startingFen1, new Coordinate(8, 8), new Coordinate(4, 8), ExpectedResult: false) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen1, new Coordinate(8, 8), new Coordinate(8, 1), ExpectedResult: false) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen1, new Coordinate(3, 3), new Coordinate(6, 3), ExpectedResult: false) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen2, new Coordinate(5, 3), new Coordinate(5, 8), ExpectedResult: false) };
            yield return new object[] { new CannonObstacleValidationTestData(startingFen2, new Coordinate(5, 3), new Coordinate(5, 10), ExpectedResult: false) };

        }
    }

    [Theory]
    [MemberData(nameof(CannonValidationTestData))]
    public static void ShouldReturnExpectedResult_WhenValidatingMoves_ForCannon(CannonValidationTestData cannonValidationTestData)
    {
        // Arrange
        Coordinate startingPosition = cannonValidationTestData.StartingPosition;
        Coordinate destination = cannonValidationTestData.Destination;
        bool expectedResult = cannonValidationTestData.ExpectedResult;

        XiangqiBuilder builder = new ();
        XiangqiGame game = builder
                            .UseEmptyBoard()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Cannon, Side.Red , startingPosition))
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;
        
        // Act
        Cannon cannon = (Cannon) boardPosition.GetPieceAtPosition(startingPosition);

        bool result = cannon.ValidationStrategy.ValidateMoveLogicForPiece(boardPosition, startingPosition, destination);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(CannonObstacleValidationTestData))]
    public static void ShouldReturnExpectedResult_WhenValidatingMoves_ForCannon_WithObstacles(CannonObstacleValidationTestData cannonObstacleValidationTestData)
    {
        // Arrange
        Coordinate startingPosition = cannonObstacleValidationTestData.StartingPosition;
        Coordinate destination = cannonObstacleValidationTestData.Destination;
        bool expectedResult = cannonObstacleValidationTestData.ExpectedResult;
        string startingFen = cannonObstacleValidationTestData.StartingFen;

        XiangqiBuilder builder = new();
        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;

        // Act
        Cannon cannon = (Cannon)boardPosition.GetPieceAtPosition(startingPosition);

        bool result = cannon.ValidationStrategy.ValidateMoveLogicForPiece(boardPosition, startingPosition, destination);

        // Assert
        result.Should().Be(expectedResult);
    }
}

public record CannonValidationTestData(Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);

public record  CannonObstacleValidationTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);
