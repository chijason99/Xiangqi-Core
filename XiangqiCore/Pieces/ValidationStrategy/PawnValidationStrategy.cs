namespace XiangqiCore.Pieces.ValidationStrategy;
public class PawnValidationStrategy : DefaultValidationStrategy
{
    public override int[] GetPossibleColumns()
    {
        throw new NotImplementedException();
    }

    public override int[] GetPossibleRows(Side color)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        return true;
    }
}
