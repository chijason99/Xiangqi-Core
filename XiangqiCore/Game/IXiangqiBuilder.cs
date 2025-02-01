using XiangqiCore.Boards;
using XiangqiCore.Move;

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
    XiangqiBuilder RandomisePosition(PieceCounts pieceCounts, bool allowCheck = true);

    XiangqiBuilder WithMoveRecord(string moveRecord, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese);

    XiangqiBuilder WithDpxqGameRecord(string dpxqGameRecord, MoveNotationType moveNotationType = MoveNotationType.SimplifiedChinese);



	XiangqiGame Build();
}