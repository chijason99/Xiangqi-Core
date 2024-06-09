namespace XiangqiCore.Move;
public record MultiColumnPawnParsedMoveObject : ParsedMoveObject
{
    public MultiColumnPawnParsedMoveObject(ParsedMoveObject parsedMoveObject, int minNumberOfPawnsOnColumn) 
        : base(parsedMoveObject.PieceType, 
               parsedMoveObject.StartingColumn, 
               parsedMoveObject.MoveDirection, 
               parsedMoveObject.FourthCharacter, 
               parsedMoveObject.PieceOrderIndex)
    {
        MinNumberOfPawnsOnColumn = minNumberOfPawnsOnColumn;
    }

    public static int FrontPawnIndex => -1;

    public int MinNumberOfPawnsOnColumn { get; set; }
    public int[] PossiblePawnColumnsToMakeTheMove { get; set; } = [];
}
