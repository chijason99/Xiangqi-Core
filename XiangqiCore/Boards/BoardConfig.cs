using XiangqiCore.Misc;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Boards;

/// <summary>
/// Represents the configuration of a game board.
/// </summary>
public class BoardConfig
{
	/// <summary>
	/// Gets the dictionary of pieces to add to the board.
	/// </summary>
	public Dictionary<Coordinate, Piece> PiecesToAdd { get; } = new Dictionary<Coordinate, Piece>();

	/// <summary>
	/// Adds a piece of the specified type and color to the specified coordinate.
	/// </summary>
	/// <param name="targetPieceType">The type of the piece to add.</param>
	/// <param name="targetColor">The color of the piece to add.</param>
	/// <param name="targetCoordinate">The coordinate where the piece should be added.</param>
	public void AddPiece(PieceType targetPieceType, Side targetColor, Coordinate targetCoordinate)
	{
		PiecesToAdd[targetCoordinate] = PieceFactory.Create(targetPieceType, targetColor, targetCoordinate);
	}

	/// <summary>
	/// Adds a random piece to the specified coordinate.
	/// </summary>
	/// <param name="targetCoordinate">The coordinate where the piece should be added.</param>
	public void AddRandomPiece(Coordinate targetCoordinate)
	{
		PiecesToAdd[targetCoordinate] = PieceFactory.CreateRandomPiece(targetCoordinate);
	}
}
