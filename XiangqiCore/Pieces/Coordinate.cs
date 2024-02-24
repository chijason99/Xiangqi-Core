namespace XiangqiCore.Pieces;
public readonly struct Coordinate(int column, int row)
{
    public int Column { get; init; } = column;
    public int Row { get; init; } = row;
}
