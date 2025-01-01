using XiangqiCore.Attributes;
using XiangqiCore.Misc;

namespace XiangqiCore.Move;

public enum PieceOrder
{
	/// <summary>
	/// The piece order is unknown
	/// </summary>
	Unknown = -1,

	/// <summary>
	/// The piece at the back when there are multiple pieces of the same type and side in the same column (i.e. equilvalent to "後" or "-" in notation)
	/// </summary>
	[Symbol(Language.TraditionalChinese, "後")]
	[Symbol(Language.SimplifiedChinese, "后")]
	[Symbol(Language.English, "-")]
	Last = 0,

	/// <summary>
	/// The piece in front when there are multiple pieces of the same type and side in the same column (i.e. equilvalent to "前" or "+" in notation)
	/// Also used when there is only one piece of that type in the column
	/// </summary>
	[Symbol(Language.TraditionalChinese, "前")]
	[Symbol(Language.English, ["1", "+"])]
	First = 1,

	/// <summary>
	/// The order of the pawn in the middle when there are 3 pawns in the same column
	/// </summary>
	[Symbol(Language.TraditionalChinese, ["中", "二"])]
	[Symbol(Language.English, "2")]
	Second = 2,

	[Symbol(Language.TraditionalChinese, "三")]
	[Symbol(Language.English, "3")]
	Third = 3,

	[Symbol(Language.TraditionalChinese, "四")]
	[Symbol(Language.English, "4")]
	Fourth = 4,
}
