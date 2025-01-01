using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.NotationParser;

public class EnglishNotationParser : MoveNotationParserBase
{
    public EnglishNotationParser() : base(Language.English) { }

    public int ParseStartingColumn(string notation)
    {
        const int defaultColumnIndex = 1;
        char secondCharacter = notation[defaultColumnIndex];
        bool isBlack = notation.Any(char.IsLower);

        return int.TryParse(secondCharacter.ToString(), out int startingColumn) ?
                startingColumn :
                ParsedMoveObject.UnknownStartingColumn;
    }

    public int GetFourthCharacter(string notation) => int.Parse(notation.Last().ToString());

    public override ParsedMoveObject Parse(string notation)
    {
        PieceType pieceType = ParsePieceType(notation);
        int startingColumn = ParseStartingColumn(notation);
        MoveDirection moveDirection = ParseMoveDirection(notation);
        int fourthCharacter = GetFourthCharacter(notation);
        PieceOrder pieceOrder = ParsePieceOrder(notation);

        ParsedMoveObject parsedMoveObject = new(pieceType, startingColumn, moveDirection, fourthCharacter, pieceOrder);

        return IsMultiColumnPawn(notation) ? 
            new MultiColumnPawnParsedMoveObject(parsedMoveObject, GetMinNumberOfPawnsOnColumn(notation)) :
            parsedMoveObject;
    }

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