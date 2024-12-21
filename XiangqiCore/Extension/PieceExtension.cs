using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using XiangqiCore.Boards;
using XiangqiCore.Misc;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Extension;
/// <summary>
/// Provides extension methods for the Piece class.
/// </summary>
public static class PieceExtension
{
    /// <summary>
    /// Gets the piece at the specified position.
    /// </summary>
    /// <param name="position">The position array.</param>
    /// <param name="targetCoordinate">The target coordinate.</param>
    /// <returns>The piece at the specified position.</returns>
    public static Piece GetPieceAtPosition(this Piece[,] position, Coordinate targetCoordinate)
    {
        int rowInPositionArray = targetCoordinate.Row - 1;
        int columnInPositionArray = targetCoordinate.Column - 1;

        return position[rowInPositionArray, columnInPositionArray];
    }

    /// <summary>
    /// Checks if there is a piece at the specified position.
    /// </summary>
    /// <param name="position">The position array.</param>
    /// <param name="coordinateToCheck">The coordinate to check.</param>
    /// <returns>True if there is a piece at the specified position, otherwise false.</returns>
    public static bool HasPieceAtPosition(this Piece[,] position, Coordinate coordinateToCheck)
        => coordinateToCheck.Equals(Coordinate.Empty) || position.GetPieceAtPosition(coordinateToCheck) is not EmptyPiece;

    /// <summary>
    /// Gets the pieces on the specified row.
    /// </summary>
    /// <param name="position">The position array.</param>
    /// <param name="row">The row number.</param>
    /// <returns>The pieces on the specified row.</returns>
    public static IEnumerable<Piece> GetPiecesOnRow(this Piece[,] position, int row)
        => position
            .Cast<Piece>()
            .Where(x => x.Coordinate.Row == row && x.PieceType != PieceType.None)
            .OrderBy(x => x.Coordinate.Column);

    /// <summary>
    /// Gets the pieces on the specified column.
    /// </summary>
    /// <param name="position">The position array.</param>
    /// <param name="column">The column number.</param>
    /// <returns>The pieces on the specified column.</returns>
    public static IEnumerable<Piece> GetPiecesOnColumn(this Piece[,] position, int column)
        => position
            .Cast<Piece>()
            .Where(p => p.Coordinate.Column == column && p.PieceType != PieceType.None)
            .OrderBy(p => p.Coordinate.Row);

    /// <summary>
    /// Counts the number of pieces between two positions on the same row.
    /// </summary>
    /// <param name="position">The position array.</param>
    /// <param name="startingPosition">The starting position.</param>
    /// <param name="destination">The destination position.</param>
    /// <returns>The number of pieces between the two positions on the same row.</returns>
    /// <exception cref="ArgumentException">Thrown when the two coordinates are not on the same row or the starting and ending column are the same.</exception>
    public static int CountPiecesBetweenOnRow(this Piece[,] position, Coordinate startingPosition, Coordinate destination)
    {
        if (startingPosition.Row != destination.Row)
            throw new ArgumentException("The two coordinates need to be on the same row");

        if (startingPosition.Column == destination.Column)
            throw new ArgumentException("The starting and ending column cannot be the same");

        IEnumerable<Piece> piecesOnRow = position.GetPiecesOnRow(startingPosition.Row);

        return startingPosition.Column < destination.Column ?
            piecesOnRow.Count(piece => piece.Coordinate.Column > startingPosition.Column && piece.Coordinate.Column < destination.Column) :
            piecesOnRow.Count(piece => piece.Coordinate.Column < startingPosition.Column && piece.Coordinate.Column > destination.Column);
    }

