using XiangqiCore.Misc;

namespace XiangqiCore.Extension;
public static class IntExtension
{
    public static int ConvertStepsBaseOnSide(this int originalValue, Side side)
        => side == Side.Red ? originalValue : originalValue * -1;

    public static int ConvertToColumnBasedOnSide(this int originalValue, Side side)
        => side == Side.Red ? 10 - originalValue : originalValue;
}
