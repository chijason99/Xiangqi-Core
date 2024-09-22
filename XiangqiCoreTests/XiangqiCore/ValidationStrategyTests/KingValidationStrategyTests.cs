using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class ValidateMoveTests
{
	[Theory]
	[InlineData(5, 3)]
	[InlineData(5, 1)]
	[InlineData(4, 2)]
	[InlineData(6, 2)]
	public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForKing(int column, int row)
	{
		// Arrange
		XiangqiBuilder builder = new();

		Coordinate destination = new(column, row);
		Coordinate kingCoordinate = new(5, 2);

		XiangqiGame game = builder
							.WithDefaultConfiguration()
							.WithBoardConfig(config => config.AddPiece(PieceType.King, Side.Red, kingCoordinate))
							.Build();
		// Act
		King king = (King)game.Board.GetPieceAtPosition(kingCoordinate);

		bool isMoveValid = king.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, king.Coordinate, destination);

		// Assert
		isMoveValid.Should().BeTrue();
	}

	[Theory]
	[InlineData(4, 3)]
	[InlineData(6, 1)]
	[InlineData(4, 1)]
	[InlineData(6, 3)]
	public static void ValidateMoveLogicForPieceShouldReturnFalse_WhenGivenInvalidMoves_ForKing(int column, int row)
	{
		// Arrange
		XiangqiBuilder builder = new();

		Coordinate kingCoordinate = new(5, 2);
		Coordinate destination = new(column, row);

		XiangqiGame game = builder
							.WithDefaultConfiguration()
							.WithBoardConfig(config => config.AddPiece(PieceType.King, Side.Red, kingCoordinate))
							.Build();

		King king = (King)game.Board.GetPieceAtPosition(kingCoordinate);

		// Act
		bool isMoveValid = king.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, king.Coordinate, destination);

		// Assert
		isMoveValid.Should().BeFalse();
	}

	[Theory]
	[InlineData("2bakab2/1C7/n3c1n2/p1p3p1p/6c2/P3P1R2/3r2P1P/4B1N1C/4A4/1R2KAB2 b - - 0 16", 5, 10, 5, 9)]
	[InlineData("2bakab2/9/n3c4/p1p3p1p/4P4/P5R2/1n5rP/1R4N1C/4A4/4KAB2 w - - 0 21", 5, 1, 4, 1)]
	public static void ValidateMoveLogicForPieceShouldReturnTrue_WhenKingMovesInCorrectCoordinates(string fen, int startingPointCol, int startingPointRow, int destinationCol, int destinationRow)
	{
		// Arrange
		XiangqiBuilder builder = new();
		XiangqiGame game = builder.WithStartingFen(fen).Build();

		Coordinate startingPoint = new(startingPointCol, startingPointRow);
		Coordinate destination = new(destinationCol, destinationRow);

		King king = (King)game.Board.GetPieceAtPosition(startingPoint);
		IValidationStrategy validationStrategy = king.ValidationStrategy;
		// Act
		bool result = validationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, startingPoint, destination);
		// Assert
		result.Should().BeTrue();
	}
}

