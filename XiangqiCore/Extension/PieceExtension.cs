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

    public static IEnumerable<Piece> GetPiecesOnRow(this Piece[,] position, int row)
    => position
        .Cast<Piece>()
        .Where(x => x.Coordinate.Row == row && x.GetPieceType != Pieces.PieceTypes.PieceType.None)
        .OrderBy(x => x.Coordinate.Column);
}
