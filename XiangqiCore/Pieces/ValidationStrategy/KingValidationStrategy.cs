using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class KingValidationStrategy : DefaultValidationStrategy, IValidationStrategy, IHasSpecificPositions
{
    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingInTheSameRow = startingPosition.Row == destination.Row;
        bool isMovingInTheSameColumn = startingPosition.Column == destination.Column;

        if (!isMovingInTheSameColumn && !isMovingInTheSameRow)
            return false;

        if (isMovingInTheSameRow)
        {
            bool isMovingLeftOneStep = startingPosition.Column == destination.Column - 1;
            bool isMovingRightOneStep = startingPosition.Column == destination.Column + 1;
            
            return isMovingLeftOneStep || isMovingRightOneStep;
        }

        if (isMovingInTheSameColumn)
        {
            bool isMovingUpOneStep = startingPosition.Row == destination.Row - 1;
            bool isMovingDownOneStep = startingPosition.Row == destination.Row + 1;

            return isMovingUpOneStep || isMovingDownOneStep;
        }

        return false;
    }

    public override int[] GetPossibleRows(Side color) => Board.GetPalaceRows(color);

    public override int[] GetPossibleColumns() => Board.GetPalaceColumns();
    
    public Coordinate[] GetSpecificPositions(Side side)
    {
        return side switch
        {
            Side.None => throw new InvalidSideException(side),
            Side.Red =>
            [
                new Coordinate(4, 1), 
                new Coordinate(4, 2), 
                new Coordinate(4, 3), 
                new Coordinate(5, 1),
                new Coordinate(5, 2),
                new Coordinate(5, 3), 
                new Coordinate(6, 1), 
                new Coordinate(6, 2),
                new Coordinate(6, 3),
            ],
            _ =>
            [
                new Coordinate(4, 8), 
                new Coordinate(4, 9), 
                new Coordinate(4, 10), 
                new Coordinate(5, 8),
                new Coordinate(5, 9), 
                new Coordinate(5, 10), 
                new Coordinate(6, 8), 
                new Coordinate(6, 9),
                new Coordinate(6, 10),
            ]
        };
    }
    
    public Coordinate[] GetValidCoordinates(Side side) 
        => GetSpecificPositions(side);
}
