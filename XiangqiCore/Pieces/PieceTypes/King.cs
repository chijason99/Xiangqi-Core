using XiangqiCore.Attributes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces.PieceTypes;

[HasAvailableColumns(4,5,6)]
[HasAvailableRows(redRows: [1,2,3], blackRows: [8, 9, 10])]
public sealed class King : Piece
{
    public King(Coordinate coordinate, Side side)
        : base(coordinate, side)
    {
        ValidationStrategy = new KingValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }

    public override int[] GetAvailableColumns()
    {

    }
}
