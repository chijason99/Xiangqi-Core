using XiangqiCore.Boards;
using XiangqiCore.Extension;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class DefaultValidationStrategy : IValidationStrategy
{
    public virtual bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        int[] rows = GetPossibleRows(color);
        int[] columns = GetPossibleColumns();

        return columns.Contains(destination.Column) && rows.Contains(destination.Row);
    }

    public virtual int[] GetPossibleColumns() => Board.GetAllColumns();

    public virtual int[] GetPossibleRows(Side color) => Board.GetAllRows();

    public bool IsProposedMoveValid(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination)
    {
        Piece pieceToMove = boardPosition.GetPieceAtPosition(startingPoint);
        Side side = pieceToMove.Side;
        IValidationStrategy validationStrategy = pieceToMove.ValidationStrategy;

        if (!validationStrategy.AreCoordinatesValid(side, destination))
            return false;

        if (boardPosition.IsDestinationContainingFriendlyPiece(startingPoint, destination))
			return false;

        if (!validationStrategy.ValidateMoveLogicForPiece(boardPosition, startingPoint, destination))
            return false;

        if (boardPosition.WillExposeKingToDanger(startingPoint, destination))
			return false;

        return true;
    }

    public virtual bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination) => true;
}
