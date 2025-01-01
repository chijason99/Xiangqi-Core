using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators;

public class EnglishNotationTranslator : BaseNotationTranslator
{
	protected EnglishNotationTranslator() : base(Language.English)
	{
	}

	public override string Translate(MoveHistoryObject moveHistoryObject)
	{
		string pieceType = GetPieceTypeSymbol(moveHistoryObject.PieceMoved, moveHistoryObject.MovingSide);
		string direction = GetMoveDirectionSymbol(moveHistoryObject.MoveDirection, moveHistoryObject.MovingSide);

		string startingColumn = GetStartingColumn(moveHistoryObject).ToString();

		string fourthCharacter = GetFourthCharacter(moveHistoryObject).ToString();

		return $"{pieceType}{startingColumn}{direction}{fourthCharacter}";
	}
}