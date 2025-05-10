using XiangqiCore.Boards;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

public class MoveCommandInvoker(Board board)
{
	private readonly Board _board = board;
	private readonly Stack<IMoveCommand> _moveCommands = [];
	private readonly Stack<IMoveCommand> _undoCommands = [];

	public MoveHistoryObject ExecuteCommand(IMoveCommand moveCommand)
	{
		_moveCommands.Push(moveCommand);
		
		MoveHistoryObject moveHistoryObject = moveCommand.Execute(_board);

		return moveHistoryObject;
	}

	public MoveHistoryObject? UndoCommand(int numberOfMovesToUndo = 1)
	{
		for (int i = 0; i < numberOfMovesToUndo; i++)
		{
			IMoveCommand moveCommand = _moveCommands.Pop();
			moveCommand.Undo(_board);

			_undoCommands.Push(moveCommand);
		}

		return _moveCommands.Any() ? _moveCommands.Peek().MoveHistoryObject : null;
	}

	public List<MoveHistoryObject> GetMoveHistories()
	{
		List<MoveHistoryObject> result = new(_moveCommands.Count);

		foreach (IMoveCommand moveCommand in _moveCommands)
			result.Add(moveCommand.MoveHistoryObject);

		result.Reverse();

		return result;
	}
}