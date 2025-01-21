using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

public class MoveCommandInvoker
{
	private readonly Stack<IMoveCommand> _moveCommands = [];

	public MoveHistoryObject ExecuteCommand(IMoveCommand moveCommand)
	{
		_moveCommands.Push(moveCommand);
		
		MoveHistoryObject moveHistoryObject = moveCommand.Execute();

		return moveHistoryObject;
	}

	public void Undo()
	{
		throw new NotImplementedException();
	}
}