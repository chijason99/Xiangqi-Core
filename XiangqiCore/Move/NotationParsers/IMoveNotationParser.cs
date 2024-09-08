using XiangqiCore.Move.MoveObject;

namespace XiangqiCore.Move.NotationParsers;

public interface IMoveNotationParser
{
	ParsedMoveObject Parse(string notation);
}
