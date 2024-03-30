using XiangqiCore.Boards;
using XiangqiCore.Extension;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class KingValidationStrategy : DefaultValidationStrategy
{
    public override bool AreCoordinatesValid(Side color, Coordinate destination)
    {
        int[] palaceRows = GetPossibleRows(color);
        int[] palaceColumns = GetPossibleColumns();

        return palaceColumns.Contains(destination.Column) && palaceRows.Contains(destination.Row);
    }

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        if(startingPosition.Row != destination.Row && startingPosition.Column != destination.Column)
            return false;

        // Move horizontally
        if(startingPosition.Row == destination.Row)
            return startingPosition.Column == destination.Column + 1 || startingPosition.Column == destination.Column - 1;

        // Move vertically
        if (startingPosition.Column == destination.Column)
            return startingPosition.Row == destination.Row + 1 || startingPosition.Row == destination.Row - 1;

        return false;
    }

    public override int[] GetPossibleRows(Side color) => Board.GetPalaceRows(color);

    public override int[] GetPossibleColumns() => Board.GetPalaceColumns;
}
