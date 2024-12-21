using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.NotationParsers;

public class ChineseNotationParser : MoveNotationBase
{
	private static char[] pieceChineseNames => ['將', '帥', '車', '俥', '馬', '傌', '砲', '炮', '士', '仕', '象', '相', '卒', '兵'];
	private static char[] chineseNumbers => ['一', '二', '三', '四', '五', '六', '七', '八', '九'];
	private static char[] pawnsInChinese => ['卒', '兵'];

	public ChineseNotationParser() { }

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
			PieceOrderIndex = isMultiColumnPawn ? ParsePieceOrderIndexForMultiColumnPawn(translatedNotation) : ParsePieceOrderIndex(translatedNotation)
		};

		if (isMultiColumnPawn)
		{
			int minNumberOfPawnsOnColumn = GetMinNumberOfPawnsOnColumn(translatedNotation);
			MultiColumnPawnParsedMoveObject multiColumnPawnResult = new(result, minNumberOfPawnsOnColumn);

			return multiColumnPawnResult;
		}

		return result;
	}

	private PieceType ParsePieceType(string notation)
	{
		char pieceNameToCheck = pieceChineseNames.Contains(notation[0]) ? notation[0] : notation[1];

		return pieceNameToCheck switch
		{
			'將' or '帥' => PieceType.King,
			'車' or '俥' => PieceType.Rook,
			'馬' or '傌' => PieceType.Knight,
			'砲' or '炮' => PieceType.Cannon,
			'士' or '仕' => PieceType.Advisor,
			'象' or '相' => PieceType.Bishop,
			'卒' or '兵' => PieceType.Pawn,
			_ => PieceType.Pawn
		};
	}

	private MoveDirection ParseMoveDirection(string notation)
		=> notation[2] switch
		{
			'進' => MoveDirection.Forward,
			'退' => MoveDirection.Backward,
			'平' => MoveDirection.Horizontal,
			_ => throw new ArgumentException("Invalid Move Direction")
		};

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
			return chineseNumbers.Contains(notation[fourthCharacterIndex]) ? ChineseNumberParser.Parse(notation[fourthCharacterIndex]) : ParsedMoveObject.UnknownStartingColumn;
	}

	private int ParsePieceOrderIndex(string notation)
		=> notation[0] == '後' ? 1 : notation[0] == '前' ? 0 : ParsedMoveObject.UnknownPieceOrderIndex;

	// Multi Column Pawn situation
	// Meaning that there are more than one columns holding two or more pawns of the same color
	private bool IsMultiColumnPawn(string notation) => ParsePieceType(notation) == PieceType.Pawn &&
													   notation.IndexOfAny(pawnsInChinese) != 0;

	private int GetMinNumberOfPawnsOnColumn(string notation)
		=> notation[0] switch
		{
			'中' => 3,
			'前' or '後' => 2,
			_ => ChineseNumberParser.Parse(notation[0])
		};

	private int ParsePieceOrderIndexForMultiColumnPawn(string notation)
	{
		char firstCharacter = notation[0];

		return firstCharacter switch
		{
			'前' => 0,
			'中' => 1,
			'後' => MultiColumnPawnParsedMoveObject.LastPawnIndex,
			_ => ChineseNumberParser.Parse(firstCharacter) - 1
		};
	}
}
