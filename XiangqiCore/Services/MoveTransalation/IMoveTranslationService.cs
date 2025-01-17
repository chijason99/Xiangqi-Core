using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Services.MoveTransalation;

public interface IMoveTranslationService
{
	public string TranslateMove(MoveHistoryObject move, MoveNotationType notationType);
}