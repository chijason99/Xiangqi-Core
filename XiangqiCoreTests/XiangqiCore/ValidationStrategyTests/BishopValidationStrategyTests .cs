using XiangqiCore.Extension;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class BishopValidationStrategyTests
{
    [Theory]
    [InlineData(3, 1)]
    [InlineData(3, 5)]
    [InlineData(7, 1)]
    [InlineData(7, 5)]
    public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForRedBishop(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate bishopCoordinate = new(5, 3);

        XiangqiGame game =  builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Red, bishopCoordinate))
                            .Build();
        // Act
        Bishop bishop = (Bishop)game.GetBoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.GetBoardPosition, bishopCoordinate, destination);

        // Assert
        isMoveValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(9, 3)]
    [InlineData(6, 3)]
    [InlineData(5, 1)]
    public static void ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForRedBishop(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate bishopCoordinate = new(5, 3);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Red, bishopCoordinate))
                            .Build();
        // Act
        Bishop bishop = (Bishop)game.GetBoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.GetBoardPosition, bishopCoordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(3, 10)]
    [InlineData(7, 10)]
    [InlineData(3, 6)]
    [InlineData(7, 6)]
    public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForBlackBishop(int column, int row)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate destination = new(column, row);
        Coordinate bishopCoordinate = new(5, 8);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Black, bishopCoordinate))
                            .Build();
        // Act
        Bishop bishop = (Bishop)game.GetBoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.GetBoardPosition, bishopCoordinate, destination);

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
        Coordinate bishopCoordinate = new(5, 8);

        XiangqiGame game = builder
                            .UseDefaultConfiguration()
                            .UseBoardConfig(config => config.AddPiece(PieceType.Bishop, Side.Red, bishopCoordinate))
                            .Build();
        // Act
        Bishop bishop = (Bishop)game.GetBoardPosition.GetPieceAtPosition(bishopCoordinate);

        bool isMoveValid = bishop.ValidationStrategy.ValidateMoveLogicForPiece(game.GetBoardPosition, bishopCoordinate, destination);

        // Assert
        isMoveValid.Should().BeFalse();
    }
}
