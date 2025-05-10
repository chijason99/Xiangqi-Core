using System.Text.RegularExpressions;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;

namespace XiangqiCore.Move.NotationParsers.Implementations;

public partial class UciNotationParser : MoveNotationParserBase
{
	private readonly Dictionary<char, int> UciColumnMap;

	public UciNotationParser()
	{
		UciColumnMap = CreateUciColumnMap();
	}

	public override ParsedMoveObject Parse(string notation)
	{
		Regex uciNotationRegex = UciNotationRegex();
		Match uciMatch = uciNotationRegex.Match(notation);

		if (!uciMatch.Success)
			throw new ArgumentException($"Invalid UCI notation: '{notation}'. Expected format: '<startingColumn><startingRow><destinationColumn><destinationRow>' (e.g., 'e2e4').");
		
		// Parse the starting coordinate
		Coordinate startingPoint = new(
			UciColumnMap[uciMatch.Groups["startingColumn"].Value[0]],
			int.Parse(uciMatch.Groups["startingRow"].Value)
		);

		// Parse the destination coordinate
		Coordinate destination = new(
			UciColumnMap[uciMatch.Groups["destinationColumn"].Value[0]],
			int.Parse(uciMatch.Groups["destinationRow"].Value)
		);

		return new ParsedMoveObject(startingPoint, destination);
	}

	private Dictionary<char, int> CreateUciColumnMap()
	{
		Dictionary<char, int> map = [];
		const int columnUnicodeConversionDifference = -96;
		const int unicodeOfA = 97;
		const int unicodeOfI = 105;

		for (int i = unicodeOfI; i >= unicodeOfA; i--)
		{
			char columnLetter = Convert.ToChar(i);
			int correspondingColumn = i + columnUnicodeConversionDifference;

			map[columnLetter] = correspondingColumn;
		}

		return map;
	}

	[GeneratedRegex(@"^(?<startingColumn>\w)(?<startingRow>\d+)(?<destinationColumn>\w)(?<destinationRow>\d+)")]
	private static partial Regex UciNotationRegex();
}
