using XiangqiCore.Misc;

namespace XiangqiCore.Move.NotationTranslators;

public static class NotationTranslatorFactory
{
	public static INotationTranslator GetTranslator(MoveNotationType targetNotationType)
	{
		return targetNotationType switch
		{
			MoveNotationType.TraditionalChinese => new TraditionalChineseNotationTranslator(),
			MoveNotationType.SimplifiedChinese => new SimplifiedChineseNotationTranslator(),
			MoveNotationType.English => new EnglishNotationTranslator(),
			MoveNotationType.UCCI => new UcciNotationTranslator(),
			_ => throw new NotSupportedException($"Notation type {targetNotationType} is not supported."),
		};
	}
}
