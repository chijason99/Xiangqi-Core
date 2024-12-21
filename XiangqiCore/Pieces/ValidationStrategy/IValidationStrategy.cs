using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;

public interface IValidationStrategy
{
    /// <summary>
    /// Check if making this move would break any rules against the move logic of the piece
    /// This method does not validate against capturing a same color piece,
    /// nor does it validate against pieces being in a invalid position(King getting out of the palace for example)
    /// These other conditions are validated through other methods
    /// <param name="boardPosition"></param>
    /// <param name="startingPoint"></param>
    /// <param name="destination"></param>
    /// <returns>A boolean indicating if the move is valid or not</returns>
    bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination);

    /// <summary>
    /// Check if the destination falls within the range of the piece, regardless of the current board state
    /// </summary>
    /// <param name="color"></param>
    /// <param name="destination"></param>
    /// <returns>A boolean indicating if the destination is valid or not</returns>
    bool AreCoordinatesValid(Side color, Coordinate destination);

    int[] GetPossibleRows(Side color);
    
    int[] GetPossibleColumns();

    bool IsProposedMoveValid(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination);

    Coordinate GetRandomCoordinate(Random random, Side side)
    {
        int randomRowIndex = random.Next(0, GetPossibleRows(side).Length);
        int randomColumnIndex = random.Next(0, GetPossibleColumns().Length);

		return new Coordinate(GetPossibleColumns()[randomColumnIndex], GetPossibleRows(side)[randomRowIndex]);
	}
}
