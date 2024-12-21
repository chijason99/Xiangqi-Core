using System.Reflection;
using XiangqiCore.Attributes;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObject;
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

	/// <summary>
	/// Use the BoardConfig to override existing pieces on board
	/// </summary>
	/// <param name="config"></param>
	[Obsolete("This constructor is no longer in use")]
	private Board(string fenString, BoardConfig config) : this(fenString)
	{
		foreach (var keyValuePair in config.PiecesToAdd)
			SetPieceAtPosition(keyValuePair.Key, keyValuePair.Value);
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

	internal MoveHistoryObject MakeMove(Coordinate startingPosition, Coordinate destination, Side sideToMove)
	{
		if (!_position.HasPieceAtPosition(startingPosition))
			throw new InvalidOperationException($"There must be a piece on the starting position {startingPosition}");

		Piece pieceToMove = GetPieceAtPosition(startingPosition);

		if (pieceToMove.Side != sideToMove)
			throw new InvalidOperationException($"The side to move now should be {EnumHelper<Side>.GetDisplayName(sideToMove)}");

		if (!pieceToMove.ValidateMove(_position, startingPosition, destination))
			throw new InvalidOperationException($"The proposed move from {startingPosition} to {destination} violates the game logic"); ;

		MoveHistoryObject moveHistory = CreateMoveHistory(sideToMove, startingPosition, destination);
		_position.MakeMove(startingPosition, destination);

		return moveHistory;
	}

	internal MoveHistoryObject MakeMove(ParsedMoveObject moveObject, Side sideToMove)
	{
		Coordinate startingPosition = FindStartingPosition(moveObject, sideToMove);
		Coordinate destination = FindDestination(moveObject, startingPosition);

		return MakeMove(startingPosition, destination, sideToMove);
	}

	public static string GetImageResourcePath()
		=> "XiangqiCore.Assets.Board.board.png";

	private Coordinate FindStartingPosition(ParsedMoveObject moveObject, Side sideToMove)
	{
		if (moveObject.IsFromUcciNotation)
			return moveObject.StartingPosition.Value;

		Piece[] piecesToMove = GetPiecesToMove(moveObject.PieceType, sideToMove);
		Piece pieceToMove;

		// If the starting column is provided, then find the piece that has the same column as the starting column;
		// Otherwise, i.e. there are more than one piece of the same type and side in the column, pick the one following the order
		if (moveObject is MultiColumnPawnParsedMoveObject multiColumnPawnObject)
			pieceToMove = FindPieceToMoveForMultiColumnPawn(multiColumnPawnObject, piecesToMove, sideToMove);
		else
		{
			int actualStartingColumn = moveObject.StartingColumn.ConvertToColumnBasedOnSide(sideToMove);

			if (piecesToMove.Count(p => p.Coordinate.Column == actualStartingColumn) == 1)
				pieceToMove = piecesToMove.Single(p => p.Coordinate.Column == actualStartingColumn);
			// Edge case: in some notation, there are two pieces of the same type on the same column and the notation
			// do not mark which piece is moving, but only one of them would be able to perform the move specified validly
			else if (moveObject.PieceOrderIndex == ParsedMoveObject.UnknownPieceOrderIndex)
			{
				pieceToMove = piecesToMove.First(x =>
				{
					try
					{
						Coordinate guessedDestination = x.GetDestinationCoordinateFromNotation(moveObject.MoveDirection, moveObject.FourthCharacter);

						return x.ValidateMove(Position, x.Coordinate, guessedDestination);
					}
					catch (ArgumentOutOfRangeException ex)
					{
						return false;
					}
				});
			}
			else
				pieceToMove = piecesToMove[moveObject.PieceOrderIndex];
		}

		return pieceToMove.Coordinate;
	}

	private Piece[] GetPiecesToMove(Type pieceType, Side sideToMove)
	{
		MethodInfo method = typeof(PieceExtension).GetMethod(nameof(PieceExtension.GetPiecesOfType));
		MethodInfo genericMethod = method.MakeGenericMethod(pieceType);

		IEnumerable<Piece> allPiecesOfType = ((IEnumerable<Piece>)genericMethod.Invoke(obj: null, parameters: [_position, sideToMove]));

		if (!allPiecesOfType.Any()) throw new InvalidOperationException($"Cannot find any columns containing more than one {EnumHelper<Side>.GetDisplayName(sideToMove)} {pieceType.Name}");

		Piece[] piecesToMove = allPiecesOfType
									.OrderByRowWithSide(sideToMove)
									.ToArray();

		return piecesToMove;
	}

	private Piece FindPieceToMoveForMultiColumnPawn(MultiColumnPawnParsedMoveObject moveObject, Piece[] piecesToMove, Side sideToMove)
	{
		Piece[] pawnsOnColumn;

		if (moveObject.StartingColumn != ParsedMoveObject.UnknownStartingColumn)
		{
			pawnsOnColumn = piecesToMove
								.Where(p => p.Coordinate.Column == moveObject.StartingColumn.ConvertToColumnBasedOnSide(sideToMove))
								.OrderByRowWithSide(sideToMove)
								.ToArray();
		}
		else
		{
			pawnsOnColumn = piecesToMove
								.GroupBy(p => p.Coordinate.Column)
								.Where(group => group.Count() >= moveObject.MinNumberOfPawnsOnColumn)
								.SelectMany(group => group)
								.OrderByRowWithSide(sideToMove)
								.ToArray();
		}

		if (moveObject.PieceOrderIndex == MultiColumnPawnParsedMoveObject.LastPawnIndex)
			return pawnsOnColumn.Last();
		else
			return pawnsOnColumn[moveObject.PieceOrderIndex];
	}

	private Coordinate FindDestination(ParsedMoveObject moveObject, Coordinate startingCoordinate)
	{
		if (moveObject.IsFromUcciNotation)
			return moveObject.Destination.Value;

		Piece pieceToMove = _position.GetPieceAtPosition(startingCoordinate);

		MoveDirection moveDirection = moveObject.MoveDirection;

		if (pieceToMove.GetType().GetCustomAttribute<MoveInDiagonalsAttribute>() is not null && moveDirection == MoveDirection.Horizontal)
			throw new ArgumentException($"Piece type {moveObject.PieceType.Name} cannot move horizontally");

		Coordinate destination = pieceToMove.GetDestinationCoordinateFromNotation(moveObject.MoveDirection, moveObject.FourthCharacter);

		return destination;
	}

	private MoveHistoryObject CreateMoveHistory(Side sideToMove, Coordinate startingPosition, Coordinate destination)
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
			destination
		);

		return moveHistory;
	}
}
