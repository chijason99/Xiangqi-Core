using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class ValidateMoveTests
{
    [Theory]
    [InlineData(5, 3)]
    [InlineData(5, 1)]
    [InlineData(4, 2)]
    [InlineData(6, 2)]
    public static void ShouldReturnTrue_WhenGivenValidMoves_ForKing(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate kingCoordinate = new(5, 2);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.King, Side.Red, kingCoordinate))
                            .Build();
        // Act
        King king = (King)game.Board.GetPieceAtPosition(kingCoordinate);

        bool isMoveValid = king.ValidationStrategy.ValidateMoveLogicForPiece(game.Board.Position, king.Coordinate, destination);

        // Assert
        isMoveValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(4, 3)]
    [InlineData(6, 1)]
    [InlineData(4, 1)]
    [InlineData(6, 3)]
    public static void ShouldReturnFalse_WhenGivenInValidMoves_ForKing(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate kingCoordinate = new(5, 2);
        Coordinate destination = new(column, row);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.King, Side.Red, kingCoordinate))
                            .Build();

        King king = (King)game.Board.GetPieceAtPosition(kingCoordinate);

        // Act
        bool isMoveValid = king.ValidationStrategy.ValidateMoveLogicForPiece(game.GetBoardPosition, king.Coordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }
}
