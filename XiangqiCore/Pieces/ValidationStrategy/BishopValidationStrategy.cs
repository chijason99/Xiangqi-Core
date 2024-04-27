using XiangqiCore.Exceptions;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class BishopValidationStrategy : DefaultValidationStrategy
{
    public override int[] GetPossibleColumns() => [1, 3, 5, 7, 9];

    public override int[] GetPossibleRows(Side color)
        => color == Side.Red ? [1, 3, 5] : color == Side.Black ? [6, 8, 10] : throw new InvalidSideException(color);

    public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition ,Coordinate startingPosition, Coordinate destination)
    {
        return true;
    }
}
