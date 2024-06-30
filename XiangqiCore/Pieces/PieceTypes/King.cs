using XiangqiCore.Attributes;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces.PieceTypes;

[MoveInOrthogonals]
public sealed class King : Piece
{
    public King(Coordinate coordinate, Side side)
        : base(coordinate, side)
    {
        ValidationStrategy = new KingValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }

    public override PieceType PieceType => PieceType.King;
    public override char FenCharacter => 'k';
}
