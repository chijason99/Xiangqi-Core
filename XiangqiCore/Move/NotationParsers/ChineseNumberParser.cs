namespace XiangqiCore.Move.NotationParsers;

public static class ChineseNumberParser
{
	public static int Parse(char chineseNumber)
	{
		int result = chineseNumber switch
		{
			'一' => 1,
			'二' => 2,
			'三' => 3,
			'四' => 4,
			'五' => 5,
			'六' => 6,
			'七' => 7,
			'八' => 8,
			'九' => 9,
			'十' => 10,
			_ => throw new ArgumentException("The method can only be used for a Chinese Number")
		};

		return result;
	}

	public static bool TryParse(char chineseNumber, out int result)
	{
		try
		{
			result = Parse(chineseNumber);

			return true;
		}
		catch (ArgumentException)
		{
			result = 0;
			return false;
		}
	}
}
