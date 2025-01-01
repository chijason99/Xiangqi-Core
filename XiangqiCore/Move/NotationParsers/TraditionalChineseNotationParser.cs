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
		Side notationSide = GetNotationSide(notation);
		bool isMultiColumnPawn = IsMultiColumnPawn(notation);

		PieceType pieceType = ParsePieceType(notation);
		int startingColumn = ParseStartingColumn(notation, notationSide);
		MoveDirection moveDirection = ParseMoveDirection(notation);
		int foruthCharacter = ParseFourthCharacter(notation);
		PieceOrder pieceOrder = ParsePieceOrder(notation);

		ParsedMoveObject result = new(
			pieceType, 
			startingColumn, 
			moveDirection, 
			foruthCharacter, 
			pieceOrder,
			hasMultiplePieceOfSameTypeOnSameColumn: isMultiColumnPawn || startingColumn == ParsedMoveObject.UnknownStartingColumn);

		if (isMultiColumnPawn)
		{
			int minNumberOfPawnsOnColumn = GetMinNumberOfPawnsOnColumn(notation);
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
}
