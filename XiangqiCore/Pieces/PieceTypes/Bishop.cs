using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public sealed class Bishop : Piece
{
    public Bishop(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new BishopValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }

    public override PieceType GetPieceType => PieceType.Bishop;
}
