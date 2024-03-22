using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public sealed class Cannon : Piece
{
    public Cannon(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new CannonValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }
}
