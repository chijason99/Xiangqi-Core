using XiangqiCore.Extension;
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

	/// <summary>
	/// Get the piece counts of the pieces to add.
	/// </summary>
	/// <returns><see cref="PieceCounts"/></returns>
	public PieceCounts ExtractPieceCounts()
	{
		Dictionary<PieceType, int> redPiecesCount = [];
		Dictionary<PieceType, int> blackPiecesCount = [];

		foreach ((_, Piece piece) in PiecesToAdd)
		{
			PieceType pieceType = piece.PieceType;
			var target = piece.Side == Side.Red ? redPiecesCount : blackPiecesCount;

			if (target.TryGetValue(pieceType, out int value))
				target[pieceType] = ++value;
			else
				target[pieceType] = 1;
		}
		
		return new PieceCounts(redPiecesCount, blackPiecesCount);
	}

	public void RandomisePiecePositions(PieceCounts pieceCounts, bool allowCheck = false)
	{
		Random random = new();
		HashSet<Coordinate> occupiedCoordinates = [];

		if (!pieceCounts.BlackPieces.ContainsKey(PieceType.King) || !pieceCounts.RedPieces.ContainsKey(PieceType.King))
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
			shouldRetry = board.Position.IsKingInCheck(Side.Red) || board.Position.IsKingInCheck(Side.Black);
		}

		bool ShouldRetry(Piece[,] position)
		{
			Coordinate redKingCoordinate = position.GetPiecesOfType<King>(Side.Red).Single().Coordinate;

			if (position.IsKingExposedDirectlyToEnemyKing(redKingCoordinate))
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
			var pieceCountsForSide = side == Side.Red ? pieceCounts.RedPieces : pieceCounts.BlackPieces;

			foreach ((PieceType pieceType, int count) in pieceCountsForSide)
			{
				for (int i = 0; i < count; i++)
				{
					Coordinate randomCoordinate = GetRandomCoordinateForPiece(pieceType, side);

					while (occupiedCoordinates.Contains(randomCoordinate))
						randomCoordinate = GetRandomCoordinateForPiece(pieceType, side);

					occupiedCoordinates.Add(randomCoordinate);
					AddPiece(pieceType, side, randomCoordinate);
				}
			}
		}
	}
}
