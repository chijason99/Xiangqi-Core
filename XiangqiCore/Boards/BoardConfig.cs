using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Game;

namespace XiangqiCore.Boards;

/// <summary>
/// Represents the configuration of a game board.
/// </summary>
public class BoardConfig
{
	/// <summary>
	/// Gets the dictionary of pieces to add to the board.
	/// </summary>
	public Dictionary<Coordinate, Piece> PiecesToAdd { get; } = [];

	public PieceCounts PieceCounts { get; private set; } = new PieceCounts(RedPieces: [], BlackPieces: []);

	/// <summary>
	/// Adds a piece of the specified type and color to the specified coordinate.
	/// </summary>
	/// <param name="targetPieceType">The type of the piece to add.</param>
	/// <param name="targetColor">The color of the piece to add.</param>
	/// <param name="targetCoordinate">The coordinate where the piece should be added.</param>
	public void AddPiece(PieceType targetPieceType, Side targetColor, Coordinate targetCoordinate)
	{
		PiecesToAdd[targetCoordinate] = PieceFactory.Create(targetPieceType, targetColor, targetCoordinate);

		PieceCounts.IncrementPieceCount(targetPieceType, targetColor);
	}

	private void AddPieceWithoutIncrementingPieceCount(PieceType targetPieceType, Side targetColor, Coordinate targetCoordinate)
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

		Piece newPiece = PiecesToAdd[targetCoordinate];

		PieceCounts.IncrementPieceCount(newPiece.PieceType, newPiece.Side); 
	}

	/// <summary>
	/// Sets the piece counts for the board configuration. This is used for <see cref="XiangqiBuilder.RandomisePiecePositions"/>, and it will remove all the previous PiecesToAdd in the board config.
	/// </summary>
	/// <param name="pieceCounts">The piece counts to set.</param>
	public void SetPieceCounts(PieceCounts pieceCounts)
	{
		PiecesToAdd.Clear();
		PieceCounts = pieceCounts;
	}

	public void RandomisePiecePositions(bool allowCheck = false)
	{
		Random random = new();
		HashSet<Coordinate> occupiedCoordinates = [];

		if (!PieceCounts.BlackPieces.ContainsKey(PieceType.King) || !PieceCounts.RedPieces.ContainsKey(PieceType.King))
			throw new ArgumentException("Both sides must have a king.");

		AddPiecesForSide(Side.Red);
		AddPiecesForSide(Side.Black);

		// Temperoraily creating a board to validate the position
		Board board = new (this);

		bool shouldRetry = ShouldRetry(board.Position);

		while (shouldRetry)
		{
			occupiedCoordinates.Clear();
			PiecesToAdd.Clear();

			AddPiecesForSide(Side.Red);
			AddPiecesForSide(Side.Black);
			
			board = new(this);
			shouldRetry = ShouldRetry(board.Position);
		}

		bool ShouldRetry(Piece[,] position)
		{
			if (position.IsKingExposedDirectlyToEnemyKing())
				return true;

			bool isRedKingInCheck = position.IsKingInCheck(Side.Red);
			bool isBlackKingInCheck = position.IsKingInCheck(Side.Black);

			if (!allowCheck)
				return isRedKingInCheck || isBlackKingInCheck;

			// If both kings are in check, then the position is invalid
			return isRedKingInCheck && isBlackKingInCheck;
		}

		Coordinate GetRandomCoordinateForPiece(PieceType pieceType, Side side) 
			=> pieceType.GetValidationStrategy().GetRandomCoordinate(random, side);

		void AddPiecesForSide(Side side)
		{
			var pieceCountsForSide = side == Side.Red ? PieceCounts.RedPieces : PieceCounts.BlackPieces;

			foreach ((PieceType pieceType, int count) in pieceCountsForSide)
			{
				for (int i = 0; i < count; i++)
				{
					Coordinate randomCoordinate = GetRandomCoordinateForPiece(pieceType, side);

					while (occupiedCoordinates.Contains(randomCoordinate))
						randomCoordinate = GetRandomCoordinateForPiece(pieceType, side);

					occupiedCoordinates.Add(randomCoordinate);
					AddPieceWithoutIncrementingPieceCount(pieceType, side, randomCoordinate);
				}
			}
		}
	}
}
