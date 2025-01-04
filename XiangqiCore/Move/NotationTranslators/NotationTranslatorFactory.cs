using XiangqiCore.Move.NotationTranslators.Implementations;

namespace XiangqiCore.Move.NotationTranslators;

public static class NotationTranslatorFactory
{
	private static readonly Dictionary<MoveNotationType, INotationTranslator> _translatorCache = [];

	public static INotationTranslator GetTranslator(MoveNotationType targetNotationType)
	{
		if (_translatorCache.TryGetValue(targetNotationType, out INotationTranslator? translator))
		{
			return translator;
		}
		else
		{
			translator = targetNotationType switch
			{
				MoveNotationType.TraditionalChinese => new TraditionalChineseNotationTranslator(),
				MoveNotationType.SimplifiedChinese => new SimplifiedChineseNotationTranslator(),
				MoveNotationType.English => new EnglishNotationTranslator(),
				MoveNotationType.UCCI => new UcciNotationTranslator(),
				_ => throw new NotSupportedException($"Notation type {targetNotationType} is not supported."),
			};

			_translatorCache[targetNotationType] = translator;
			return translator;
		}
	}
}
