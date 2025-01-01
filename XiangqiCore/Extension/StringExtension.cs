using XiangqiCore.Misc;

namespace XiangqiCore.Extension;

public static class StringExtension
{
	public static Dictionary<char, char> SimplifiedToTraditionalChineseMap = new Dictionary<char, char>()
	{
		{ '将', '將' }, 
		{ '帅', '帥' }, 
		{ '车', '車' }, 
		{ '马', '馬' }, 
		{ '进', '進' }, 
		{ '后', '後' }, 
	};

	public static Dictionary<char, char> TraditionalToSimplifiedChineseMap = new Dictionary<char, char>()
	{
		{ '將', '将'}, 
		{ '帥', '帅'}, 
		{ '車', '车'}, 
		{ '馬', '马'}, 
		{ '進', '进'}, 
		{ '後', '后'},
	};

	public static string Translate(this string move, Chinese chinese)
	{
		return string.Concat(move.Select(character =>
		{
			if (chinese == Chinese.Traditional)
			{
				return SimplifiedToTraditionalChineseMap.TryGetValue(character, out var translated) ? translated : character;
			}
			else
			{
				return TraditionalToSimplifiedChineseMap.TryGetValue(character, out var translated) ? translated : character;
			}
		}));
	}

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
