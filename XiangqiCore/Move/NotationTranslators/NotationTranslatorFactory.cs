using XiangqiCore.Misc;

namespace XiangqiCore.Move.NotationTranslators;

public static class NotationTranslatorFactory
{
	public static INotationTranslator GetTranslator(MoveNotationType targetNotationType, Language language = Language.NotSpecified)
	{
		return targetNotationType switch
		{
			MoveNotationType.TraditionalChinese => language switch
			{
				Language.SimplifiedChinese => new SimplifiedChineseNotationTranslator(),
				Language.TraditionalChinese => new TraditionalChineseNotationTranslator(),
				_ => throw new NotSupportedException($"Language {language} is not supported for Chinese notation."),
			},
			MoveNotationType.English => new EnglishNotationTranslator(),
			MoveNotationType.UCCI => new UcciNotationTranslator(),
			_ => throw new NotSupportedException($"Notation type {targetNotationType} is not supported."),
		};
	}
}
