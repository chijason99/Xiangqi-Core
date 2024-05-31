namespace XiangqiCore.Move;
public record ParsedMoveObject
{
    // The starting column would be unknown if there are more than one piece on the same column,
    // which the notation would skip the starting column
    public static int UnknownStartingColumn => -1;
    public ParsedMoveObject() { }
    public ParsedMoveObject(Type pieceType, int startingColumn, MoveDirection moveDirection, int foruthCharacter, int pieceOrderIndex = 0)
    {
        PieceType = pieceType;
        StartingColumn = startingColumn;
        MoveDirection = moveDirection;
        ForuthCharacter = foruthCharacter;
        PieceOrderIndex = pieceOrderIndex;
    }

    public Type PieceType { get; set; }
    public int StartingColumn { get; set; }
    public int ForuthCharacter {  get; set; }
    public int PieceOrderIndex { get; set; }

    public MoveDirection MoveDirection { get; set; }
}
