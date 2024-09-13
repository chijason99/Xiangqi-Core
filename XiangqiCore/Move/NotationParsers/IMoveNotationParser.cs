using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move.NotationParsers;

public interface IMoveNotationParser
{
	ParsedMoveObject Parse(string notation);
	string TranslateToChinese(MoveHistoryObject moveHistoryObject);
	string TranslateToEnglish (MoveHistoryObject moveHistoryObject);
	string TranslateToUcci(MoveHistoryObject moveHistoryObject);
}
