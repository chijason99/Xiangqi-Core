using System.Text.RegularExpressions;

namespace XiangqiCore.Misc;

public static partial class GameRecordParser
{
    /// <summary>
    /// Regex to match individual move elements within a move set. This regex is designed to capture
    /// sequences of exactly four characters, where each character can be a CJK Unified Ideograph,
    /// a half-width number (0-9), a full-width number (０-９), an English letter (a-zA-Z), or one of the
    /// symbols +, -, or =. This allows for the extraction of moves that include a variety of character
    /// types, accommodating different notation styles used in game records.
    /// </summary>
    [GeneratedRegex(@"[\p{IsCJKUnifiedIdeographs}\d\uFF10-\uFF19a-zA-Z\+\-=]{4}")]
    private static partial Regex MoveSetElementRegex();

    public static List<string> Parse(string gameRecordString)
    {
        List<string> moveSets = [];

        // Split the game record string by newline characters
        string[] lines = gameRecordString.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            // Remove leading and trailing whitespace from each line
            string trimmedLine = line.Trim();

            // Skip empty lines
            if (trimmedLine.Length == 0)
                continue;

            // Extract the move set from the line
            MatchCollection moveSet = MoveSetElementRegex().Matches(trimmedLine);

            // Add the move set to the result list
            foreach (Match matchedMoveSet in moveSet)
                moveSets.Add(matchedMoveSet.Value);
        }

        return moveSets;
    }
}
