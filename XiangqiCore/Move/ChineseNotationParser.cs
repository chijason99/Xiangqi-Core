using XiangqiCore.Extension;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move;

public class ChineseNotationParser : MoveNotationBase
{
    private static char[] pieceChineseNames => ['將', '帥', '車', '俥', '馬', '傌', '砲', '炮', '士', '仕', '象', '相', '卒', '兵'];
    private static char[] chineseNumbers => ['一', '二', '三', '四', '五', '六', '七', '八', '九'];
    private static char[] pawnsInChinese => ['卒', '兵'];

    public ChineseNotationParser() { }

    public Side GetNotationSide(string notation) => notation.Any(char.IsDigit) ? Side.Black : Side.Red;

    public override ParsedMoveObject Parse(string notation)
    {
        Side notationSide = GetNotationSide(notation);
        bool isMultiColumnPawn = IsMultiColumnPawn(notation);

        Type pieceType = ParsePieceType(notation);
        int startingColumn = ParseStartingColumn(notation, notationSide);
        MoveDirection moveDirection = ParseMoveDirection(notation);
        int foruthCharacter = ParseFourthCharacter(notation);

        ParsedMoveObject result = new(pieceType, startingColumn, moveDirection, foruthCharacter)
        {
            PieceOrderIndex = isMultiColumnPawn ? ParsePieceOrderIndexForMultiColumnPawn(notation) : ParsePieceOrderIndex(notation)
        };

        if (isMultiColumnPawn)
        {
            int minNumberOfPawnsOnColumn = GetMinNumberOfPawnsOnColumn(notation);
            MultiColumnPawnParsedMoveObject multiColumnPawnResult = new(result, minNumberOfPawnsOnColumn);

            return multiColumnPawnResult;
        }

        return result;
    }

    private Type ParsePieceType(string notation)
    {
        char pieceNameToCheck = pieceChineseNames.Contains(notation[0]) ? notation[0] : notation[1];

        return pieceNameToCheck switch
        {
            '將' or '帥' => typeof(King),
            '車' or '俥' => typeof(Rook),
            '馬' or '傌' => typeof(Knight),
            '砲' or '炮' => typeof(Cannon),
            '士' or '仕' => typeof(Advisor),
            '象' or '相' => typeof(Bishop),
            '卒' or '兵' => typeof(Pawn),
            _ => typeof(Pawn)
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
                    (int) char.GetNumericValue(secondCharacter) : 
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
            double fourthCharacterInDouble = Char.GetNumericValue(notation[fourthCharacterIndex]);

            return (int)fourthCharacterInDouble;
        }
        else
            return chineseNumbers.Contains(notation[fourthCharacterIndex]) ? ChineseNumberParser.Parse(notation[fourthCharacterIndex]) : ParsedMoveObject.UnknownStartingColumn;
    }

    private int ParsePieceOrderIndex(string notation)
        => notation[0] == '前' ? 0 : 1;

    // Multi Column Pawn situation
    // Meaning that there are more than one columns holding two or more pawns of the same color
    private bool IsMultiColumnPawn(string notation) => ParsePieceType(notation) == typeof(Pawn) && 
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
