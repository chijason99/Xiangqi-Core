namespace XiangqiCore.Misc;
public readonly struct Coordinate()
{
    private const int _minRow = 1;
    private const int _minColumn = 1;
    private const int _maxRow = 10;
    private const int _maxColumn = 9;
    private const int _emptyCoordinateValue = -1;

    private const int _uniCodeOfA = 65;
    private const int _uniCodeOfI = 73;

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

    public static string TranslateToUcciCoordinate(Coordinate coordinate)
	{
		if (coordinate.Equals(Empty))
			return "00";

		char column = TranslateToUcciColumn(coordinate.Column);
		int row = TranslateToUcciRow(coordinate.Row);

		return $"{column}{row}";
	}

	private static int TranslateToUcciRow(int row) => row - 1;

	private static char TranslateToUcciColumn(int column)
	{
		int columnUnicode = column + _uniCodeOfA - 1 + 32; // Adding 32 to convert to lower case
		return Convert.ToChar(columnUnicode);
	}

    public override string ToString() => $"({Column}, {Row})";
}
