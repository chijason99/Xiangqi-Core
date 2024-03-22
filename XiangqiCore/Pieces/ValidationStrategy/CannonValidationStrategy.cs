﻿namespace XiangqiCore.Pieces.ValidationStrategy;
public class CannonValidationStrategy : IValidationStrategy
{
    public bool AreCoordinatesValid(Side color, Coordinate destination) => true;

    public bool ValidateMove(Coordinate startingPosition, Coordinate destination)
    {
        return true;
    }
}
