using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class AdvisorValidationStrategyTests
{
    [Theory]
    [InlineData(4, 3)]
    [InlineData(4, 1)]
    [InlineData(6, 3)]
    [InlineData(6, 1)]
    public static async Task ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForRedAdvisorAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 2);

        XiangqiGame game =  await builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Red, advisorCoordinate))
                            .BuildAsync();
        // Act
        Advisor advisor = (Advisor)game.BoardPosition.GetPieceAtPosition(advisorCoordinate);

        bool isMoveValid = advisor.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, advisorCoordinate, destination);

        // Assert
        isMoveValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(4, 5)]
    [InlineData(5, 2)]
    [InlineData(3, 4)]
    [InlineData(5, 1)]
    public static async Task ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForRedAdvisorAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 2);

        XiangqiGame game = await builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Red, advisorCoordinate))
                            .BuildAsync();
        // Act
        Advisor advisor = (Advisor)game.BoardPosition.GetPieceAtPosition(advisorCoordinate);

        bool isMoveValid = advisor.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, advisorCoordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(4, 10)]
    [InlineData(6, 10)]
    [InlineData(4, 8)]
    [InlineData(6, 8)]
    public static async Task ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForBlackAdvisorAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 9);

        XiangqiGame game = await builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Black, advisorCoordinate))
                            .BuildAsync();
        // Act
        Advisor advisor = (Advisor)game.BoardPosition.GetPieceAtPosition(advisorCoordinate);

        bool isMoveValid = advisor.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, advisorCoordinate, destination);

        // Assert
        isMoveValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(4, 1)]
    [InlineData(5, 7)]
    [InlineData(5, 9)]
    [InlineData(5, 10)]
    public static async Task ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForBlackAdvisorAsync(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 9);

        XiangqiGame game = await builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Red, advisorCoordinate))
                            .BuildAsync();
        // Act
        Advisor advisor = (Advisor)game.BoardPosition.GetPieceAtPosition(advisorCoordinate);

        bool isMoveValid = advisor.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, advisorCoordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }
}
