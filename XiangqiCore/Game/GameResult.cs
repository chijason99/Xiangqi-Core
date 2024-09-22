using System.ComponentModel;

namespace XiangqiCore.Game;

public enum GameResult
{
    [Description("1-0")]
    RedWin,
    [Description("0-1")]
    BlackWin,
    [Description("1/2-1/2")]
    Draw,
    Unknown
}
