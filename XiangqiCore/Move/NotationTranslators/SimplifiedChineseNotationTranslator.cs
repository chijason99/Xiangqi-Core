using XiangqiCore.Extension;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Misc;

namespace XiangqiCore.Move.NotationTranslators;

public class SimplifiedChineseNotationTranslator : BaseNotationTranslator
{
	public SimplifiedChineseNotationTranslator() : base(Language.SimplifiedChinese)
	{
	}

	protected override string GetSecondCharacter(MoveHistoryObject moveHistoryObject)
	{
		if (moveHistoryObject.HasMultiplePieceOfSameTypeOnSameColumn)
			return GetPieceTypeSymbol(moveHistoryObject.PieceMoved, moveHistoryObject.MovingSide).ToString();
		else
		{
			return moveHistoryObject.MovingSide == Side.Red ?
				GetStartingColumn(moveHistoryObject).ToChineseNumeral().ToString() :
				GetStartingColumn(moveHistoryObject).ToString().ToFullWidth();
		}
	}

	protected override string GetFourthCharacter(MoveHistoryObject moveHistoryObject)
	{
		int fourthCharacter;

		// If the piece is moving in diagonals or if the piece is moving horizontally, the fourth character is the destination column
		if (moveHistoryObject.PieceMoved.IsMovingInDiagonals() || moveHistoryObject.MoveDirection == MoveDirection.Horizontal)
			fourthCharacter = moveHistoryObject.Destination
				.Column
				.ConvertToColumnBasedOnSide(moveHistoryObject.MovingSide);
		// If the piece is moving vertically, the fourth character is the number of rows moved
		else
			fourthCharacter = Math.Abs(moveHistoryObject.Destination.Row - moveHistoryObject.StartingPosition.Row);

		return moveHistoryObject.MovingSide == Side.Red ?
			fourthCharacter.ToChineseNumeral().ToString() :
			fourthCharacter.ToString().ToFullWidth();
	}
}