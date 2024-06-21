using XiangqiCore.Boards;
using XiangqiCore.Extension;

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

        bool areCoordinatesValid = validationStrategy.AreCoordinatesValid(side, destination);
        bool isDestinationContainingFriendlyPiece = boardPosition.IsDestinationContainingFriendlyPiece(startingPoint, destination);
        bool isMoveLogicValid = validationStrategy.ValidateMoveLogicForPiece(boardPosition, startingPoint, destination);
        bool willExposeKingToDanger = boardPosition.WillExposeKingToDanger(startingPoint, destination);

        return areCoordinatesValid &&
               !isDestinationContainingFriendlyPiece &&
               isMoveLogicValid &&
               !willExposeKingToDanger;
    }
    public virtual bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination) => true;
}
