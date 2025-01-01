using XiangqiCore.Attributes;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.PieceTypes;

public enum PieceType
{
    [Symbol(Language.TraditionalChinese, "帥", "將")]
	[Symbol(Language.English, "K", "k")]
	[Symbol(Language.SimplifiedChinese, "帅", "将")]
    King,

	[Symbol(Language.TraditionalChinese, "車")]
	[Symbol(Language.SimplifiedChinese, "车")]
	[Symbol(Language.English, "R", "r")]
	Rook,

	[Symbol(Language.TraditionalChinese, "馬")]
	[Symbol(Language.SimplifiedChinese, "马")]
	[Symbol(Language.English, "N", "n")]
	[MoveInDiagonals]
	Knight,

	[Symbol(Language.TraditionalChinese, "炮")]
	[Symbol(Language.English, "C", "c")]
	Cannon,

	[Symbol(Language.TraditionalChinese, "士")]
	[Symbol(Language.English, "A", "a")]
	[MoveInDiagonals]
	Advisor,

	[Symbol(Language.TraditionalChinese, "相", "象")]
	[Symbol(Language.English, "E", "e")]
	[MoveInDiagonals]
	Bishop,

	[Symbol(Language.TraditionalChinese, "兵", "卒")]
	[Symbol(Language.English, "P", "p")]
	Pawn,

    [IgnoreFromRandomPick]
    None
}
