using XiangqiCore.Boards;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

public interface IMoveCommand
{
	MoveHistoryObject MoveHistoryObject { get; }

	/// <summary>
	/// Perform a move
	/// </summary>
	/// <returns>The move history object that stores information about the move</returns>
	MoveHistoryObject Execute(Board board);

	/// <summary>
	/// Undo a move
	/// </summary>
	/// <returns>The move history object that it undid</returns>
	MoveHistoryObject Undo(Board board);
}