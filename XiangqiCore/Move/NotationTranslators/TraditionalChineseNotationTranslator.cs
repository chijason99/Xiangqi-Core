using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators;

public class TraditionalChineseNotationTranslator : BaseNotationTranslator
{
	public TraditionalChineseNotationTranslator() : base(Language.TraditionalChinese)
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

		return $"{pieceType}{startingColumnString}{direction}{fourthCharacterString}";
	}
}
