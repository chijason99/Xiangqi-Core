using XiangqiCore.Move.MoveObject;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.NotationParser;
public class EnglishNotationParser : MoveNotationBase
{
    private static char[] pieceEnglishNames => ['k', 'r', 'h', 'c', 'a', 'e', 'p'];
    private static char[] pieceOrderIndexSymbol => ['+', '-'];
    private static char[] pawnsInEnglish => ['p', 'P'];
    public EnglishNotationParser() { }

    public PieceType ParsePieceType(string notation)
    {
        char pieceNameToCheck = pieceEnglishNames.Contains(char.ToLower(notation[0])) ? notation[0] : notation[1];

        return char.ToLower(pieceNameToCheck) switch
        {
            'k' => PieceType.King,
            'r' => PieceType.Rook,
            'h' => PieceType.Knight,
            'c' => PieceType.Cannon,
            'a' => PieceType.Advisor,
            'e' => PieceType.Bishop,
            'p' => PieceType.Pawn,
            _ => PieceType.Pawn
        };
    }

    // Write a function that could parse the starting column of the move notation
    public int ParseStartingColumn(string notation)
    {
        const int defaultColumnIndex = 1;
        char secondCharacter = notation[defaultColumnIndex];
        bool isBlack = notation.Any(char.IsLower);

        return int.TryParse(secondCharacter.ToString(), out int startingColumn) ?
                startingColumn :
                ParsedMoveObject.UnknownStartingColumn;

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

        // Multi column pawn scenario
        if (char.IsDigit(pieceOrderCharacter))
            return int.Parse(pieceOrderCharacter.ToString()) - 1;
        else if (pieceOrderIndexSymbol.Contains(pieceOrderCharacter))
            return pieceOrderCharacter == '+' ? 0 : IsMultiColumnPawn(notation) ? MultiColumnPawnParsedMoveObject.LastPawnIndex : 1;
        else
            return defaultPieceOrderIndex;
    }

    public override ParsedMoveObject Parse(string notation)
    {
        PieceType pieceType = ParsePieceType(notation);
        int startingColumn = ParseStartingColumn(notation);
        MoveDirection moveDirection = ParseMoveDirection(notation);
        int fourthCharacter = GetFourthCharacter(notation);
        int pieceOrderIndex = GetPieceOrderIndex(notation);

        ParsedMoveObject parsedMoveObject = new(pieceType, startingColumn, moveDirection, fourthCharacter, pieceOrderIndex);

        return IsMultiColumnPawn(notation) ? new MultiColumnPawnParsedMoveObject(parsedMoveObject, GetMinNumberOfPawnsOnColumn(notation)) :
                                             parsedMoveObject;
    }

    private bool IsMultiColumnPawn(string notation) => ParsePieceType(notation) == PieceType.Pawn && notation.IndexOfAny(pawnsInEnglish) == -1;

    private int GetMinNumberOfPawnsOnColumn(string notation)
    {
        const int defaultMinNumberOfPawnsOnColumn = 2;
        char firstCharacter = notation[0];

        bool successfulParseFirstCharacter = int.TryParse(firstCharacter.ToString(), out int minNumberOfPawnsOnColumn);

        if (!successfulParseFirstCharacter)
            return defaultMinNumberOfPawnsOnColumn;
        else
            return minNumberOfPawnsOnColumn;
    }
}