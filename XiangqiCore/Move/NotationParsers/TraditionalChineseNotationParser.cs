using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.NotationParsers;

public class TraditionalChineseNotationParser : MoveNotationParserBase
{
	private static char[] ChineseNumbers => ['一', '二', '三', '四', '五', '六', '七', '八', '九'];

	public TraditionalChineseNotationParser() : base(Language.TraditionalChinese) { }

	public Side GetNotationSide(string notation) => notation.Any(char.IsDigit) ? Side.Black : Side.Red;

	public override ParsedMoveObject Parse(string notation)
	{
		string translatedNotation = notation.Translate(Chinese.Traditional);

		Side notationSide = GetNotationSide(translatedNotation);
		bool isMultiColumnPawn = IsMultiColumnPawn(translatedNotation);

		PieceType pieceType = ParsePieceType(translatedNotation);
		int startingColumn = ParseStartingColumn(translatedNotation, notationSide);
		MoveDirection moveDirection = ParseMoveDirection(translatedNotation);
		int foruthCharacter = ParseFourthCharacter(translatedNotation);

		ParsedMoveObject result = new(pieceType, startingColumn, moveDirection, foruthCharacter)
		{
			PieceOrder = isMultiColumnPawn ? ParsePieceOrderIndexForMultiColumnPawn(translatedNotation) : ParsePieceOrder(translatedNotation)
		};

		if (isMultiColumnPawn)
		{
			int minNumberOfPawnsOnColumn = GetMinNumberOfPawnsOnColumn(translatedNotation);
			MultiColumnPawnParsedMoveObject multiColumnPawnResult = new(result, minNumberOfPawnsOnColumn);

			return multiColumnPawnResult;
		}

		return result;
	}

	private int ParseStartingColumn(string notation, Side notationSide)
	{
		const int defaultColumnIndex = 1;
		char secondCharacter = notation[defaultColumnIndex];

		if (notationSide == Side.Black)
			return char.IsDigit(secondCharacter) ?
					(int)char.GetNumericValue(secondCharacter) :
					ParsedMoveObject.UnknownStartingColumn;
		else
			return ChineseNumberParser.TryParse(secondCharacter, out int startingColumn) ?
					startingColumn :
					ParsedMoveObject.UnknownStartingColumn;
	}

	private int ParseFourthCharacter(string notation)
	{
		const int fourthCharacterIndex = 3;
		bool isBlack = notation.Any(char.IsDigit);

		if (isBlack)
		{
			// Use this method instead of int.TryParse as the int.TryParse cannot handle numbers in full-width form
			double fourthCharacterInDouble = char.GetNumericValue(notation[fourthCharacterIndex]);

			return (int)fourthCharacterInDouble;
		}
		else
			return ChineseNumbers.Contains(notation[fourthCharacterIndex]) ? ChineseNumberParser.Parse(notation[fourthCharacterIndex]) : ParsedMoveObject.UnknownStartingColumn;
	}

	private int GetMinNumberOfPawnsOnColumn(string notation)
		=> notation[0] switch
		{
			'中' => 3,
			'前' or '後' => 2,
			_ => ChineseNumberParser.Parse(notation[0])
		};

	private PieceOrder ParsePieceOrderIndexForMultiColumnPawn(string notation)
	{
		char firstCharacter = notation[0];

		return firstCharacter switch
		{
			'前' => PieceOrder.First,
			'中' => PieceOrder.Second,
			'後' => PieceOrder.Last,
			_ => (PieceOrder)ChineseNumberParser.Parse(firstCharacter) - 1
		};
	}
}
