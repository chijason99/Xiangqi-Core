namespace XiangqiCore;
public readonly struct Coordinate()
{
    private const int _minRow = 1;
    private const int _minColumn = 1;
    private const int _maxRow = 10;
    private const int _maxColumn = 9;
    private const int _emptyCoordinateValue = -1;

    public Coordinate(int column, int row) : this()
    {
        bool isEmptyCoordinate = column == _emptyCoordinateValue && row == _emptyCoordinateValue;
        bool isInvalidRow = row < _minRow || row > _maxRow;
        bool isInvalidColumn = column < _minColumn || column > _maxColumn;

        if (isInvalidRow && !isEmptyCoordinate)
            throw new ArgumentOutOfRangeException(nameof(row), $"Row must be between {_minRow} and {_maxRow}.");

        if (isInvalidColumn && !isEmptyCoordinate)
            throw new ArgumentOutOfRangeException(nameof(column), $"Column must be between {_minColumn} and {_maxColumn}.");

        Row = row;
        Column = column;
    }

    public int Column { get; init; }
    public int Row { get; init; }

    public static Coordinate Empty => new(-1, -1);
}
