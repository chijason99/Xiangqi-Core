using XiangqiCore.Misc;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces.PieceTypes;
public sealed class EmptyPiece : Piece
{
    public EmptyPiece() : base(Coordinate.Empty, Side.None){ }

    public override IValidationStrategy ValidationStrategy => throw new NotImplementedException();
    public override PieceType PieceType => PieceType.None;

    public override char FenCharacter => '1';
}
