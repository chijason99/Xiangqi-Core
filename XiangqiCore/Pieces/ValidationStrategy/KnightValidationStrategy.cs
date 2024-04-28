using XiangqiCore.Extension;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class KnightValidationStrategy : DefaultValidationStrategy
{
    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingUp = startingPosition.Row + 2 == destination.Row;
        bool isMovingDown = startingPosition.Row - 2 == destination.Row;
        bool isMovingLeft = startingPosition.Column - 2 == destination.Column;
        bool isMovingRight = startingPosition.Column + 2 == destination.Column;

        bool isMovingUpLeft = isMovingUp && (startingPosition.Column - 1 == destination.Column);
        bool isMovingUpRight = isMovingUp && (startingPosition.Column + 1 == destination.Column);
        bool isMovingDownLeft = isMovingDown && (startingPosition.Column - 1 == destination.Column);
        bool isMovingDownRight = isMovingDown && (startingPosition.Column + 1 == destination.Column);
        bool isMovingLeftUp = isMovingLeft && (startingPosition.Row + 1 == destination.Row);
        bool isMovingLeftDown = isMovingLeft && (startingPosition.Row - 1 == destination.Row);
        bool isMovingRightUp = isMovingRight && (startingPosition.Row + 1 == destination.Row);
        bool isMovingRightDown= isMovingRight && (startingPosition.Row - 1 == destination.Row);

        Coordinate obstacleCoordinateForMovingUp = GetObstacleCoordinateForKnightFromMoveDirection(KnighMoveMainDirection.Up, startingPosition);
        Coordinate obstacleCoordinateForMovingDown = GetObstacleCoordinateForKnightFromMoveDirection(KnighMoveMainDirection.Down, startingPosition);
        Coordinate obstacleCoordinateForMovingLeft = GetObstacleCoordinateForKnightFromMoveDirection(KnighMoveMainDirection.Left, startingPosition);
        Coordinate obstacleCoordinateForMovingRight = GetObstacleCoordinateForKnightFromMoveDirection(KnighMoveMainDirection.Right, startingPosition);

        bool isValidMovingUpLeft = isMovingUpLeft && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingUp);
        bool isValidMovingUpRight= isMovingUpRight && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingUp);
        bool isValidMovingDownLeft = isMovingDownLeft && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingDown);
        bool isValidMovingDownRight = isMovingDownRight && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingDown);
        bool isValidMovingLeftUp = isMovingLeftUp && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingLeft);
        bool isValidMovingLeftDown = isMovingLeftDown && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingLeft);
        bool isValidMovingRightUp = isMovingRightUp && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingRight);
        bool isValidMovingRightDown = isMovingRightDown && !boardPosition.HasPieceAtPosition(obstacleCoordinateForMovingRight);

        return isValidMovingUpLeft || isValidMovingUpRight || isValidMovingDownLeft || isValidMovingDownRight 
               || isValidMovingLeftUp || isValidMovingLeftDown || isValidMovingRightUp || isValidMovingRightDown;
    }

    private Coordinate GetObstacleCoordinateForKnightFromMoveDirection(KnighMoveMainDirection knighMoveDirection, Coordinate startingPosition)
    {
        int startingRow = startingPosition.Row;
        int startingColumn = startingPosition.Column;

        return knighMoveDirection switch
        {
            KnighMoveMainDirection.Up => new Coordinate(startingColumn, startingRow + 1),
            KnighMoveMainDirection.Down => new Coordinate(startingColumn, startingRow - 1),
            KnighMoveMainDirection.Left => new Coordinate(startingColumn - 1, startingRow),
            KnighMoveMainDirection.Right => new Coordinate(startingColumn + 1, startingRow),
            _ => throw new ArgumentException("Invalid knight move direction")
        };
    }

    private enum KnighMoveMainDirection
    {
        Up,
        Down,
        Left,
        Right,
    }
}
