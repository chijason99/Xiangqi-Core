namespace XiangqiCore.Pieces;
public readonly struct Coordinate()
{
    private const int _minRow = 1;
    private const int _minColumn = 1;
    private const int _maxRow = 10;
    private const int _maxColumn = 9;

    public Coordinate(int column, int row) : this()
    {
        if (row < _minRow || row > _maxRow)
            throw new ArgumentOutOfRangeException(nameof(row), $"Row must be between {_minRow} and {_maxRow}.");

        if (column < _minColumn || column > _maxColumn)
            throw new ArgumentOutOfRangeException(nameof(column), $"Column must be between {_minColumn} and {_maxColumn}.");

        Row = row;
        Column = column;
    }

    public int Column { get; init; }
    public int Row { get; init; }

    public static Coordinate Empty => new (-1, -1);
}
