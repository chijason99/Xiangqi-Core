using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;

namespace XiangqiCore.Move.NotationParsers;

public class SimplifiedChineseNotationParser : MoveNotationParserBase
{
	public SimplifiedChineseNotationParser() : base(Language.SimplifiedChinese) { }

	public override ParsedMoveObject Parse(string notation)
	{
		throw new NotImplementedException();
	}
}
