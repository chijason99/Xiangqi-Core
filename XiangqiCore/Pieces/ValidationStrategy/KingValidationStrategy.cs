using XiangqiCore.Boards;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class KingValidationStrategy : DefaultValidationStrategy
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
}
