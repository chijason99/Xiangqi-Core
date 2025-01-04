using XiangqiCore.Move.MoveObject;

namespace XiangqiCore.Move.NotationParsers;

public interface INotationParser
{
	ParsedMoveObject Parse(string notation);
}
