using XiangqiCore.Boards;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.Commands;

public class CoordinateMoveCommand(
	Board board, 
	Coordinate from, 
	Coordinate to,
	Side sideToMove) : IMoveCommand
{
	private readonly Board _board = board;
	private readonly Coordinate _from = from;
	private readonly Coordinate _to = to;
	private readonly Side _sideToMove = sideToMove;

	public MoveHistoryObject MoveHistoryObject { get; private set; }


	public MoveHistoryObject Execute()
	{
		MoveHistoryObject moveHistoryObject = _board.MakeMove(_from, _to, _sideToMove);

		MoveHistoryObject = moveHistoryObject;

		return moveHistoryObject;
	}

	public MoveHistoryObject Undo()
	{
		_board.UndoMove(MoveHistoryObject);

		return MoveHistoryObject;
	}
}
