using XiangqiCore.Boards;

namespace XiangqiCore.Game;

public interface IXiangqiBuilder
{
    XiangqiBuilder WithDefaultConfiguration();

    XiangqiBuilder WithStartingFen(string customFen);

    XiangqiBuilder WithRedPlayer(Action<Player> acction);

    XiangqiBuilder WithBlackPlayer(Action<Player> acction);
    
    XiangqiBuilder WithCompetition(Action<CompetitionBuilder> action);
    
    XiangqiBuilder WithGameResult(GameResult gameResult);
    
    XiangqiBuilder WithBoardConfig(BoardConfig config);
    
    XiangqiBuilder WithBoardConfig(Action<BoardConfig> action);
    
    XiangqiBuilder WithGameName(string gameName);

    XiangqiBuilder RandomisePosition(bool fromFen = true, bool allowCheck = true);

	XiangqiGame Build();
}