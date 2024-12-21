using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Boards;

public record PieceCounts(Dictionary<PieceType, int> RedPieces, Dictionary<PieceType, int> BlackPieces)
{
	public void IncrementPieceCount(PieceType pieceType, Side side)
	{
		Dictionary<PieceType, int> pieces = side == Side.Red ? RedPieces : BlackPieces;

		if (pieces.TryGetValue(pieceType, out int count))
			pieces[pieceType] = ++count;
		else
			pieces[pieceType] = 1;
	}

	public int GetPieceCount(PieceType pieceType, Side side)
	{
		return side == Side.Red ? RedPieces.GetValueOrDefault(pieceType, 0) : BlackPieces.GetValueOrDefault(pieceType, 0);
	}
}