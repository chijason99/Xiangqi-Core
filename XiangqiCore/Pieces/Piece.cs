using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public abstract class Piece(Coordinate coordinate, Side side)
{
    public Coordinate Coordinate { get; private set; } = coordinate;
    public Side Side { get; init; } = side;
    public abstract IValidationStrategy ValidationStrategy { get; }
    public virtual int[] GetAvailableRows() => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    public virtual int[] GetAvailableColumns() => [1, 2, 3, 4, 5, 6, 7, 8, 9];
}
