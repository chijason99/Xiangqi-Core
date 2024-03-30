using XiangqiCore.Pieces;

namespace XiangqiCore.Extension;
public static class PieceExtension
{
    public static Piece GetPieceAtPosition(this Piece[,] position, Coordinate targetCoordinate)
    {
        int rowInPositionArray = targetCoordinate.Row - 1;
        int columnInPositionArray = targetCoordinate.Column - 1;

        return position[rowInPositionArray, columnInPositionArray];
    }
}
