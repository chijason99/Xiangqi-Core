using XiangqiCore.Extension;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Misc;

namespace XiangqiCore.Move.NotationTranslators;

public class SimplifiedChineseNotationTranslator : BaseNotationTranslator
{
	public SimplifiedChineseNotationTranslator() : base(Language.SimplifiedChinese)
	{
	}

	public override string Translate(MoveHistoryObject moveHistoryObject)
	{
		char pieceType = GetPieceTypeSymbol(moveHistoryObject.PieceMoved, moveHistoryObject.MovingSide);
		char direction = GetMoveDirectionSymbol(moveHistoryObject.MoveDirection);
		
		int startingColumn = GetStartingColumn(moveHistoryObject);
		int fourthCharacter = GetFourthCharacter(moveHistoryObject);

		string startingColumnString = moveHistoryObject.MovingSide == Side.Red ?
			startingColumn.ToChineseNumeral() : startingColumn.ToString().ToFullWidth();

		string fourthCharacterString = moveHistoryObject.MovingSide == Side.Red ?
			fourthCharacter.ToChineseNumeral() : fourthCharacter.ToString().ToFullWidth();

		return $"{pieceType}{startingColumn}{direction}{fourthCharacter}";
	}
}