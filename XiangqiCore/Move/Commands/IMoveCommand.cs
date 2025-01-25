using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

public interface IMoveCommand
{
	MoveHistoryObject MoveHistoryObject { get; }

	/// <summary>
	/// Perform a move
	/// </summary>
	/// <returns>The move history object that stores information about the move</returns>
	MoveHistoryObject Execute();

	/// <summary>
	/// Undo a move
	/// </summary>
	/// <returns>The move history object that it undid</returns>
	MoveHistoryObject Undo();
}