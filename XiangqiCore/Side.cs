using XiangqiCore.Attributes;

namespace XiangqiCore;

public enum Side
{
    Red,
    Black,
    [IgnoreFromRandomPick]
    None
}