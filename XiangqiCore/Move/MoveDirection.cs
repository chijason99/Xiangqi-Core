using XiangqiCore.Attributes;

namespace XiangqiCore.Move;

public enum MoveDirection
{
    [ChineseName("進")]
    Forward,
    [ChineseName("退")]
    Backward,
    [ChineseName("平")]
    Horizontal
}