    /// <summary>
    /// Counts the number of pieces between two positions on the same column.
    /// </summary>
    /// <param name="position">The position array.</param>
    /// <param name="startingPosition">The starting position.</param>
    /// <param name="destination">The destination position.</param>
    /// <returns>The number of pieces between the two positions on the same column.</returns>
    /// <exception cref="ArgumentException">Thrown when the two coordinates are not on the same column or the starting and ending row are the same.</exception>
    public static int CountPiecesBetweenOnColumn(this Piece[,] position, Coordinate startingPosition, Coordinate destination)
    {
        if (startingPosition.Column != destination.Column)
            throw new ArgumentException("The two coordinates need to be on the same column");

        if (startingPosition.Row == destination.Row)
            throw new ArgumentException("The starting and ending column cannot be the same");

        IEnumerable<Piece> piecesOnColumn = position.GetPiecesOnColumn(startingPosition.Column);

        return startingPosition.Row < destination.Row ?
            piecesOnColumn.Count(piece => piece.Coordinate.Row > startingPosition.Row && piece.Coordinate.Row < destination.Row) :
			piecesOnColumn.Count(piece => piece.Coordinate.Row < startingPosition.Row && piece.Coordinate.Row > destination.Row);
	}

    /// <summary>
    /// Checks if the destination position contains a friendly piece.
    /// </summary>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="startingPosition">The starting position.</param>
    /// <param name="destination">The destination position.</param>
    /// <returns>True if the destination position contains a friendly piece, otherwise false.</returns>
    public static bool IsDestinationContainingFriendlyPiece(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        Side side = boardPosition.GetPieceAtPosition(startingPosition).Side;

        return boardPosition.HasPieceAtPosition(destination) && boardPosition.GetPieceAtPosition(destination).Side == side;
    }

    /// <summary>
    /// Sets the piece at the specified position on the board.
    /// </summary>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="targetCoordinates">The target coordinates.</param>
    /// <param name="targetPiece">The piece to set at the position.</param>
    public static void SetPieceAtPosition(this Piece[,] boardPosition, Coordinate targetCoordinates, Piece targetPiece)
    {
        int row = targetCoordinates.Row - 1;
        int column = targetCoordinates.Column - 1;

        boardPosition[row, column] = targetPiece;
    }

    /// <summary>
    /// Determines if making a move will expose the king to danger.
    /// </summary>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="startingPosition">The starting position.</param>
    /// <param name="destination">The destination position.</param>
    /// <returns>True if making the move will expose the king to danger, otherwise false.</returns>
    /// <exception cref="ArgumentException">Thrown when the starting position does not contain a piece.</exception>
    public static bool WillExposeKingToDanger(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        if (!boardPosition.HasPieceAtPosition(startingPosition))
            throw new ArgumentException("The starting position must contain a piece");

        Piece pieceToMove = boardPosition.GetPieceAtPosition(startingPosition);
        Side targetKingSide = pieceToMove.Side;
        Piece[,] positionAfterSimulation = boardPosition.SimulateMove(startingPosition, destination);
        King targetKing = positionAfterSimulation.GetPiecesOfType<King>(targetKingSide).Single();

        return positionAfterSimulation.IsKingInCheck(targetKingSide) || 
               positionAfterSimulation.IsKingExposedDirectlyToEnemyKing(targetKing.Coordinate);
    }

    /// <summary>
    /// Gets the pieces of the specified type on the board for the specified side.
    /// </summary>
    /// <typeparam name="TPieceType">The type of the pieces to get.</typeparam>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="side">The side of the pieces.</param>
    /// <returns>The pieces of the specified type on the board for the specified side.</returns>
    public static IEnumerable<TPieceType> GetPiecesOfType<TPieceType>(this Piece[,] boardPosition, Side side) where TPieceType : Piece
        => boardPosition
            .OfType<TPieceType>()
            .Where(piece => piece.Side == side);


	/// <summary>
	/// Gets the pieces of the specified type on the board for the specified side.
	/// </summary>
	/// <param name="boardPosition">The board position array.</param>
	/// <param name="pieceType">The type of the pieces to get.</param>
	/// <param name="side">The side of the pieces. If null, pieces of the specified type from all sides are returned.</param>
	/// <returns>The pieces of the specified type on the board for the specified side.</returns>
	public static IEnumerable<Piece> GetPiecesOfType(this Piece[,] boardPosition, PieceType pieceType, Side? side = null)
		=> boardPosition
			.Cast<Piece>()
			.Where(piece => piece.PieceType == pieceType)
			.WhereIf(side is not null, p => p.Side == side.Value);

