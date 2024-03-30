﻿using XiangqiCore.Boards;

namespace XiangqiCore;

public interface IXiangqiBuilder
{
    XiangqiBuilder UseDefaultConfiguration();
    XiangqiBuilder UseCustomFen(string customFen);
    XiangqiBuilder HasRedPlayer(Action<Player> acction);
    XiangqiBuilder HasBlackPlayer(Action<Player> acction);
    XiangqiBuilder PlayedInCompetition(string competitionName);
    XiangqiBuilder PlayedOnDate(DateTime gameDate);
    XiangqiBuilder UseBoardConfig(BoardConfig config);
    XiangqiBuilder UseBoardConfig(Action<BoardConfig> action);
    XiangqiGame Build();
}