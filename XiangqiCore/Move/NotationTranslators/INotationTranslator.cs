using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationTranslators;

public interface INotationTranslator
{
	/// <summary>
	/// Translates a MoveNotation into another format.
	/// </summary>
	/// <param name="moveHistoryObject"></param>
	/// <returns></returns>
	string Translate(MoveHistoryObject moveHistoryObject);
}
