﻿using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class BishopValidationStrategy : DefaultValidationStrategy, IHasSpecificPositions, IValidationStrategy
{
    public override int[] GetPossibleColumns() => [1, 3, 5, 7, 9];

    public override int[] GetPossibleRows(Side color)
        => color == Side.Red ? [1, 3, 5] : color == Side.Black ? [6, 8, 10] : throw new InvalidSideException(color);

    public Coordinate[] GetSpecificPositions(Side side)
    {
        if (side == Side.None)
            throw new InvalidSideException(side);

        if (side == Side.Red)
        {
            return
            [
                new Coordinate(1, 3),
                new Coordinate(3, 1),
                new Coordinate(3, 5),
                new Coordinate(5, 3),
                new Coordinate(7, 1),
                new Coordinate(7, 5),
                new Coordinate(9, 3)
            ];
        }
        else
        {
            return
            [
                new Coordinate(1, 8),
                new Coordinate(3, 10),
                new Coordinate(3, 6),
                new Coordinate(5, 8),
                new Coordinate(7, 10),
                new Coordinate(7, 6),
                new Coordinate(9, 8)
            ];
        }
    }

	public override bool AreCoordinatesValid(Side color, Coordinate destination) => GetSpecificPositions(color).Contains(destination);

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

        bool isValidMovingTopLeft = isMovingTopLeft && !boardPosition.HasPieceAtPosition(topLeftObstacleCoordinate);
        bool isValidMovingTopRight = isMovingTopRight && !boardPosition.HasPieceAtPosition(topRightObstacleCoordinate);
        bool isValidMovingBottomLeft = isMovingBottomLeft && !boardPosition.HasPieceAtPosition(bottomLeftObstacleCoordinate);
        bool isValidMovingBottomRight = isMovingBottomRight && !boardPosition.HasPieceAtPosition(bottomRightObstacleCoordinate);

        return isValidMovingTopLeft || isValidMovingTopRight || isValidMovingBottomLeft || isValidMovingBottomRight;
    }

    private Coordinate GetObstacleCoordinateOnMoveDirection(BishopMoveDirection bishopMoveDirection, Coordinate startingPosition)
    {
        int startingRow = startingPosition.Row;
        int startingColumn = startingPosition.Column;

        try
        {
            return bishopMoveDirection switch
            {
                BishopMoveDirection.TopLeft => new Coordinate(startingColumn - 1, startingRow + 1),
                BishopMoveDirection.TopRight => new Coordinate(startingColumn + 1, startingRow + 1),
                BishopMoveDirection.BottomLeft => new Coordinate(startingColumn - 1, startingRow - 1),
                BishopMoveDirection.BottomRight => new Coordinate(startingColumn + 1, startingRow - 1),
                _ => throw new ArgumentException("Invalid bishop move direction")
            };
        }
        catch(ArgumentOutOfRangeException ex)
        {
            return Coordinate.Empty;
        }
    }

    private enum BishopMoveDirection
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    Coordinate IValidationStrategy.GetRandomCoordinate(Random random, Side side)
    {
        int randomIndex = random.Next(0, GetSpecificPositions(side).Length);

		return GetSpecificPositions(side)[randomIndex];
	}
}
