using XiangqiCore.Extension;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move;
public class EnglishNotationParser : MoveNotationBase
{
    private static char[] pieceEnglishNames => ['k', 'r', 'h', 'c', 'a', 'e', 'p'];
    private static char[] pieceOrderIndexSymbol => ['+', '-'];
    public EnglishNotationParser() { }

    public Type ParsePieceType(string notation)
    {
        char pieceNameToCheck = pieceEnglishNames.Contains(char.ToLower(notation[0])) ? notation[0] : notation[1];

        return char.ToLower(pieceNameToCheck) switch
        {
            'k' => typeof(King),
            'r' => typeof(Rook),
            'h' => typeof(Knight),
            'c' => typeof(Cannon),
            'a' => typeof(Advisor),
            'e' => typeof(Bishop),
            'p' => typeof(Pawn),
            _ => typeof(Pawn)
        };
    }

    // Write a function that could parse the starting column of the move notation
    public int ParseStartingColumn(string notation)
    {
        const int defaultColumnIndex = 1;
        char secondCharacter = notation[defaultColumnIndex];
        bool isBlack = notation.Any(char.IsLower);

        bool successfulParse = int.TryParse(secondCharacter.ToString(), out int startingColumn);
        
        if (successfulParse)
            return isBlack ? startingColumn.ConvertToColumnBasedOnSide(Side.Black) : startingColumn.ConvertToColumnBasedOnSide(Side.Red);
        else
            return ParsedMoveObject.UnknownStartingColumn;
    }

    public MoveDirection ParseMoveDirection(string notation)
    {
        const int defaultMoveActionIndex = 2;
        char thirdCharacter = notation[defaultMoveActionIndex];

        return thirdCharacter switch
        {
            '+' => MoveDirection.Forward,
            '-' => MoveDirection.Backward,
            '=' => MoveDirection.Horizontal,
            _ => throw new ArgumentException("Invalid Move Direction")
        };
    }

    public int GetFourthCharacter(string notation) => int.Parse(notation.Last().ToString());

    public int GetPieceOrderIndex(string notation)
    {
        const int pieceOrderCharacterIndex = 0;
        const int defaultPieceOrderIndex = 0;

        char pieceOrderCharacter = notation[pieceOrderCharacterIndex];
        bool isBlack = notation.Any(char.IsLower);

        if (char.IsDigit(pieceOrderCharacter))
            return int.Parse(pieceOrderCharacter.ToString());
        else if (pieceOrderIndexSymbol.Contains(pieceOrderCharacter))
        {
            if (isBlack)
                return pieceOrderCharacter == '+' ? 0 : 1;

            return pieceOrderCharacter == '+' ? 1 : 0;
        }
        else
            return defaultPieceOrderIndex;
    }

    public override ParsedMoveObject Parse(string notation)
    {
        Type pieceType = ParsePieceType(notation);
        int startingColumn = ParseStartingColumn(notation);
        MoveDirection moveDirection = ParseMoveDirection(notation);
        int fourthCharacter = GetFourthCharacter(notation);
        int pieceOrderIndex = GetPieceOrderIndex(notation);

        return new ParsedMoveObject(pieceType, startingColumn, moveDirection, fourthCharacter, pieceOrderIndex);
    }
}