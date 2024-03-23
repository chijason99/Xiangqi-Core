using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public sealed class Cannon : Piece
{
    public Cannon(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new CannonValidationStrategy();
    }
    public override PieceType GetPieceType => PieceType.Cannon;
    public override IValidationStrategy ValidationStrategy { get; }
    public override char FenCharacter => 'c';
}
