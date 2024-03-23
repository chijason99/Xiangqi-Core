using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public abstract class Piece(Coordinate coordinate, Side side)
{
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        Piece other = (Piece)obj;

        return Coordinate.Equals(other.Coordinate) && Side == other.Side;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Coordinate, Side);
    }

    public Coordinate Coordinate { get; private set; } = coordinate;
    public Side Side { get; init; } = side;
    public abstract IValidationStrategy ValidationStrategy { get; }
    public virtual int[] GetAvailableRows() => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    public virtual int[] GetAvailableColumns() => [1, 2, 3, 4, 5, 6, 7, 8, 9];
    public abstract PieceType GetPieceType { get; }
}
