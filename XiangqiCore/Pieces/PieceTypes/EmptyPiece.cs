using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces.PieceTypes;
public class EmptyPiece : Piece
{
    public EmptyPiece() : base(Coordinate.Empty, Side.None){ }

    public override IValidationStrategy ValidationStrategy => throw new NotImplementedException();
}
