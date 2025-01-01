using XiangqiCore.Attributes;
using XiangqiCore.Misc;

namespace XiangqiCore.Move;

public enum MoveDirection
{
    [Symbol(Language.TraditionalChinese, "進")]
    [Symbol(Language.SimplifiedChinese, "进")]
    [Symbol(Language.English, "+")]
    Forward,

    [Symbol(Language.TraditionalChinese, "退")]
	[Symbol(Language.English, "-")]
	Backward,

    [Symbol(Language.TraditionalChinese, "平")]
    [Symbol(Language.English, "=")]
    Horizontal
}
