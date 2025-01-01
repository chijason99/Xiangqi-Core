using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators;

public interface IMoveNotationTranslator
{
	string Translate(MoveHistoryObject moveHistoryObject);
}
