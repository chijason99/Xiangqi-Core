using XiangqiCore.Boards;
using XiangqiCore.Extension;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class DefaultValidationStrategy : IValidationStrategy
{
    public bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        int[] palaceRows = GetPossibleRows(color);
        int[] palaceColumns = GetPossibleColumns();

        return palaceColumns.Contains(destination.Column) && palaceRows.Contains(destination.Row);
    }

    public virtual int[] GetPossibleColumns() => Board.GetAllColumns;

    public virtual int[] GetPossibleRows(Side color) => Board.GetAllRows;

    public bool IsProposedMoveValid(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination)
    {
        Piece pieceToMove = boardPosition.GetPieceAtPosition(startingPoint);
        Side side = pieceToMove.Side;
        IValidationStrategy validationStrategy = pieceToMove.ValidationStrategy;

        return validationStrategy.AreCoordinatesValid(side, destination) &&
               validationStrategy.ValidateMoveLogicForPiece(boardPosition, startingPoint, destination) &&
               !WillExposeKingToDanger(boardPosition, startingPoint, destination) &&
               !IsDestinationContainingHavingFriendlyPiece(boardPosition, side, destination);
    }
    public virtual bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination) => true;

    public bool WillExposeKingToDanger(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination)
    {
        return false;
    }

    public bool IsDestinationContainingHavingFriendlyPiece(Piece[,] boardPosition,Side color ,Coordinate destination)
      => !boardPosition.GetPieceAtPosition(destination).Equals(new EmptyPiece()) && boardPosition.GetPieceAtPosition(destination).Side == color;
}
