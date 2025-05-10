using System.Text.RegularExpressions;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.NotationParsers.Implementations;

namespace XiangqiCore.Services.MoveParsing;

public partial class DefaultMoveParsingService : IMoveParsingService
{
	private readonly TraditionalChineseNotationParser _traitionalChineseNotationParser;
	private readonly SimplifiedChineseNotationParser _simplifiedChineseNotationParser;
	private readonly EnglishNotationParser _englishNotationParser;
	private readonly UcciNotationParser _ucciNotationParser;
	private readonly UciNotationParser _uciNotationParser;

	public DefaultMoveParsingService()
	{
		_traitionalChineseNotationParser = new();
		_simplifiedChineseNotationParser = new();
		_englishNotationParser = new();
		_ucciNotationParser = new();
		_uciNotationParser = new();
	}

	public ParsedMoveObject ParseMove(string move, MoveNotationType moveNotationType)
		=> moveNotationType switch
		{
			MoveNotationType.TraditionalChinese => _traitionalChineseNotationParser.Parse(move),
			MoveNotationType.SimplifiedChinese => _simplifiedChineseNotationParser.Parse(move),
			MoveNotationType.English => _englishNotationParser.Parse(move),
			MoveNotationType.UCCI => _ucciNotationParser.Parse(move),
			MoveNotationType.UCI => _uciNotationParser.Parse(move),
			_ => throw new NotImplementedException()
		};


	public List<string> ParseGameRecord(string gameRecordString)
	{
		List<string> moveSets = [];

		// Split the game record string by newline characters
		string[] lines = gameRecordString.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		Regex regexPattern = MoveRecordRegex();

		foreach (string line in lines)
		{
			// Remove leading and trailing whitespace from each line
			string trimmedLine = line.Trim();

			// Skip empty lines
			if (trimmedLine.Length == 0)
				continue;

			// Extract the move set from the line
			MatchCollection moveSet = regexPattern.Matches(trimmedLine);

			// Add the move set to the result list
			foreach (Match matchedMoveSet in moveSet)
				moveSets.Add(matchedMoveSet.Value);
		}

		return moveSets;
	}

	/// <summary>
	/// Regex to match individual move elements within a move set. This regex is designed to capture
	/// sequences of exactly four characters, where each character can be a CJK Unified Ideograph,
	/// a half-width number (0-9), a full-width number (０-９), an English letter (a-zA-Z), or one of the
	/// symbols +, -, or =. This allows for the extraction of moves that include a variety of character
	/// types, accommodating different notation styles used in game records.
	/// </summary>
	[GeneratedRegex(@"[\p{IsCJKUnifiedIdeographs}\d\uFF10-\uFF19a-zA-Z\+\-=]{4}")]
	private static partial Regex MoveRecordRegex();
}
