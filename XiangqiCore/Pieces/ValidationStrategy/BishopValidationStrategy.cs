namespace XiangqiCore.Pieces.ValidationStrategy;
public class BishopValidationStrategy : DefaultValidationStrategy
{
    public override bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        HashSet<Coordinate> possibleCoordinatesForRedBishop = [new Coordinate(1, 3), new Coordinate(3, 1), new Coordinate(3, 5), new Coordinate(5, 3),
                                                              new Coordinate(7, 1), new Coordinate(7, 5), new Coordinate(9, 3)];

        HashSet<Coordinate> possibleCoordinatesForBlackBishop = [new Coordinate(1, 8), new Coordinate(3, 6), new Coordinate(3, 10), new Coordinate(5, 8),
                                                              new Coordinate(7, 6), new Coordinate(7, 10), new Coordinate(9, 8)];

        return color switch
        {
            Side.Red => possibleCoordinatesForRedBishop.Contains(destination),
            Side.Black => possibleCoordinatesForBlackBishop.Contains(destination),
            _ => throw new ArgumentException("Please provide a valid side")
        };
    }

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

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition ,Coordinate startingPosition, Coordinate destination)
    {
        return true;
    }
}
