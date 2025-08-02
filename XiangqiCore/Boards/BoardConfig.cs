using System.Diagnostics;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Game;
using XiangqiCore.Pieces.ValidationStrategy;

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

	public PieceCounts PieceCounts { get; private set; } = new(RedPieces: [], BlackPieces: []);
	
	/// <summary>
	/// A dictionary that holds user defined constraints for piece placement.
	/// </summary>
	public Dictionary<(PieceType, Side), List<Func<Coordinate, bool>>> PiecePlacementConstraints { get; } = [];

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

	// TODO: make this public?
	// TODO: Also might want a method to pass in Func for filtering constraint
	/// <summary>
	/// This method is used for RandomizePiecePositions, where we want to add a piece onto the board
	/// without incrementing the piece count after clearing the board.
	/// </summary>
	/// <param name="targetPieceType"></param>
	/// <param name="targetColor"></param>
	/// <param name="targetCoordinate"></param>
	public void AddPieceWithoutIncrementingPieceCount(PieceType targetPieceType, Side targetColor, Coordinate targetCoordinate)
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
		if (!PieceCounts.BlackPieces.ContainsKey(PieceType.King) || !PieceCounts.RedPieces.ContainsKey(PieceType.King))
			throw new ArgumentException("Both sides must have a king.");

		const int maxAttempts = 25;
		for (int attempt = 1; attempt <= maxAttempts; attempt++)
		{
			try
			{
				PiecesToAdd.Clear();
				PlaceAllPieces(); // This can throw InvalidOperationException

				var board = new Board(this);

				// If the final position is valid, we are done.
				if (!IsInvalidFinalPosition(board.Position, allowCheck))
					return;
			}
			catch (InvalidOperationException)
			{
				// This catches the placement failure. If it's the last attempt,
				// the exception will be thrown by the check after the loop.
				// Otherwise, we continue to the next attempt.
			}
		}
		
		// If the loop completes without returning, all attempts have failed.
		throw new InvalidOperationException(
			$"Failed to generate a valid random board position after {maxAttempts} attempts. " +
			"The piece counts or placement constraints are likely too restrictive.");
	}
	
	private static bool IsInvalidFinalPosition(Piece[,] position, bool allowCheck = false)
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

	private void PlaceAllPieces()
	{
		var availableCoordinates = Board.GetAllRows()
			.SelectMany(row => Board.GetAllColumns()
				.Select(col => new Coordinate(col, row)))
			.ToList();
		
		AddPiecesForSide(Side.Red, availableCoordinates);
		AddPiecesForSide(Side.Black, availableCoordinates);
	}
	
	private void AddPiecesForSide(Side side, List<Coordinate> availableCoordinates)
	{
		var pieceCountsForSide = side == Side.Red ? PieceCounts.RedPieces : PieceCounts.BlackPieces;

		foreach ((PieceType pieceType, int count) in pieceCountsForSide)
		{
			// Get the valid coordinates for the piece type for placement
			var validPlacementCoordinates = pieceType.GetValidationStrategy()
				.GetValidCoordinates(side);
			
			PiecePlacementConstraints.TryGetValue((pieceType, side), out var constraints);
				
			for (int i = 0; i < count; i++)
			{
				// Intersect the valid placement coordinates with the unoccupied coordinates
				var eligibleCoordinates = validPlacementCoordinates
					.WhereIf(
						constraints?.ElementAtOrDefault(i) is not null,
						coordinate =>
						{
							var constraint = constraints![i];
							
							return constraint(coordinate);
						})
					.Intersect(availableCoordinates)
					.ToList();
				
				// Check if there are any valid spots left for this piece.
				if (eligibleCoordinates.Count == 0)
				{
					// If not, the current randomization attempt has failed because the board
					// is too crowded or the constraints are too restrictive.
					throw new InvalidOperationException(
						$"Could not place piece {i + 1} of type {pieceType} for {side}. " +
						"There are no available squares that satisfy all placement constraints. " +
						"Consider using less restrictive constraints or fewer pieces.");
				}
				
				int randomIndex = Random.Shared.Next(eligibleCoordinates.Count);
				Coordinate targetCoordinate = eligibleCoordinates.ElementAt(randomIndex);

				AddPieceWithoutIncrementingPieceCount(pieceType, side, targetCoordinate);
					
				availableCoordinates.Remove(targetCoordinate);
			}
		}
	}
}
