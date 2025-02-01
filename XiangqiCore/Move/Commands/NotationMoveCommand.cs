using XiangqiCore.Boards;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces;
using XiangqiCore.Services.MoveParsing;

namespace XiangqiCore.Move.Commands;

public class NotationMoveCommand : IMoveCommand
{
	private readonly IMoveParsingService _moveParsingService;
	private readonly string _moveNotation;
	private readonly Side _sideToMove;
	private readonly MoveNotationType _moveNotationType;
	private readonly Piece[,] _position;

	public NotationMoveCommand(
		IMoveParsingService moveParsingService,
		string moveNotation,
		Side sideToMove,
		MoveNotationType moveNotationType)
	{
		_moveParsingService = moveParsingService;
		_moveNotation = moveNotation;
		_sideToMove = sideToMove;
		_moveNotationType = moveNotationType;
	}

	public NotationMoveCommand(
		string moveNotation,
		Side sideToMove,
		MoveNotationType moveNotationType) : 
		this(
			new DefaultMoveParsingService(), 
			moveNotation, 
			sideToMove, 
			moveNotationType)
	{
	}

	public MoveHistoryObject MoveHistoryObject { get; private set; }

	public MoveHistoryObject Execute(Board board)
	{
		ParsedMoveObject parsedMoveObject = _moveParsingService.ParseMove(_moveNotation, _moveNotationType);
		Coordinate startingPosition = FindStartingPosition(parsedMoveObject, _sideToMove);
		Coordinate destination = FindDestination(parsedMoveObject, startingPosition);

		MoveHistoryObject moveHistoryObject = board.MakeMove(
			startingPosition, 
			destination, 
			_sideToMove, 
			parsedMoveObject.PieceOrder);

		moveHistoryObject.UpdateMoveNotation(_moveNotation, _moveNotationType);

		MoveHistoryObject = moveHistoryObject;
		return moveHistoryObject;
	}

	public MoveHistoryObject Undo(Board board)
	{
		board.UndoMove(MoveHistoryObject);

		return MoveHistoryObject;
	}

	private Coordinate FindStartingPosition(ParsedMoveObject moveObject, Side sideToMove)
	{
		if (moveObject.IsFromUcciNotation)
			return moveObject.StartingPosition!.Value;

		Piece[] potentialPiecesToMove = GetPotentialPiecesToMove(moveObject.PieceType, sideToMove);
		Piece? movedPiece = null;

		if (moveObject is MultiColumnPawnParsedMoveObject multiColumnPawnObject)
			movedPiece = FindMovedPieceForMultiColumnPawn(multiColumnPawnObject, potentialPiecesToMove, sideToMove);
		else
			movedPiece = FindMovedPiece(potentialPiecesToMove, moveObject, sideToMove);

		if (movedPiece is null)
			throw new InvalidOperationException("No valid piece found to move.");

		return movedPiece.Coordinate;
	}

	private Piece[] GetPotentialPiecesToMove(PieceType pieceType, Side sideToMove)
	{
		var allPiecesOfType = _position.GetPiecesOfType(pieceType, sideToMove);

		if (!allPiecesOfType.Any()) throw new InvalidOperationException($"Cannot find any columns containing more than one {EnumHelper<Side>.GetDisplayName(sideToMove)} {EnumHelper<PieceType>.GetDisplayName(pieceType)}");

		Piece[] potentialPiecesToMove = allPiecesOfType
			.OrderByRowWithSide(sideToMove)
			.ToArray();

		return potentialPiecesToMove;
	}

	private Piece FindMovedPiece(Piece[] piecesToMove, ParsedMoveObject moveObject, Side sideToMove)
	{
		// If the starting column is provided, then find the piece that has the same column as the starting column;
		// Otherwise, i.e. there are more than one piece of the same type and side in the column, pick the one following the order

		if (piecesToMove.Length == 0)
			throw new ArgumentException("No pieces to move");

		Piece? movedPiece = null;

		// If the starting column is unknown, it means that the two pieces are on the same column
		if (moveObject.StartingColumn == ParsedMoveObject.UnknownStartingColumn)
		{
			movedPiece = moveObject.PieceOrder == PieceOrder.First ?
				piecesToMove.FirstOrDefault() :
				piecesToMove.LastOrDefault();
		}
		else
		{
			int startingColumn = moveObject.StartingColumn.ConvertToColumnBasedOnSide(sideToMove);

			if (piecesToMove.Count(p => p.Coordinate.Column == startingColumn) == 1)
			{
				movedPiece = piecesToMove.Single(p => p.Coordinate.Column == startingColumn);
				moveObject.PieceOrder = PieceOrder.First;
			}

			// Edge case: in some notation, there are two pieces of the same type on the same column and the notation
			// do not mark which piece is moving, but only one of them would be able to perform the move specified validly
			if (moveObject.PieceOrder == PieceOrder.Unknown)
			{
				Piece piece = piecesToMove.First();

				try
				{
					Coordinate guessedDestination = piece.GetDestinationCoordinateFromNotation(moveObject.MoveDirection, moveObject.FourthCharacter);

					if (piece.ValidateMove(_position, piece.Coordinate, guessedDestination))
					{
						movedPiece = piece;
						moveObject.PieceOrder = PieceOrder.First;
					}
					else
					{
						movedPiece = piecesToMove.Last();
						moveObject.PieceOrder = PieceOrder.Last;
					}
				}
				catch (ArgumentOutOfRangeException)
				{
					Console.WriteLine("Trying to find out the correct piece to move due to an ambiguous move notation");

					movedPiece = piecesToMove.Last();
					moveObject.PieceOrder = PieceOrder.Last;
				}
			}
		}

		if (movedPiece is null)
			throw new InvalidOperationException("No valid piece found to move.");

		return movedPiece;
	}

	private Piece FindMovedPieceForMultiColumnPawn(MultiColumnPawnParsedMoveObject moveObject, Piece[] piecesToMove, Side sideToMove)
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

		if (moveObject.PieceOrder == PieceOrder.Last)
			return pawnsOnColumn.Last();
		else
			return pawnsOnColumn[(int)moveObject.PieceOrder - 1];
	}

	private Coordinate FindDestination(ParsedMoveObject moveObject, Coordinate startingCoordinate)
	{
		if (moveObject.IsFromUcciNotation)
			return moveObject.Destination.Value;

		Piece pieceToMove = _position.GetPieceAtPosition(startingCoordinate);

		MoveDirection moveDirection = moveObject.MoveDirection;

		if (pieceToMove.PieceType.IsMovingInDiagonals() && moveDirection == MoveDirection.Horizontal)
			throw new ArgumentException($"Piece type {EnumHelper<PieceType>.GetDisplayName(pieceToMove.PieceType)} cannot move horizontally");

		Coordinate destination = pieceToMove.GetDestinationCoordinateFromNotation(moveObject.MoveDirection, moveObject.FourthCharacter);

		return destination;
	}
}