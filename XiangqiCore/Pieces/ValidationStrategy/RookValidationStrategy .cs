namespace XiangqiCore.Pieces.ValidationStrategy;
public class RookValidationStrategy : DefaultValidationStrategy
{
    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        if (startingPosition.Row != destination.Row && startingPosition.Column != destination.Column)
            return false;

        return true;
    }
}
