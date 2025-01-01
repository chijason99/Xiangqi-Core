using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.NotationTranslators;

public abstract class BaseNotationTranslator(Language language = Language.NotSpecified) : INotationTranslator
{
	public Language Language { get; init; } = language;

	public abstract string Translate(MoveHistoryObject moveHistoryObject);

	protected virtual string GetPieceTypeSymbol(PieceType pieceType, Side side)
		=> EnumHelper<PieceType>.GetSymbol(pieceType, Language, side);

	protected virtual string GetMoveDirectionSymbol(MoveDirection moveDirection, Side side)
		=> EnumHelper<MoveDirection>.GetSymbol(moveDirection, Language, side);

	protected virtual int GetFourthCharacter(MoveHistoryObject moveHistoryObject)
	{
		// If the piece is moving in diagonals or if the piece is moving horizontally, the fourth character is the destination column
		if (moveHistoryObject.PieceMoved.IsMovingInDiagonals() || moveHistoryObject.MoveDirection == MoveDirection.Horizontal)
		{
			return moveHistoryObject.Destination
				.Column
				.ConvertToColumnBasedOnSide(moveHistoryObject.MovingSide);
		}
		// If the piece is moving vertically, the fourth character is the number of rows moved
		else
		{
			return Math.Abs(moveHistoryObject.Destination.Row - moveHistoryObject.StartingPosition.Row);
		}
	}

	protected virtual int GetStartingColumn(MoveHistoryObject moveHistoryObject)
		=> moveHistoryObject.StartingPosition
			.Column
			.ConvertToColumnBasedOnSide(moveHistoryObject.MovingSide);
}
