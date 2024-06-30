using XiangqiCore.Attributes;

namespace XiangqiCore.Misc;

public enum Side
{
    Red,
    Black,
    [IgnoreFromRandomPick]
    None
}