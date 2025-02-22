﻿using XiangqiCore.Attributes;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.PieceTypes;

public enum PieceType
{
    [Symbol(Language.TraditionalChinese, "帥", "將")]
	[Symbol(Language.English, "K", "k")]
	[Symbol(Language.SimplifiedChinese, "帅", "将")]
    King,

	[Symbol(Language.TraditionalChinese, ["車", "俥"])]
	[Symbol(Language.SimplifiedChinese, "车")]
	[Symbol(Language.English, "R", "r")]
	Rook,

	[Symbol(Language.TraditionalChinese, ["馬", "傌"])]
	[Symbol(Language.SimplifiedChinese, ["马"])]
	[Symbol(Language.English, ["H", "N"], ["h", "n"])]
	Knight,

	[Symbol(Language.TraditionalChinese, ["炮", "砲"])]
	[Symbol(Language.English, "C", "c")]
	Cannon,

	[Symbol(Language.TraditionalChinese, "仕", "士")]
	[Symbol(Language.English, "A", "a")]
	Advisor,

	[Symbol(Language.TraditionalChinese, "相", "象")]
	[Symbol(Language.English, ["E", "B"], ["e", "b"])]
	Bishop,

	[Symbol(Language.TraditionalChinese, "兵", "卒")]
	[Symbol(Language.English, "P", "p")]
	Pawn,

    [IgnoreFromRandomPick]
    None
}
