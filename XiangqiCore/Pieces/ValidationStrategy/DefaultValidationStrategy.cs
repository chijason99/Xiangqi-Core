using XiangqiCore.Boards;
using XiangqiCore.Extension;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class DefaultValidationStrategy : IValidationStrategy
{
    public bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        int[] palaceRows = GetPossibleRows(color);
        int[] palaceColumns = GetPossibleColumns();

        return palaceColumns.Contains(destination.Column) && palaceRows.Contains(destination.Row);
    }

    public virtual int[] GetPossibleColumns() => Board.GetAllColumns();

    public virtual int[] GetPossibleRows(Side color) => Board.GetAllRows();

    public bool IsProposedMoveValid(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination)
    {
        Piece pieceToMove = boardPosition.GetPieceAtPosition(startingPoint);
        Side side = pieceToMove.Side;
        IValidationStrategy validationStrategy = pieceToMove.ValidationStrategy;

        return validationStrategy.AreCoordinatesValid(side, destination) &&
               validationStrategy.ValidateMoveLogicForPiece(boardPosition, startingPoint, destination) &&
               !boardPosition.WillExposeKingToDanger(startingPoint, destination) &&
               !boardPosition.IsDestinationContainingFriendlyPiece(startingPoint, destination);
    }
    public virtual bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination) => true;
}
