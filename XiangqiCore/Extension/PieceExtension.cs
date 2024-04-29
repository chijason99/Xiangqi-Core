using LinqKit;
using System.Drawing;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Extension;
public static class PieceExtension
{
    public static ExpressionStarter<Piece> BuildPieceWhereClause() => PredicateBuilder.New<Piece>();

    public static Piece GetPieceAtPosition(this Piece[,] position, Coordinate targetCoordinate)
    {
        int rowInPositionArray = targetCoordinate.Row - 1;
        int columnInPositionArray = targetCoordinate.Column - 1;

        return position[rowInPositionArray, columnInPositionArray];
    }

    public static bool HasPieceAtPosition(this Piece[,] position, Coordinate coordinateToCheck)
     => position.GetPieceAtPosition(coordinateToCheck) is not EmptyPiece;

    public static IEnumerable<Piece> GetPiecesOnRow(this Piece[,] position, int row)
    => position
        .Cast<Piece>()
        .Where(x => x.Coordinate.Row == row && x.PieceType != PieceType.None)
        .OrderBy(x => x.Coordinate.Column);

    public static IEnumerable<Piece> GetPiecesOnColumn(this Piece[,] position, int column)
    => position
        .Cast<Piece>()
        .Where(p => p.Coordinate.Column == column && p.PieceType != PieceType.None)
        .OrderBy(p => p.Coordinate.Row);

    public static int CountPiecesBetweenOnRow(this Piece[,] position, Coordinate startingPosition, Coordinate destination)
    {
        if (startingPosition.Row != destination.Row)
            throw new ArgumentException("The two coordinates need to be on the same row");

        if (startingPosition.Column == destination.Column)
            throw new ArgumentException("The starting and ending column cannot be the same");

        IEnumerable<Piece> piecesOnRow = position.GetPiecesOnRow(startingPosition.Row);

        ExpressionStarter<Piece> predicate = BuildPieceWhereClause();

        if (startingPosition.Column < destination.Column)
            predicate.And(piece => piece.Coordinate.Column > startingPosition.Column && piece.Coordinate.Column < destination.Column);
        else
            predicate.And(piece => piece.Coordinate.Column < startingPosition.Column && piece.Coordinate.Column > destination.Column);

        return piecesOnRow.Count(predicate);
    }

    public static int CountPiecesBetweenOnColumn(this Piece[,] position, Coordinate startingPosition, Coordinate destination)
    {
        if (startingPosition.Column != destination.Column)
            throw new ArgumentException("The two coordinates need to be on the same column");

        if (startingPosition.Row == destination.Row)
            throw new ArgumentException("The starting and ending column cannot be the same");

        IEnumerable<Piece> piecesOnRow = position.GetPiecesOnColumn(startingPosition.Column);

        ExpressionStarter<Piece> predicate = BuildPieceWhereClause();

        if (startingPosition.Row < destination.Row)
            predicate.And(piece => piece.Coordinate.Row > startingPosition.Row && piece.Coordinate.Row < destination.Row);
        else
            predicate.And(piece => piece.Coordinate.Row < startingPosition.Row && piece.Coordinate.Row > destination.Row);

        return piecesOnRow.Count(predicate);
    }

    public static bool IsDestinationContainingFriendlyPiece(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        Side side = boardPosition.GetPieceAtPosition(startingPosition).Side;

        return boardPosition.HasPieceAtPosition(destination) && boardPosition.GetPieceAtPosition(destination).Side == side;
    }
    
    public static bool WillExposeKingToDanger(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        return false;
    }
}
