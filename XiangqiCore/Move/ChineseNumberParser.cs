namespace XiangqiCore.Move;
public static class ChineseNumberParser
{
    private static readonly Dictionary<char, int> ChineseNumberMap = new ()
    {
        {'零', 0}, 
        {'一', 1}, 
        {'二', 2}, 
        {'三', 3}, 
        {'四', 4}, 
        {'五', 5}, 
        {'六', 6}, 
        {'七', 7}, 
        {'八', 8}, 
        {'九', 9}, 
        {'十', 10}
    };

    public static int Parse(char chineseNumber)
    {
        if (!ChineseNumberMap.TryGetValue(chineseNumber, out int result))
            throw new ArgumentException("The method can only be used for a Chinese Number");

        return result;
    }
}
