using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class AreCoordinatesValidTests
{
    [Theory]
    [InlineData(4,1)]
    [InlineData(4,2)]
    [InlineData(4,3)]
    [InlineData(5,1)]
    [InlineData(5,2)]
    [InlineData(5,3)]
    [InlineData(6,1)]
    [InlineData(6,2)]
    [InlineData(6,3)]
    public static void ShouldReturnTrue_WhenGivenValidCoordinates_ForRedKing(int column, int row) 
    {
        // Arrange
        var result = PieceFactory.Create(PieceType.King, Side.Red, new Coordinate(5, 2));

        King redKing = (King)result.Value;

        Coordinate destination = new(column, row);
        // Act
        bool isDestinationValid = redKing.ValidationStrategy.AreCoordinatesValid(Side.Red, destination);

        // Assert
        isDestinationValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(4, 8)]
    [InlineData(4, 9)]
    [InlineData(4, 10)]
    [InlineData(5, 8)]
    [InlineData(5, 9)]
    [InlineData(5, 10)]
    [InlineData(6, 8)]
    [InlineData(6, 9)]
    [InlineData(6, 10)]
    public static void ShouldReturnTrue_WhenGivenValidCoordinates_ForBlackKing(int column, int row)
    {
        // Arrange
        var result = PieceFactory.Create(PieceType.King, Side.Black, new Coordinate(5, 2));

        King blackKing = (King)result.Value;

        Coordinate destination = new(column, row);
        // Act
        bool isDestinationValid = blackKing.ValidationStrategy.AreCoordinatesValid(Side.Black, destination);

        // Assert
        isDestinationValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(8, 7)]
    [InlineData(7, 3)]
    [InlineData(4, 8)]
    [InlineData(3, 5)]
    [InlineData(2, 2)]
    [InlineData(9, 10)]
    [InlineData(3, 2)]
    [InlineData(1, 1)]
    [InlineData(2, 5)]
    public static void ShouldReturnFalse_WhenGivenInvalidCoordinates_ForRedKing(int column, int row)
    {
        // Arrange
        var result = PieceFactory.Create(PieceType.King, Side.Red, new Coordinate(5, 2));

        King redKing = (King)result.Value;

        Coordinate destination = new(column, row);
        // Act
        bool isDestinationValid = redKing.ValidationStrategy.AreCoordinatesValid(Side.Red, destination);

        // Assert
        isDestinationValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(8, 7)]
    [InlineData(7, 3)]
    [InlineData(5, 3)]
    [InlineData(3, 5)]
    [InlineData(2, 2)]
    [InlineData(9, 10)]
    [InlineData(3, 2)]
    [InlineData(1, 1)]
    [InlineData(2, 5)]
    public static void ShouldReturnFalse_WhenGivenInvalidCoordinates_ForBlackKing(int column, int row)
    {
        // Arrange
        var result = PieceFactory.Create(PieceType.King, Side.Black, new Coordinate(5, 2));

        King blackKing = (King)result.Value;

        Coordinate destination = new(column, row);
        // Act
        bool isDestinationValid = blackKing.ValidationStrategy.AreCoordinatesValid(Side.Black, destination);

        // Assert
        isDestinationValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(3, 1)]
    [InlineData(3, 5)]
    [InlineData(5, 3)]
    [InlineData(7, 1)]
    [InlineData(7, 5)]
    [InlineData(9, 3)]
    public static void ShouldReturnTrue_WhenGivenValidCoordinates_ForRedBishop(int column, int row)
    {
        // Arrange
        var result = PieceFactory.Create(PieceType.Bishop, Side.Red, new Coordinate(3, 1));

        Bishop redBishop = (Bishop)result.Value;

        Coordinate destination = new(column, row);
        // Act
        bool isDestinationValid = redBishop.ValidationStrategy.AreCoordinatesValid(Side.Red, destination);

        // Assert
        isDestinationValid.Should().BeTrue();
    }
}
