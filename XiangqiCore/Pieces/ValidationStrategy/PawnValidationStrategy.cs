using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class PawnValidationStrategy : DefaultValidationStrategy
{
    public override int[] GetPossibleRows(Side color)
    {
        int[] unavailableRowsForRedPawn = [1, 2, 3];
        int[] unavailableRowsForBlackPawn = [8, 9, 10];

        return color switch
                {
                    Side.Red => Board.GetGetAllRows().Except(unavailableRowsForRedPawn).ToArray(),
                    Side.Black => Board.GetGetAllRows().Except(unavailableRowsForBlackPawn).ToArray(),
                    _ => throw new InvalidSideException(color)
                };
    }

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        Pawn pawn = (Pawn)boardPosition.GetPieceAtPosition(startingPosition);
        const int redRiverRow = 5;
        const int blackRiverRow = 6;

        bool isMovingForward = pawn.Side == Side.Red ? startingPosition.Row + 1 == destination.Row : startingPosition.Row - 1 == destination.Row;
        bool isMovingLeft = startingPosition.Column - 1 == destination.Column;
        bool isMovingRight = startingPosition.Column + 1 == destination.Column;
        bool pawnHasCrossedTheRiver = pawn.Side == Side.Red ? startingPosition.Row >= blackRiverRow : startingPosition.Row <= redRiverRow;

        return isMovingForward || (pawnHasCrossedTheRiver && (isMovingLeft || isMovingRight));
    }
}
