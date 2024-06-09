using XiangqiCore.Boards;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class AdvisorValidationStrategy : DefaultValidationStrategy
{
    public override int[] GetPossibleColumns() => Board.GetPalaceColumns();

    public override int[] GetPossibleRows(Side color) => Board.GetPalaceRows(color);

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingUp = destination.Row == startingPosition.Row + 1 ;
        bool isMovingDown = destination.Row == startingPosition.Row - 1 ;
        bool isMovingLeft = destination.Column == startingPosition.Column - 1 ;
        bool isMovingRight = destination.Column == startingPosition.Column + 1 ;

        bool isMovingTopLeft = isMovingUp && isMovingLeft;
        bool isMovingTopRight = isMovingUp && isMovingRight;
        bool isMovingBottomLeft = isMovingDown && isMovingLeft;
        bool isMovingBottomRight = isMovingDown && isMovingRight;

        return isMovingTopLeft || isMovingTopRight || isMovingBottomLeft || isMovingBottomRight;
    }
}
