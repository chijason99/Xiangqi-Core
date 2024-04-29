using XiangqiCore.Extension;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class RookValidationStrategy : DefaultValidationStrategy
{
    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingHorizontally = startingPosition.Row == destination.Row;
        bool isMovingVertically = startingPosition.Column == destination.Column;

        if (!isMovingHorizontally && !isMovingVertically) return false;

        const int expectedNumberOfPiecesBetween = 0;
        int actualNumberOfPiecesBetween = isMovingHorizontally ? boardPosition.CountPiecesBetweenOnRow(startingPosition, destination) : boardPosition.CountPiecesBetweenOnColumn(startingPosition, destination);

        return actualNumberOfPiecesBetween == expectedNumberOfPiecesBetween;
    }
}
