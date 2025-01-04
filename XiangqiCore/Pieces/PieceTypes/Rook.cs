using XiangqiCore.Attributes;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;

public sealed class Rook : Piece
{
    public Rook(Coordinate coordinate, Side side)
        : base(coordinate, side)
    {
        ValidationStrategy = new RookValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }
    public override PieceType PieceType => PieceType.Rook;
    public override char FenCharacter => 'r';
}
