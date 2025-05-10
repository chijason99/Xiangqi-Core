using XiangqiCore.Move.NotationParsers.Implementations;

namespace XiangqiCore.Move.NotationParsers;

public static class NotationParserFactory
{
	public static INotationParser GetParser(MoveNotationType notationType)
		=> notationType switch
		{
			MoveNotationType.TraditionalChinese => MoveNotationParserBase.GetMoveNotationParserInstance<TraditionalChineseNotationParser>(),
			MoveNotationType.SimplifiedChinese => MoveNotationParserBase.GetMoveNotationParserInstance<SimplifiedChineseNotationParser>(),
			MoveNotationType.English => MoveNotationParserBase.GetMoveNotationParserInstance<EnglishNotationParser>(),
			MoveNotationType.UCCI => MoveNotationParserBase.GetMoveNotationParserInstance<UcciNotationParser>(),
			MoveNotationType.UCI => MoveNotationParserBase.GetMoveNotationParserInstance<UciNotationParser>(),
			_ => throw new ArgumentException("Invalid Move Notation Type")
		};
}