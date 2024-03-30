namespace XiangqiCore.Pieces.ValidationStrategy;
public interface IValidationStrategy
{
    /// <summary>
    /// Check if making this move would break any rules against the move logic of the piece
    /// For example, check if the cannon is going across pieces without capturing.
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

    bool WillExposeKingToDanger(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination);

    int[] GetPossibleRows(Side color);
    
    int[] GetPossibleColumns();
}
