using XiangqiCore.Boards;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

/// <summary>
/// A class responsible for invoking move commands in the game of Xiangqi (Chinese Chess).
/// </summary>
/// <param name="board"></param>
public class MoveCommandInvoker(Board board)
{
	private readonly Board _board = board;

	/// <summary>
	/// It executes a move command on the board and returns a MoveHistoryObject that contains information about the move.
	/// </summary>
	/// <param name="moveCommand"></param>
	/// <returns></returns>
	public MoveHistoryObject ExecuteCommand(IMoveCommand moveCommand)
	{
		var moveHistoryObject = moveCommand.Execute(_board);

		return moveHistoryObject;
	}
	
	/// <summary>
	///  Undoes a move command on the board and returns a MoveHistoryObject that contains information about the move that was undone.
	/// </summary>
	/// <param name="moveCommand"></param>
	/// <returns></returns>
	public MoveHistoryObject UndoCommand(IMoveCommand moveCommand)
	{
		var moveHistoryObject = moveCommand.Undo(_board);

		return moveHistoryObject;
	}
}