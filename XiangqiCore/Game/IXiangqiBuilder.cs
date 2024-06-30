using XiangqiCore.Boards;

namespace XiangqiCore.Game;

public interface IXiangqiBuilder
{
    XiangqiBuilder UseDefaultConfiguration();
    XiangqiBuilder UseCustomFen(string customFen);
    XiangqiBuilder HasRedPlayer(Action<Player> acction);
    XiangqiBuilder HasBlackPlayer(Action<Player> acction);
    XiangqiBuilder PlayedInCompetition(Action<CompetitionBuilder> action);
    XiangqiBuilder WithGameResult(GameResult gameResult);
    XiangqiBuilder UseBoardConfig(BoardConfig config);
    XiangqiBuilder UseBoardConfig(Action<BoardConfig> action);
    XiangqiGame Build();
}