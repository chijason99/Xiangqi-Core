namespace XiangqiCore.Move.MoveObject;

public record MultiColumnPawnParsedMoveObject : ParsedMoveObject
{
    public MultiColumnPawnParsedMoveObject(ParsedMoveObject parsedMoveObject, int minNumberOfPawnsOnColumn)
        : base(parsedMoveObject.PieceType,
               parsedMoveObject.StartingColumn,
               parsedMoveObject.MoveDirection,
               parsedMoveObject.FourthCharacter,
               parsedMoveObject.PieceOrder)
    {
        MinNumberOfPawnsOnColumn = minNumberOfPawnsOnColumn;
    }

    public static int LastPawnIndex => -1;

    public int MinNumberOfPawnsOnColumn { get; set; }
    public int[] PossiblePawnColumnsToMakeTheMove { get; set; } = [];
}
