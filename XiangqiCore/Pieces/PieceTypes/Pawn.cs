using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public sealed class Pawn : Piece
{
    public Pawn(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new PawnValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }

    public override PieceType GetPieceType => PieceType.Pawn;

    public override char FenCharacter => 'p';
}
