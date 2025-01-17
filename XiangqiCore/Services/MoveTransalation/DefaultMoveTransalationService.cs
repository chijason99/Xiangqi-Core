using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Move.NotationTranslators.Implementations;

namespace XiangqiCore.Services.MoveTransalation;

public class DefaultMoveTransalationService : IMoveTranslationService
{
	private readonly TraditionalChineseNotationTranslator _traitionalChineseNotationTranslator;
	private readonly SimplifiedChineseNotationTranslator _simplifiedChineseNotationTranslator;
	private readonly EnglishNotationTranslator _englishNotationTranslator;
	private readonly UcciNotationTranslator _ucciNotationTranslator;

	public DefaultMoveTransalationService()
	{
		_traitionalChineseNotationTranslator = new();
		_simplifiedChineseNotationTranslator = new();
		_englishNotationTranslator = new();
		_ucciNotationTranslator = new();
	}

	/// <summary>
	/// Translates a MoveNotation into another format.
	/// </summary>
	/// <param name="move"></param>
	/// <param name="notationType">Notation Type to translate to</param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public string TranslateMove(MoveHistoryObject move, MoveNotationType notationType)
		=> notationType switch
		{
			MoveNotationType.TraditionalChinese => _traitionalChineseNotationTranslator.Translate(move),
			MoveNotationType.SimplifiedChinese => _simplifiedChineseNotationTranslator.Translate(move),
			MoveNotationType.English => _englishNotationTranslator.Translate(move),
			MoveNotationType.UCCI => _ucciNotationTranslator.Translate(move),
			_ => throw new NotImplementedException()
		};
}