	/// <summary>
	/// Checks if the king is attacked by a piece of the specified type.
	/// </summary>
	/// <typeparam name="TPieceType">The type of the attacking piece.</typeparam>
	/// <param name="boardPosition">The board position array.</param>
	/// <param name="kingCoordinate">The coordinate of the king.</param>
	/// <returns>True if the king is attacked by a piece of the specified type, otherwise false.</returns>
	public static bool IsKingAttackedBy<TPieceType>(this Piece[,] boardPosition, Coordinate kingCoordinate) where TPieceType : Piece
    {
        King kingToCheck = (King)boardPosition.GetPieceAtPosition(kingCoordinate);
        Side enemySide = kingToCheck.Side.GetOppositeSide();
        IEnumerable<TPieceType> enemyPieces = boardPosition.GetPiecesOfType<TPieceType>(enemySide);

        if (!enemyPieces.Any())
            return false;

        foreach (TPieceType enemyPiece in enemyPieces)
        {
            if (enemyPiece.ValidationStrategy.ValidateMoveLogicForPiece(boardPosition, enemyPiece.Coordinate, kingCoordinate))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the king is directly exposed to the enemy king.
    /// </summary>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="kingCoordinate">The coordinate of the king.</param>
    /// <returns>True if the king is directly exposed to the enemy king, otherwise false.</returns>
    public static bool IsKingExposedDirectlyToEnemyKing(this Piece[,] boardPosition, Coordinate kingCoordinate)
    {
        King targetKing = (King)boardPosition.GetPieceAtPosition(kingCoordinate);
        King opponentKing = boardPosition
            .GetPiecesOfType<King>(targetKing.Side.GetOppositeSide())
            .Single();

        if (targetKing.Coordinate.Column != opponentKing.Coordinate.Column)
            return false;

        return boardPosition.CountPiecesBetweenOnColumn(kingCoordinate, opponentKing.Coordinate) == 0;
    }

    public static bool IsKingExposedDirectlyToEnemyKing(this Piece[,] boardPosition)
    {
		King redKing = boardPosition.GetPiecesOfType<King>(Side.Red).Single();
		King blackKing = boardPosition.GetPiecesOfType<King>(Side.Black).Single();

		if (redKing.Coordinate.Column != blackKing.Coordinate.Column)
			return false;

		return boardPosition.CountPiecesBetweenOnColumn(redKing.Coordinate, blackKing.Coordinate) == 0;
	}

    /// <summary>
    /// Simulates a move on the board position array.
    /// </summary>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="startingPosition">The starting position.</param>
    /// <param name="destination">The destination position.</param>
    /// <returns>The board position array after the move is simulated.</returns>
    /// <exception cref="ArgumentException">Thrown when there is no piece at the starting position on the board.</exception>
    public static Piece[,] SimulateMove(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        if (!boardPosition.HasPieceAtPosition(startingPosition))
            throw new ArgumentException("There must be a piece at the starting position on the board");

        //Piece[,] boardPositionClone = boardPosition.DeepClone();

        Piece[,] boardPositionClone = (Piece[,])boardPosition.Clone();
        boardPositionClone.MakeMove(startingPosition, destination);

        return boardPositionClone;
    }

    /// <summary>
    /// Makes a move on the board position array.
    /// </summary>
    /// <param name="boardPosition">The board position array.</param>
    /// <param name="startingPosition">The starting position.</param>
    /// <param name="destination">The destination position.</param>
    public static void MakeMove(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        Piece pieceAtStartingPosition = boardPosition.GetPieceAtPosition(startingPosition);

        Piece movedPiece = PieceFactory.Create(pieceAtStartingPosition.PieceType, pieceAtStartingPosition.Side, destination);
        Piece emptyPiece = PieceFactory.CreateEmptyPiece();

        boardPosition.SetPieceAtPosition(startingPosition, emptyPiece);
        boardPosition.SetPieceAtPosition(destination, movedPiece);
    }

    /// <summary>
    /// A method to order the pieces correct according to the side of move, such that the parsed piece order index is actually grabbing the correct pawn
    /// </summary>
    /// <param name="pieces"></param>
    /// <param name="sideToMove"></param>
    /// <returns></returns>
    public static IEnumerable<Piece> OrderByRowWithSide(this IEnumerable<Piece> pieces, Side sideToMove)
        => sideToMove == Side.Black ? pieces.OrderBy(p => p.Coordinate.Row) : pieces.OrderByDescending(p => p.Coordinate.Row);

    public static bool IsKingInCheck(this Piece[,] position, Side sideToCheck)
    {
        King targetKing = position.GetPiecesOfType<King>(sideToCheck).Single();

        return position.IsKingAttackedBy<Rook>(targetKing.Coordinate) ||
               position.IsKingAttackedBy<Cannon>(targetKing.Coordinate) ||
               position.IsKingAttackedBy<Pawn>(targetKing.Coordinate) ||
               position.IsKingAttackedBy<Knight>(targetKing.Coordinate);
    }

	public static bool IsSideInCheckmate(this Piece[,] position, Side sideToCheck)
	{
		foreach (Piece piece in GetPiecesToCheck(position, sideToCheck))
		{
			foreach (Coordinate potentatialCoordinate in piece.GeneratePotentialMoves(position))
				return false;
		}

		return true;
	}

	private static IEnumerable<Piece> GetPiecesToCheck(Piece[,] position, Side sideToCheck)
    {
        var piecesToCheck = position
                            .Cast<Piece>()
                            .Where(p => p.Side == sideToCheck);

        foreach (Piece pieceToCheck in piecesToCheck)
            yield return pieceToCheck;
	}

    public static bool HasDuplicatePieceOnColumn(this Piece[,] position, int column, PieceType pieceType, Side side)
		=> position.GetPiecesOnColumn(column).Count(x => x.PieceType == pieceType && x.Side == side) > 1;

    public static Piece[,] DeepClone(this Piece[,] position)
    {
        Piece[,] deepClonedPosition = new Piece[10, 9];

		for (int row = 0; row < 10; row++)
		{
			for (int column = 0; column < 9; column++)
			{
                Piece originalPiece = position[row, column];

                Piece clonedPiece = originalPiece.PieceType != PieceType.None ? 
                    PieceFactory.Create(
                    originalPiece.PieceType, 
                    originalPiece.Side, 
                    new Coordinate(column + 1, row + 1)) :
                EmptyPiece.Instance;

				deepClonedPosition[row, column] = clonedPiece;
			}
		}

        return deepClonedPosition;
	}

    public static byte[] GenerateBoardImage(
        this Piece[,] position, 
        bool flipHorizontal = false, 
        bool flipVertical = false)
	{
		const int defaultSquareSize = 50;
		const int defaultBoardHeight = 500;
		const int defaultBoardWidth = 450;

		const int columns = 9;
		const int rows = 10;

		using Image<Rgba32> boardImage = ImageCache.GetImage(Board.GetImageResourcePath());
        boardImage.Mutate(x => x.Resize(defaultBoardWidth, defaultBoardHeight));

        foreach (Piece piece in position.Cast<Piece>().Where(p => p is not EmptyPiece))
        {
            string pieceResourcePath = piece.GetImageResourcePath();
			using Image<Rgba32> pieceImage = ImageCache.GetImage(pieceResourcePath);

			int xCoordinate = (piece.Coordinate.Column - 1);
            int yCoordinate = rows - piece.Coordinate.Row;

			// If flipping the board horizontally, both the x-coordinate and y-coordinate should be flipped
			// If flipping the board vertically, then the x-coordinate should be flipped
			// If flipping the board vertically and horizontally, then only the y-coordinate should be flipped because the x-coordinate is flipped twice
			if (flipVertical && flipHorizontal)
				yCoordinate = piece.Coordinate.Row - 1;
            else if (flipVertical)
                xCoordinate = columns - piece.Coordinate.Column;
            else if (flipHorizontal)
            {
                yCoordinate = piece.Coordinate.Row - 1;
                xCoordinate = columns - piece.Coordinate.Column;
			}

			boardImage.Mutate(ctx => ctx.DrawImage(pieceImage, 
                new Point(xCoordinate * defaultSquareSize, yCoordinate * defaultSquareSize), 1f));
		}
        
        using MemoryStream memoryStream = new();
		boardImage.Save(memoryStream, new PngEncoder());

		return memoryStream.ToArray();
	}
}
