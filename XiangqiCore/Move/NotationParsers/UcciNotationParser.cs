using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;

namespace XiangqiCore.Move.NotationParsers;

public class UcciNotationParser : MoveNotationBase
{

	private Dictionary<char, int> UcciColumnMap;
	private Dictionary<int, int> UcciRowMap;
	public UcciNotationParser()
	{
		UcciColumnMap = CreateUcciColumnMap();
		UcciRowMap = CreateUcciRowMap();
	}

	public override ParsedMoveObject Parse(string notation)
	{
		string ucciStartingCoordinate = notation[..2];
		string ucciDestinationCoordinate = notation[^2..];

		Coordinate startingPosition = ParseUcciCoordinate(ucciStartingCoordinate);
		Coordinate destination = ParseUcciCoordinate(ucciDestinationCoordinate);

		return new ParsedMoveObject(startingPosition, destination);
	}

	private Coordinate ParseUcciCoordinate(string ucciCoordinate)
	{
		char ucciColumn = char.ToUpper(ucciCoordinate[0]);
		int ucciRow = int.Parse(ucciCoordinate[1].ToString());

		return new Coordinate(ParseUcciColumn(ucciColumn), ParseUcciRow(ucciRow));
	}

	private Dictionary<char, int> CreateUcciColumnMap()
	{
		Dictionary<char, int> map = [];
		const int columnUnicodeConversionDifference = -64;
		const int unicodeOfA = 65;
		const int unicodeOfI = 73;

		for (int i = unicodeOfI; i >= unicodeOfA; i--)
		{
			char columnLetter = Convert.ToChar(i);
			int correspondingColumn = i + columnUnicodeConversionDifference;

			map[columnLetter] = correspondingColumn;
		}

		return map;
	}

	private int ParseUcciColumn(char ucciColumn) => UcciColumnMap[ucciColumn];
	private int ParseUcciRow(int ucciRow) => UcciRowMap[ucciRow];

	private Dictionary<int, int> CreateUcciRowMap()
	{
		Dictionary<int, int> map = [];
		const int rowConversionDifference = 1;

		for (int i = 0; i <= 9; i++)
		{
			map[i] = i + rowConversionDifference;
		}

		return map;
	}

	public override string TranslateToUcci(MoveHistoryObject moveHistoryObject)
	{
		string UcciStartingCoordinate = Coordinate.TranslateToUcciCoordinate(moveHistoryObject.StartingPosition);
		string UcciDestinationCoordinate = Coordinate.TranslateToUcciCoordinate(moveHistoryObject.StartingPosition);

		return $"{UcciStartingCoordinate}{UcciDestinationCoordinate}";
	}
}
