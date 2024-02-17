using System.Text.RegularExpressions;

namespace XiangqiCore.FenHelper;

public static partial class FenHelper
{
    public static bool Validate(string fenInput)
    {
        Regex fenRegex = FenRegex();
        Match match = fenRegex.Match(fenInput);

        // 1 overall group, 10 groups for 10 rows on the board, 1 group for turnorder, 1 group for round number, 1 group for turns without capturing
        const int expectedNumberOfGroups = 14;
        
        if (match.Groups.Count != expectedNumberOfGroups) return false;

        // Validate each FEN row
        return Enumerable.Range(1, 10).All(index => ValidateSingleFenRow(match.Groups[index].Value));
    }

    private static bool ValidateSingleFenRow(string fenRow)
    {
        int totalNumberOfColumnsInRow = fenRow
                                        .Select(c => char.IsDigit(c) ? int.Parse(c.ToString()) : 1)
                                        .Sum();

        return totalNumberOfColumnsInRow == 9;
    }


    // Tried splitting the FEN string before, so try with regex this time
    // Regex Key
    // ([1-9rbaknpc]+)... : capturing the group of string composed of 1-9 and rbaknpc, this is for all the rows.
    // \s([bw]) : separated by a space, then capture the turn order of the FEN, indicating by b/w
    // .{5}:  ignore 5 characters, they are not useful in xiangqi fen
    // (\d+): capture the the number of moves without capturing here
    // \s(\d+) : separated by a white space, then capture the round number
    [GeneratedRegex(@"^([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)\s([bw]).{5}(\d+)\s(\d+)", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex FenRegex();
}