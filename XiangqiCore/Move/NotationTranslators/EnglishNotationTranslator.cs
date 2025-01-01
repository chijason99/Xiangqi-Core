using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators;

public class EnglishNotationTranslator : BaseNotationTranslator
{
	public EnglishNotationTranslator() : base(Language.English)
	{
	}

	public override string Translate(MoveHistoryObject moveHistoryObject)
	{
		char pieceType = GetPieceTypeSymbol(moveHistoryObject.PieceMoved, moveHistoryObject.MovingSide);
		char direction = GetMoveDirectionSymbol(moveHistoryObject.MoveDirection);

		string startingColumn = GetStartingColumn(moveHistoryObject).ToString();

		string fourthCharacter = GetFourthCharacter(moveHistoryObject).ToString();

		return $"{pieceType}{startingColumn}{direction}{fourthCharacter}";
	}
}