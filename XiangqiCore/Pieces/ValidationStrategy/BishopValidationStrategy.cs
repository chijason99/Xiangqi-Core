using XiangqiCore.Exceptions;
using XiangqiCore.Extension;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class BishopValidationStrategy : DefaultValidationStrategy
{
    public override int[] GetPossibleColumns() => [1, 3, 5, 7, 9];

    public override int[] GetPossibleRows(Side color)
        => color == Side.Red ? [1, 3, 5] : color == Side.Black ? [6, 8, 10] : throw new InvalidSideException(color);

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition , Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingUp = startingPosition.Row + 2 == destination.Row;
        bool isMovingDown = startingPosition.Row - 2 == destination.Row;
        bool isMovingLeft = startingPosition.Column - 2 == destination.Column;
        bool isMovingRight = startingPosition.Column + 2 == destination.Column;

        bool isMovingTopLeft = isMovingUp && isMovingLeft;
        bool isMovingTopRight = isMovingUp && isMovingRight;
        bool isMovingBottomLeft = isMovingDown && isMovingLeft;
        bool isMovingBottomRight = isMovingDown && isMovingRight;

        Coordinate topLeftObstacleCoordinate = GetObstacleCoordinateOnMoveDirection(BishopMoveDirection.TopLeft, startingPosition);
        Coordinate topRightObstacleCoordinate = GetObstacleCoordinateOnMoveDirection(BishopMoveDirection.TopRight, startingPosition);
        Coordinate bottomLeftObstacleCoordinate = GetObstacleCoordinateOnMoveDirection(BishopMoveDirection.BottomLeft, startingPosition);
        Coordinate bottomRightObstacleCoordinate = GetObstacleCoordinateOnMoveDirection(BishopMoveDirection.BottomRight, startingPosition);

        return (isMovingTopLeft && !boardPosition.HasPieceAtPosition(topLeftObstacleCoordinate)) ||
               (isMovingTopRight && !boardPosition.HasPieceAtPosition(topRightObstacleCoordinate)) ||
               (isMovingBottomLeft && !boardPosition.HasPieceAtPosition(bottomLeftObstacleCoordinate)) ||
               (isMovingBottomRight && !boardPosition.HasPieceAtPosition(bottomRightObstacleCoordinate));
    }

    private Coordinate GetObstacleCoordinateOnMoveDirection(BishopMoveDirection bishopMoveDirection, Coordinate startingPosition)
    {
        int startingRow = startingPosition.Row;
        int startingColumn = startingPosition.Column;

        return bishopMoveDirection switch
        {
            BishopMoveDirection.TopLeft => new Coordinate(startingRow + 1, startingColumn - 1),
            BishopMoveDirection.TopRight => new Coordinate(startingRow + 1, startingColumn + 1),
            BishopMoveDirection.BottomLeft => new Coordinate(startingRow - 1, startingColumn - 1),
            BishopMoveDirection.BottomRight => new Coordinate(startingRow - 1, startingColumn + 1),
            _ => throw new ArgumentException("Invalid bishop move direction")
        };
    }

    private enum BishopMoveDirection
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
