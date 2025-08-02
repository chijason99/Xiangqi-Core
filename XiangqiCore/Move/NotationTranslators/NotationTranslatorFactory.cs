using System.Collections.Concurrent;
using XiangqiCore.Move.NotationTranslators.Implementations;

namespace XiangqiCore.Move.NotationTranslators;

public static class NotationTranslatorFactory
{
	private static readonly ConcurrentDictionary<MoveNotationType, INotationTranslator> _translatorCache = [];

	public static INotationTranslator GetTranslator(MoveNotationType targetNotationType)
	{
		return _translatorCache.GetOrAdd(targetNotationType, static type =>
		{
			return type switch
			{
				MoveNotationType.TraditionalChinese => new TraditionalChineseNotationTranslator(),
				MoveNotationType.SimplifiedChinese => new SimplifiedChineseNotationTranslator(),
				MoveNotationType.English => new EnglishNotationTranslator(),
				MoveNotationType.UCCI => new UcciNotationTranslator(),
				MoveNotationType.UCI => new UciNotationTranslator(),
				_ => throw new NotSupportedException($"Notation type {type} is not supported."),
			};
		});
	}
}
