using XiangqiCore.Attributes;

namespace XiangqiCore.Pieces.PieceTypes;
public enum PieceType
{
    King,
    Rook,
    Knight,
    Cannon,
    Advisor,
    Bishop,
    Pawn,
    [IgnoreFromRandomPick]
    None
}
