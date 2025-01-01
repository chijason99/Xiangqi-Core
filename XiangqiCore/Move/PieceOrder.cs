namespace XiangqiCore.Move;

public enum PieceOrder
{
	/// <summary>
	/// The piece order is unknown
	/// </summary>
	Unknown = -2,

	/// <summary>
	/// Used when there are multiple pawns on the column, and you want to specify the last one with respect to the side 
	/// </summary>
	Last = -1,

	/// <summary>
	/// The piece in front when there are multiple pieces of the same type and side in the same column (i.e. equilvalent to "前" or "+" in notation)
	/// Also used when there is only one piece of that type in the column
	/// </summary>
	First = 0,

	/// <summary>
	/// The piece at the back when there are multiple pieces of the same type and side in the same column (i.e. equilvalent to "後" or "-" in notation)
	/// </summary>
	Second = 1,
	Third = 2,
	Fourth = 3,
	Fifth = 4
}
