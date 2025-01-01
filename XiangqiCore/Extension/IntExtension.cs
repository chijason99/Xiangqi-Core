using XiangqiCore.Misc;

namespace XiangqiCore.Extension;
public static class IntExtension
{
    public static int ConvertStepsBaseOnSide(this int originalValue, Side side)
        => side == Side.Red ? originalValue : originalValue * -1;

    public static int ConvertToColumnBasedOnSide(this int originalValue, Side side)
        => side == Side.Red ? 10 - originalValue : originalValue;

    public static string ToChineseNumeral(this int originalValue)
		=> originalValue switch
		{
			1 => "一",
			2 => "二",
			3 => "三",
			4 => "四",
			5 => "五",
			6 => "六",
			7 => "七",
			8 => "八",
			9 => "九",
			_ => throw new ArgumentException("Invalid Column Value")
		};
}
