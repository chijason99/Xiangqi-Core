using XiangqiCore.Misc;

namespace XiangqiCore.Extension;

public static class StringExtension
{
	/// <summary>
	/// Convert a string to full width
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string ToFullWidth(this string input)
	{
		return string.Concat(input.Select(c => c >= '0' && c <= '9' ? (char)(c - '0' + '０') : c));
	}
}
