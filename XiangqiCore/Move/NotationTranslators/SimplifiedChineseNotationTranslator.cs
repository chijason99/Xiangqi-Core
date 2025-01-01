using XiangqiCore.Extension;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Misc;

namespace XiangqiCore.Move.NotationTranslators;

public class SimplifiedChineseNotationTranslator : BaseNotationTranslator
{
	protected SimplifiedChineseNotationTranslator() : base(Language.SimplifiedChinese)
	{
	}

	public override string Translate(MoveHistoryObject moveHistoryObject)
	{
		string pieceType = GetPieceTypeSymbol(moveHistoryObject.PieceMoved, moveHistoryObject.MovingSide);
		string direction = GetMoveDirectionSymbol(moveHistoryObject.MoveDirection, moveHistoryObject.MovingSide);
		
		int startingColumn = GetStartingColumn(moveHistoryObject);
		int fourthCharacter = GetFourthCharacter(moveHistoryObject);

		string startingColumnString = moveHistoryObject.MovingSide == Side.Red ?
			startingColumn.ToChineseNumeral() : startingColumn.ToString();

		string fourthCharacterString = moveHistoryObject.MovingSide == Side.Red ?
			fourthCharacter.ToChineseNumeral() : fourthCharacter.ToString();

		return $"{pieceType}{startingColumn}{direction}{fourthCharacter}";
	}
}