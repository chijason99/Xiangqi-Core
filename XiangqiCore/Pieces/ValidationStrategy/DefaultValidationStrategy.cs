namespace XiangqiCore.Pieces.ValidationStrategy;
public class DefaultValidationStrategy : IValidationStrategy
{
    public bool AreCoordinatesValid(Side color, Coordinate destination) => true;

    public bool ValidateMove(Coordinate startingPoint, Coordinate destination) => true;
}
