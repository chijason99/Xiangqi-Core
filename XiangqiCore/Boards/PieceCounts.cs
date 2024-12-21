using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Boards;

public record PieceCounts(Dictionary<PieceType, int> RedPieces, Dictionary<PieceType, int> BlackPieces)
{
	public int GetPieceCount(PieceType pieceType, Side side)
	{
		return side == Side.Red ? RedPieces.GetValueOrDefault(pieceType, 0) : BlackPieces.GetValueOrDefault(pieceType, 0);
	}
}