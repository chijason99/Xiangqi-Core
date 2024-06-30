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
    public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForRedAdvisor(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 2);

        XiangqiGame game =  builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Red, advisorCoordinate))
                            .Build();
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
    public static void ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForRedAdvisor(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 2);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Red, advisorCoordinate))
                            .Build();
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
    public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForBlackAdvisor(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 9);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Black, advisorCoordinate))
                            .Build();
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
    public static void ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForBlackAdvisor(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate advisorCoordinate = new(5, 9);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Advisor, Side.Red, advisorCoordinate))
                            .Build();
        // Act
        Advisor advisor = (Advisor)game.BoardPosition.GetPieceAtPosition(advisorCoordinate);

        bool isMoveValid = advisor.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, advisorCoordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }
}
