using XiangqiCore.Boards;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Services.MoveParsing;

namespace XiangqiCore.Move.Commands;

public class NotationMoveCommand : IMoveCommand
{
	private readonly IMoveParsingService _moveParsingService;
	private readonly Board _board;
	private readonly string _moveNotation;
	private readonly Side _sideToMove;
	private readonly MoveNotationType _moveNotationType;

	public NotationMoveCommand(
		IMoveParsingService moveParsingService,
		Board board,
		string moveNotation,
		Side sideToMove,
		MoveNotationType moveNotationType)
	{
		_moveParsingService = moveParsingService;
		_board = board;
		_moveNotation = moveNotation;
		_sideToMove = sideToMove;
		_moveNotationType = moveNotationType;
	}

	public NotationMoveCommand(
		Board board,
		string moveNotation,
		Side sideToMove,
		MoveNotationType moveNotationType) : 
		this(
			new DefaultMoveParsingService(), 
			board, 
			moveNotation, 
			sideToMove, 
			moveNotationType)
	{
	}

	public MoveHistoryObject MoveHistoryObject { get; private set; }

	public MoveHistoryObject Execute()
	{
		ParsedMoveObject parsedMoveObject = _moveParsingService.ParseMove(_moveNotation, _moveNotationType);
		MoveHistoryObject moveHistoryObject = _board.MakeMove(parsedMoveObject, _sideToMove);

		moveHistoryObject.UpdateMoveNotation(_moveNotation, _moveNotationType);

		MoveHistoryObject = moveHistoryObject;
		return moveHistoryObject;
	}

	public void Undo()
	{
		throw new NotImplementedException();
	}
}