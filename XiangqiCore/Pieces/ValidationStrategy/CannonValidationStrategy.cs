using XiangqiCore.Extension;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class CannonValidationStrategy : DefaultValidationStrategy
{
    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingHorizontally = startingPosition.Row == destination.Row;
        bool isMovingVertically = startingPosition.Column == destination.Column;

        if (!isMovingHorizontally && !isMovingVertically) return false;

        bool isCapturing = boardPosition.HasPieceAtPosition(destination);
        int numberOfPiecesBetween = isMovingHorizontally ? boardPosition.CountPiecesBetweenOnRow(startingPosition, destination) : boardPosition.CountPiecesBetweenOnColumn(startingPosition, destination);
        const int numberOfPiecesBetweenForCapturing = 1;
        const int numberOfPiecesBetweenForNormalMove = 0;

        bool isValidCapture = isCapturing && numberOfPiecesBetween == numberOfPiecesBetweenForCapturing;
        bool isValidNormalMove = !isCapturing && numberOfPiecesBetween == numberOfPiecesBetweenForNormalMove;

        return isValidCapture || isValidNormalMove;
    }
}
