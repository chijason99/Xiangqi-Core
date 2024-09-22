using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class KnightValidationStrategyTests
{
    public static IEnumerable<object[]> KnightValidMoveTestData
    {
        get
        {
            // Valid move for knight
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(5, 7), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(7, 7), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(4, 6), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(4, 4), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(5, 3), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(7, 3), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(8, 6), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(8, 4), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(1, 10), new Coordinate(2, 8), ExpectedResult: true) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(9, 5), new Coordinate(7, 6), ExpectedResult: true) };

            // Invalid move for knight
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(4, 7), ExpectedResult: false) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(2, 7), ExpectedResult: false) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(1, 6), ExpectedResult: false) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(9, 4), ExpectedResult: false) };
            yield return new object[] { new KnightValidationTestData(new Coordinate(6, 5), new Coordinate(5, 1), ExpectedResult: false) };
        }
    }

    public static IEnumerable<object[]> KnightObstacleMoveTestData
    {
        get
        {
            // Moving Up with Obstacle on top
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(2, 7), new Coordinate(3, 6), ExpectedResult: false ) };
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(4, 7), new Coordinate(3, 6), ExpectedResult: false ) };
            // Moving Down with Obstacle below
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(2, 3), new Coordinate(3, 4), ExpectedResult: false ) };
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(4, 3), new Coordinate(3, 4), ExpectedResult: false) };
            // Moving Left with Obstacle on the left
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(1, 6), new Coordinate(2, 5), ExpectedResult: false) };
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(1, 4), new Coordinate(2, 5), ExpectedResult: false) };
            // Moving Right with Obstacle on the right
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(5, 6), new Coordinate(4, 5), ExpectedResult: false) };
            yield return new object[] { new KnightObstacleValidationTestData(new Coordinate(3, 5), new Coordinate(5, 4), new Coordinate(4, 5), ExpectedResult: false) };
        }
    }

	[Theory]
	[MemberData(nameof(KnightValidMoveTestData))]
	public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForKnight(KnightValidationTestData knightValidationTestData)
	{
		// Arrange
		XiangqiBuilder builder = new();

		Coordinate knightCoordinate = knightValidationTestData.StartingPosition;
		Coordinate destination = knightValidationTestData.Destination;

		XiangqiGame game = builder
							.WithEmptyBoard()
							.WithBoardConfig(config => config.AddPiece(PieceType.Knight, Side.Red, knightCoordinate))
							.Build();
		// Act
		Knight knight = (Knight)game.BoardPosition.GetPieceAtPosition(knightCoordinate);

		bool isMoveValid = knight.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, knightCoordinate, destination);
		bool expectedResult = knightValidationTestData.ExpectedResult;

		// Assert
		isMoveValid.Should().Be(expectedResult);
	}

	[Theory]
	[MemberData(nameof(KnightObstacleMoveTestData))]
	public static void ValidateMoveLogicForPieceShouldReturnCorrectResult_WhenGivenInvalidMoves_ForKnight_WithObstacles(KnightObstacleValidationTestData knightObstacleValidationTestData)
	{
		// Arrange
		XiangqiBuilder builder = new();

		Coordinate knightCoordinate = knightObstacleValidationTestData.StartingPosition;
		Coordinate destination = knightObstacleValidationTestData.Destination;
		Coordinate obstacleCoordinate = knightObstacleValidationTestData.Obstacle;

		XiangqiGame game = builder
							.WithEmptyBoard()
							.WithBoardConfig(config =>
								{
									config.AddPiece(PieceType.Knight, Side.Red, knightCoordinate);
									config.AddRandomPiece(obstacleCoordinate);
								})
							.Build();
		// Act
		Knight knight = (Knight)game.BoardPosition.GetPieceAtPosition(knightCoordinate);

		bool isMoveValid = knight.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, knightCoordinate, destination);
		bool expectedResult = knightObstacleValidationTestData.ExpectedResult;

		// Assert
		isMoveValid.Should().Be(expectedResult);
	}
}

public record KnightValidationTestData(Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);

public record KnightObstacleValidationTestData(Coordinate StartingPosition, Coordinate Destination, Coordinate Obstacle,  bool ExpectedResult);
