using XiangqiCore.Extension;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move;

public class ChineseNotationParser : MoveNotationBase
{
    private static char[] pieceChineseNames => ['將', '帥', '車', '俥', '馬', '傌', '砲', '炮', '士', '仕', '象', '相', '卒', '兵'];
    private static char[] chineseNumbers => ['一', '二', '三', '四', '五', '六', '七', '八', '九'];

    public ChineseNotationParser() { }

    public override ParsedMoveObject Parse(string notation)
    {
        Type pieceType = ParsePieceType(notation);
        int startingColumn = ParseStartingColumn(notation);
        MoveDirection moveDirection = ParseMoveDirection(notation);
        int foruthCharacter = ParseFourthCharacter(notation);

        ParsedMoveObject result = new(pieceType, startingColumn, moveDirection, foruthCharacter);

        if (startingColumn == ParsedMoveObject.UnknownStartingColumn)
            result.PieceOrderIndex = ParsePieceOrderIndex(notation);

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

    private int ParseStartingColumn(string notation)
    {
        const int defaultColumnIndex = 1;
        char secondCharacter = notation[defaultColumnIndex];
        bool isBlack = notation.Any(char.IsDigit);

        if (isBlack)
        {
            bool successfulParse = int.TryParse(secondCharacter.ToString(), out int startingColumn);

            return successfulParse ? startingColumn.ConvertToColumnBasedOnSide(Side.Black) : ParsedMoveObject.UnknownStartingColumn;
        }
        else
            return chineseNumbers.Contains(secondCharacter) ? ChineseNumberParser.Parse(secondCharacter).ConvertToColumnBasedOnSide(Side.Red) : ParsedMoveObject.UnknownStartingColumn;
    }

    private int ParseFourthCharacter(string notation)
    {
        const int fourthCharacterIndex = 3;
        bool isBlack = notation.Any(char.IsDigit);

        if (isBlack)
        {
            bool successfulParse = int.TryParse(notation[fourthCharacterIndex].ToString(), out int fourthCharacter);

            return successfulParse ? fourthCharacter : ParsedMoveObject.UnknownStartingColumn;
        }
        else
            return chineseNumbers.Contains(notation[fourthCharacterIndex]) ? ChineseNumberParser.Parse(notation[fourthCharacterIndex]) : ParsedMoveObject.UnknownStartingColumn;
    }

    private int ParsePieceOrderIndex(string notation)
    {
        bool isBlack = notation.Any(char.IsDigit);

        if (isBlack)
            return notation[0] == '前' ? 0 : 1;
        else
            return notation[0] == '前' ? 1 : 0;
    }
}
