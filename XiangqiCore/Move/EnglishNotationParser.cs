using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move;
public class EnglishNotationParser
{
    private static char[] pieceEnglishNames => ['k', 'r', 'h', 'c', 'a', 'b', 'p'];

    public static Type ParsePieceTypeFromEnglishNotation(string notation)
    {
        char pieceNameToCheck = pieceEnglishNames.Contains(notation[0]) ? notation[0] : notation[1];

        return char.ToLower(pieceNameToCheck) switch
        {
            'k' => typeof(King),
            'r' => typeof(Rook),
            'h' => typeof(Knight),
            'c' => typeof(Cannon),
            'a' => typeof(Advisor),
            'b' => typeof(Bishop),
            'p' => typeof(Pawn),
            _ => typeof(Pawn)
        };
    }
}