using XiangqiCore.Attributes;

namespace XiangqiCore.Pieces.PieceTypes;

public enum PieceType
{
    [ChineseName("帥", "將")]
    King,
	[ChineseName("車")]
	Rook,
	[ChineseName("馬")]
	Knight,
	[ChineseName("炮")]
	Cannon,
	[ChineseName("士")]
	Advisor,
	[ChineseName("相", "象")]
	Bishop,
	[ChineseName("兵", "卒")]
	Pawn,
    [IgnoreFromRandomPick]
    None
}
