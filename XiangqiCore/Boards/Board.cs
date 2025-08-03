using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Boards;

public class Board
{
	private const string _emptyBoardFen = "9/9/9/9/9/9/9/9/9/9 w - - 0 0";

	internal Board()
	{
		Position = FenHelper.CreatePositionFromFen(_emptyBoardFen);
	}

	internal Board(string fenString)
	{
		Position = FenHelper.CreatePositionFromFen(fenString);
	}

	internal Board(BoardConfig config)
	{
		Position = FenHelper.CreatePositionFromFen(_emptyBoardFen);

		foreach (var keyValuePair in config.PiecesToAdd)
			SetPieceAtPosition(keyValuePair.Key, keyValuePair.Value);
	}

	public Piece[,] Position { get; private set; }

	private void SetPieceAtPosition(Coordinate targetCoordinates, Piece targetPiece) =>
		Position.SetPieceAtPosition(targetCoordinates, targetPiece);

	public Piece GetPieceAtPosition(Coordinate targetCoordinates) => Position.GetPieceAtPosition(targetCoordinates);

	public string GetFenFromPosition => FenHelper.GetFenFromPosition(Position);

	public static int[] GetAllRows() => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

	public static int[] GetAllColumns() => [1, 2, 3, 4, 5, 6, 7, 8, 9];

	public static int[] GetPalaceRows(Side color)
		=> color switch
		{
			Side.Red => [1, 2, 3],
			Side.Black => [8, 9, 10],
			_ => throw new ArgumentException("Please provide the correct Side that you are looking for")
		};

	public static int[] GetPalaceColumns() => [4, 5, 6];

	internal MoveHistoryObject MakeMove(
		Coordinate startingPosition, 
		Coordinate destination, 
		Side sideToMove,
		PieceOrder pieceOrder = PieceOrder.Unknown)
	{
		if (!Position.HasPieceAtPosition(startingPosition))
			throw new InvalidOperationException($"There must be a piece on the starting position {startingPosition}");

		Piece pieceToMove = GetPieceAtPosition(startingPosition);

		if (pieceToMove.Side != sideToMove)
			throw new InvalidOperationException(
				$"The side to move now should be {EnumHelper<Side>.GetDisplayName(sideToMove)}");

		if (!pieceToMove.ValidateMove(Position, startingPosition, destination))
			throw new InvalidOperationException(
				$"The proposed move from {startingPosition} to {destination} violates the game logic");

		MoveHistoryObject moveHistory = CreateMoveHistory(sideToMove, startingPosition, destination, pieceOrder);

		return moveHistory;
	}

	/// <summary>
	/// Moves the position back to the state before the move was made.
	/// </summary>
	/// <param name="moveHistory"></param>
	internal void UndoMove(MoveHistoryObject moveHistory)
	{
		Position = FenHelper.CreatePositionFromFen(moveHistory.FenBeforeMove);
	}

	/// <summary>
	///  Load the position from a MoveHistoryObject.
	/// </summary>
	/// <param name="moveHistory"></param>
	internal void LoadPositionFromMoveHistoryObject(MoveHistoryObject moveHistory)
	{
		Position = FenHelper.CreatePositionFromFen(moveHistory.FenAfterMove);
	}

private MoveHistoryObject CreateMoveHistory(Side sideToMove, Coordinate startingPosition, Coordinate destination, PieceOrder pieceOrder)
	{
		SimpleMoveObject simpleMoveObject = Position.MakeMoveInPlace(startingPosition, destination);
		
		bool isCapture = simpleMoveObject.PieceCaptured is not EmptyPiece;
		bool isCheck = Position.IsKingInCheck(sideToMove.GetOppositeSide());
		bool isCheckmate = Position.IsSideInCheckmate(sideToMove);

		Piece pieceMoved = simpleMoveObject.PieceMoved;
		Piece pieceCaptured = simpleMoveObject.PieceCaptured;

		MoveHistoryObject moveHistory = new(
			fenAfterMove: FenHelper.GetFenFromPosition(Position),
			fenBeforeMove: GetFenFromPosition,
			isCapture,
			isCheck,
			isCheckmate,
			pieceMoved.PieceType,
			pieceCaptured.PieceType,
			sideToMove,
			startingPosition,
			destination,
			pieceOrder,
			hasMultiplePieceOfSameTypeOnSameColumn: Position.HasMultiplePieceOfSameTypeOnSameColumn(
				pieceMoved.PieceType, 
				sideToMove, 
				startingPosition.Column)
		);

		return moveHistory;
	}
}
