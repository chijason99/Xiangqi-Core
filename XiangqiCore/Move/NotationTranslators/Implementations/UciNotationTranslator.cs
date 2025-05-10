using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators.Implementations;

public class UciNotationTranslator : BaseNotationTranslator
{
	public override string Translate(MoveHistoryObject moveHistoryObject)
	{
		string UcciStartingCoordinate = Coordinate.TranslateToUciCoordinate(moveHistoryObject.StartingPosition);
		string UcciDestinationCoordinate = Coordinate.TranslateToUciCoordinate(moveHistoryObject.Destination);

		return $"{UcciStartingCoordinate}{UcciDestinationCoordinate}";
	}
}