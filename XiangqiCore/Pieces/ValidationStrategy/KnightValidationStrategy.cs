﻿namespace XiangqiCore.Pieces.ValidationStrategy;
public class KnightValidationStrategy : IValidationStrategy
{
    public bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        throw new NotImplementedException();
    }

    public bool ValidateMove(Coordinate startingPosition, Coordinate destination)
    {
        return true;
    }
}