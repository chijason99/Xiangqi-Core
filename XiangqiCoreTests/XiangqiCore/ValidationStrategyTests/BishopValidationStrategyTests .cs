using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class BishopValidationStrategyTests
{
    public static IEnumerable<object []> BishopObstacleTestData
    {
        get
        {
            yield return new object[] { new BishopObstacleTestData(new Coordinate(9, 3), new Coordinate(7, 5), new Coordinate(8, 4), ExpectedResult: false)};
            yield return new object[] { new BishopObstacleTestData(new Coordinate(9, 3), new Coordinate(7, 1), new Coordinate(8, 2), ExpectedResult: false)};
            yield return new object[] { new BishopObstacleTestData(new Coordinate(5, 3), new Coordinate(7, 5), new Coordinate(6, 4), ExpectedResult: false)};
            yield return new object[] { new BishopObstacleTestData(new Coordinate(5, 3), new Coordinate(7, 1), new Coordinate(6, 2), ExpectedResult: false)};
            yield return new object[] { new BishopObstacleTestData(new Coordinate(1, 3), new Coordinate(3, 5), new Coordinate(2, 4), ExpectedResult: false)};
            yield return new object[] { new BishopObstacleTestData(new Coordinate(1, 3), new Coordinate(3, 1), new Coordinate(2, 2), ExpectedResult: false)};
        }
    }


    [Theory]
    [InlineData(3, 1)]
    [InlineData(3, 5)]
    [InlineData(7, 1)]
    [InlineData(7, 5)]
    public static async Task ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForRedBishopAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate bishopCoordinate = new(5, 3);

        XiangqiGame game =  await builder
                            .UseEmptyBoard()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Red, bishopCoordinate))
                            .BuildAsync();
        // Act
        Bishop bishop = (Bishop)game.BoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, bishopCoordinate, destination);

        // Assert
        isMoveValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(9, 3)]
    [InlineData(6, 3)]
    [InlineData(5, 1)]
    public static async Task ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForRedBishopAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate bishopCoordinate = new(5, 3);

        XiangqiGame game = await builder
                            .UseEmptyBoard()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Red, bishopCoordinate))
                            .BuildAsync();
        // Act
        Bishop bishop = (Bishop)game.BoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, bishopCoordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(3, 10)]
    [InlineData(7, 10)]
    [InlineData(3, 6)]
    [InlineData(7, 6)]
    public static async Task ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForBlackBishopAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate bishopCoordinate = new(5, 8);

        XiangqiGame game = await builder
                            .UseEmptyBoard()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Black, bishopCoordinate))
                            .BuildAsync();
        // Act
        Bishop bishop = (Bishop)game.BoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, bishopCoordinate, destination);

        // Assert
        isMoveValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(BishopObstacleTestData))]
    public static async Task ValidateMoveLogicForPiece_ShouldReturnExpectedResult_WithObstaclesAsync(BishopObstacleTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();

        XiangqiGame game = await builder
                            .UseEmptyBoard()
                            .UseBoardConfig(config =>
                            {
                                config.AddPiece(PieceType.Bishop, Side.Red, testData.BishopPosiiton);
                                config.AddRandomPiece(testData.ObstaclePosition);
                            })
                            .BuildAsync();
        bool expectedResult = testData.ExpectedResult;

        Bishop bishop = (Bishop)game.BoardPosition.GetPieceAtPosition(testData.BishopPosiiton);

        // Act
        bool actualResult = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, testData.BishopPosiiton, testData.Destination);

        // Assert
        actualResult.Should().Be(expectedResult);
    }
}

public record BishopObstacleTestData(Coordinate BishopPosiiton, Coordinate Destination, Coordinate ObstaclePosition, bool ExpectedResult);