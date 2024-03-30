namespace XiangqiCore.Pieces.ValidationStrategy;
public class CannonValidationStrategy : DefaultValidationStrategy
{
    public override bool AreCoordinatesValid(Side color, Coordinate destination) => true;

    public override int[] GetPossibleColumns()
    {
        throw new NotImplementedException();
    }

    public override int[] GetPossibleRows(Side color)
    {
        throw new NotImplementedException();
    }

    public override bool IsProposedMoveValid(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        return true;
    }
}
