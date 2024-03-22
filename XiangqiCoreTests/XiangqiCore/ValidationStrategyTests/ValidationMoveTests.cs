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
        var result = PieceFactory.Create(PieceType.King, Side.Red, new Coordinate(column: 5, row: 2));

        King king = (King)result.Value;

        Coordinate destination = new(column, row);

        // Act
        bool isMoveValid = king.ValidationStrategy.ValidateMove(king.Coordinate, destination);

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
        var result = PieceFactory.Create(PieceType.King, Side.Red, new Coordinate(column: 5, row: 2));

        King king = (King)result.Value;

        Coordinate destination = new(column, row);

        // Act
        bool isMoveValid = king.ValidationStrategy.ValidateMove(king.Coordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }
}
