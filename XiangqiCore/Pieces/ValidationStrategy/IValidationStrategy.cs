namespace XiangqiCore.Pieces.ValidationStrategy;
public interface IValidationStrategy
{
    bool ValidateMove(Coordinate startingPoint, Coordinate destination);

    bool AreCoordinatesValid(Side color, Coordinate destination);
}
