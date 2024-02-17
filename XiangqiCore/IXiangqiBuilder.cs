﻿namespace XiangqiCore;

public interface IXiangqiBuilder
{
    XiangqiBuilder UseDefaultConfiguration();
    XiangqiBuilder UseCustomFen(string customFen);
    XiangqiBuilder HasRedPlayer(Action<Player> acction);
    XiangqiBuilder HasBlackPlayer(Action<Player> acction);
    XiangqiGame Build();
}