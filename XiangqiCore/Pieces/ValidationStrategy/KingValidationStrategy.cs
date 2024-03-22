namespace XiangqiCore.Pieces.ValidationStrategy;
public class KingValidationStrategy : IValidationStrategy
{
    public bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        HashSet<Coordinate> possibleCoordinatesForRedKing = [new Coordinate(4, 1), new Coordinate(4, 2), new Coordinate(4, 3),
                                                             new Coordinate(5, 1), new Coordinate(5, 2), new Coordinate(5, 3),
                                                             new Coordinate(6, 1), new Coordinate(6, 2), new Coordinate(6, 3)];        
        
        HashSet<Coordinate> possibleCoordinatesForBlackKing = [new Coordinate(4, 8), new Coordinate(4, 9), new Coordinate(4, 10),
                                                             new Coordinate(5, 8), new Coordinate(5, 9), new Coordinate(5, 10),
                                                             new Coordinate(6, 8), new Coordinate(6, 9), new Coordinate(6, 10)];

        return color switch
        {
            Side.Red => possibleCoordinatesForRedKing.Contains(destination),
            Side.Black => possibleCoordinatesForBlackKing.Contains(destination),
            _ => throw new ArgumentException("Please provide a valid side")
        };
    }

    public bool ValidateMove(Coordinate startingPosition, Coordinate destination)
    {
        if(startingPosition.Row != destination.Row && startingPosition.Column != destination.Column)
            return false;

        // Move horizontally
        if(startingPosition.Row == destination.Row)
            return startingPosition.Column == destination.Column + 1 || startingPosition.Column == destination.Column - 1;

        // Move vertically
        if (startingPosition.Column == destination.Column)
            return startingPosition.Row == destination.Row + 1 || startingPosition.Row == destination.Row - 1;

        return false;
    }
}
