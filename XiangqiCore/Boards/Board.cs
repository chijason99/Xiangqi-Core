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
		_position = FenHelper.CreatePositionFromFen(_emptyBoardFen);
	}

	internal Board(string fenString)
	{
		_position = FenHelper.CreatePositionFromFen(fenString);
	}

	internal Board(BoardConfig config)
	{
		_position = FenHelper.CreatePositionFromFen(_emptyBoardFen);

		foreach (var keyValuePair in config.PiecesToAdd)
			SetPieceAtPosition(keyValuePair.Key, keyValuePair.Value);
	}

	private Piece[,] _position { get; set; }

	public Piece[,] Position => _position.DeepClone();

	private void SetPieceAtPosition(Coordinate targetCoordinates, Piece targetPiece) => _position.SetPieceAtPosition(targetCoordinates, targetPiece);

	public Piece GetPieceAtPosition(Coordinate targetCoordinates) => _position.GetPieceAtPosition(targetCoordinates);

	public string GetFenFromPosition => FenHelper.GetFenFromPosition(_position);

	public static int[] GetAllRows() => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

	public static int[] GetAllColumns() => [1, 2, 3, 4, 5, 6, 7, 8, 9];

	public static int[] GetPalaceRows(Side color)
		=> color == Side.Red ? [1, 2, 3] : color == Side.Black ? [8, 9, 10] : throw new ArgumentException("Please provide the correct Side that you are looking for");

	public static int[] GetPalaceColumns() => [4, 5, 6];

	internal MoveHistoryObject MakeMove(Coordinate startingPosition, Coordinate destination, Side sideToMove, PieceOrder pieceOrder = PieceOrder.Unknown)
	{
		if (!_position.HasPieceAtPosition(startingPosition))
			throw new InvalidOperationException($"There must be a piece on the starting position {startingPosition}");

		Piece pieceToMove = GetPieceAtPosition(startingPosition);

		if (pieceToMove.Side != sideToMove)
			throw new InvalidOperationException($"The side to move now should be {EnumHelper<Side>.GetDisplayName(sideToMove)}");

		if (!pieceToMove.ValidateMove(_position, startingPosition, destination))
			throw new InvalidOperationException($"The proposed move from {startingPosition} to {destination} violates the game logic"); ;

		MoveHistoryObject moveHistory = CreateMoveHistory(sideToMove, startingPosition, destination, pieceOrder);
		_position.MakeMove(startingPosition, destination);

		return moveHistory;
	}

	internal void UndoMove(MoveHistoryObject moveHistory)
	{
		Piece pieceMoved = PieceFactory.Create(moveHistory.PieceMoved, moveHistory.MovingSide, moveHistory.StartingPosition);
		SetPieceAtPosition(moveHistory.StartingPosition, pieceMoved);

		Piece pieceCaptured = new EmptyPiece();

		if (moveHistory.IsCapture)
			pieceCaptured = PieceFactory.Create(moveHistory.PieceCaptured, moveHistory.MovingSide.GetOppositeSide(), moveHistory.Destination);

		SetPieceAtPosition(moveHistory.Destination, pieceCaptured);
	}

	private MoveHistoryObject CreateMoveHistory(Side sideToMove, Coordinate startingPosition, Coordinate destination, PieceOrder pieceOrder)
	{
		Piece pieceCaptured = GetPieceAtPosition(destination);
		Piece[,] positionAfterTheProposedMove = _position.SimulateMove(startingPosition, destination);
		bool isCapture = _position.HasPieceAtPosition(destination);
		bool isCheck = positionAfterTheProposedMove.IsKingInCheck(sideToMove.GetOppositeSide());
		bool isCheckmate = positionAfterTheProposedMove.IsSideInCheckmate(sideToMove);
		Piece pieceMoved = GetPieceAtPosition(startingPosition);

		MoveHistoryObject moveHistory = new(
			fenAfterMove: FenHelper.GetFenFromPosition(positionAfterTheProposedMove),
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
			hasMultiplePieceOfSameTypeOnSameColumn: _position.HasMultiplePieceOfSameTypeOnSameColumn(
				pieceMoved.PieceType, 
				sideToMove, 
				startingPosition.Column)
		);

		return moveHistory;
	}
}
