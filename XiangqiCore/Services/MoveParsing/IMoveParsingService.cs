using XiangqiCore.Move;
using XiangqiCore.Move.MoveObject;

namespace XiangqiCore.Services.MoveParsing;

public interface IMoveParsingService
{
	public ParsedMoveObject ParseMove(string move, MoveNotationType moveNotationType);

	public List<string> ParseGameRecord(string gameRecord);
}
