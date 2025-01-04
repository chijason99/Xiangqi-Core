using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators.Implementations;

public class UcciNotationTranslator : BaseNotationTranslator
{
	public override string Translate(MoveHistoryObject moveHistoryObject)
	{
		string UcciStartingCoordinate = Coordinate.TranslateToUcciCoordinate(moveHistoryObject.StartingPosition);
		string UcciDestinationCoordinate = Coordinate.TranslateToUcciCoordinate(moveHistoryObject.Destination);

		return $"{UcciStartingCoordinate}{UcciDestinationCoordinate}";
	}
}