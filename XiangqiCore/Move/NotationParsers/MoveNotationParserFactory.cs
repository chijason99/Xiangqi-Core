using XiangqiCore.Move.NotationParser;

namespace XiangqiCore.Move.NotationParsers;

public static class MoveNotationParserFactory
{
	public static IMoveNotationParser GetParser(MoveNotationType notationType)
		=> notationType switch
		{
			MoveNotationType.TraditionalChinese => MoveNotationParserBase.GetMoveNotationParserInstance<TraditionalChineseNotationParser>(),
			MoveNotationType.SimplifiedChinese => MoveNotationParserBase.GetMoveNotationParserInstance<SimplifiedChineseNotationParser>(),
			MoveNotationType.English => MoveNotationParserBase.GetMoveNotationParserInstance<EnglishNotationParser>(),
			MoveNotationType.UCCI => MoveNotationParserBase.GetMoveNotationParserInstance<UcciNotationParser>(),
			_ => throw new ArgumentException("Invalid Move Notation Type")
		};
}