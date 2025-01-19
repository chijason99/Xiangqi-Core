using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

public interface IMoveCommand
{
	MoveHistoryObject MoveHistoryObject { get; }

	MoveHistoryObject Execute();

	void Undo();
}