using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.MoveObject;

public record ParsedMoveObject
{
    // The starting column would be unknown if there are more than one piece on the same column,
    // which the notation would skip the starting column
    public static int UnknownStartingColumn => -1;
    public static int UnknownPieceOrderIndex => -1;
    public ParsedMoveObject() { }

    // For UCCI notation
    public ParsedMoveObject(Coordinate startingPoint, Coordinate destination)
    {
        StartingPosition = startingPoint;
        Destination = destination;
    }

    // For Chinese/English Notation
    public ParsedMoveObject(
        PieceType pieceType, 
        int startingColumn, 
        MoveDirection moveDirection, 
        int foruthCharacter, 
        int pieceOrderIndex = -1)
    {
        PieceType = pieceType;
        StartingColumn = startingColumn;
        MoveDirection = moveDirection;
        FourthCharacter = foruthCharacter;
        PieceOrderIndex = pieceOrderIndex;
    }

    public PieceType PieceType { get; set; }
    public int StartingColumn { get; set; }
    public int FourthCharacter { get; set; }
    public int PieceOrderIndex { get; set; }
    public MoveDirection MoveDirection { get; set; }

    public Coordinate? StartingPosition { get; set; }
    public Coordinate? Destination { get; set; }
    public bool IsFromUcciNotation => StartingPosition is not null && Destination is not null;
}
