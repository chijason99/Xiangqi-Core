using XiangqiCore.Boards;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class DefaultValidationStrategy : IValidationStrategy
{
    public virtual bool AreCoordinatesValid(Side color, Coordinate destination) => true;

    public virtual int[] GetPossibleColumns() => Board.GetAllColumns;

    public virtual int[] GetPossibleRows(Side color) => Board.GetAllRows;

    public virtual bool IsProposedMoveValid(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination) => true;
    public virtual bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination) => true;

    public bool WillExposeKingToDanger(Piece[,] boardPosition, Coordinate startingPoint, Coordinate destination)
    {
        return false;
    }
}
