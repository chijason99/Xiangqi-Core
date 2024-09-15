using XiangqiCore.Boards;

namespace XiangqiCore.Game;

public interface IXiangqiBuilder
{
    XiangqiBuilder UseDefaultConfiguration();
    XiangqiBuilder UseCustomFen(string customFen);
    XiangqiBuilder WithRedPlayer(Action<Player> acction);
    XiangqiBuilder WithBlackPlayer(Action<Player> acction);
    XiangqiBuilder WithCompetition(Action<CompetitionBuilder> action);
    XiangqiBuilder WithGameResult(GameResult gameResult);
    XiangqiBuilder UseBoardConfig(BoardConfig config);
    XiangqiBuilder UseBoardConfig(Action<BoardConfig> action);
    Task<XiangqiGame> BuildAsync();
}