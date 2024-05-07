using LinqKit;
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
     => coordinateToCheck.Equals(Coordinate.Empty) || position.GetPieceAtPosition(coordinateToCheck) is not EmptyPiece;

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
    
    public static void SetPieceAtPosition(this Piece[,] boardPosition, Coordinate targetCoordinates, Piece targetPiece)
    {
        int row = targetCoordinates.Row - 1;
        int column = targetCoordinates.Column - 1;

        boardPosition[row, column] = targetPiece;
    }

    public static bool WillExposeKingToDanger(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        if (!boardPosition.HasPieceAtPosition(startingPosition))
            throw new ArgumentException("The starting position must contain a piece");

        Piece pieceToMove = boardPosition.GetPieceAtPosition(startingPosition);
        Side targetKingSide = pieceToMove.Side;
        Piece[,] positionAfterSimulation = boardPosition.SimulateMove(startingPosition, destination);
        King targetKing = positionAfterSimulation.GetPiecesOfType<King>(targetKingSide).Single();

        // Check Against Rook
        if (positionAfterSimulation.IsKingAttackedBy<Rook>(targetKing.Coordinate))
            return true;

        // Check Against King
        if (positionAfterSimulation.IsKingExposedDirectlyToEnemyKing(targetKing.Coordinate))
            return true;

        // Check Against Cannon
        if (positionAfterSimulation.IsKingAttackedBy<Cannon>(targetKing.Coordinate))
            return true;

        // Check Against Pawn
        if (positionAfterSimulation.IsKingAttackedBy<Pawn>(targetKing.Coordinate))
            return true;

        // Check Against Knight
        if (positionAfterSimulation.IsKingAttackedBy<Knight>(targetKing.Coordinate))
            return true;

        return false;
    }

    public static IEnumerable<TPieceType> GetPiecesOfType<TPieceType>(this Piece[,] boardPosition, Side side) where TPieceType : Piece
        => boardPosition
                .OfType<TPieceType>()
                .Where(piece => piece.Side == side);

    public static bool IsKingAttackedBy<TPieceType>(this Piece[,] boardPosition, Coordinate kingCoordinate) where TPieceType : Piece
    {
        King kingToCheck = (King)boardPosition.GetPieceAtPosition(kingCoordinate);
        Side enemySide = kingToCheck.Side.GetOppositeSide();
        IEnumerable<TPieceType> enemyPieces = boardPosition.GetPiecesOfType<TPieceType>(enemySide);

        if (!enemyPieces.Any())
            return false;

        foreach(TPieceType enemyPiece in enemyPieces)
        {
            if (enemyPiece.ValidationStrategy.ValidateMoveLogicForPiece(boardPosition, enemyPiece.Coordinate, kingCoordinate))
                return true;
        }

        return false;
    }
    
    public static bool IsKingExposedDirectlyToEnemyKing(this Piece[,] boardPosition, Coordinate kingCoordinate)
    {
        King targetKing = (King)boardPosition.GetPieceAtPosition(kingCoordinate);
        King opponentKing = boardPosition
                                .GetPiecesOfType<King>(targetKing.Side.GetOppositeSide())
                                .Single();

        if (targetKing.Coordinate.Column != opponentKing.Coordinate.Column)
            return false;

        return boardPosition.CountPiecesBetweenOnColumn(kingCoordinate, opponentKing.Coordinate) == 0;
    }

    public static Piece[,] SimulateMove(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        if (!boardPosition.HasPieceAtPosition(startingPosition))
            throw new ArgumentException("There must be a piece at the starting position on the board");

        Piece[,] boardPositionClone = (Piece[,]) boardPosition.Clone();
        boardPositionClone.MakeMove(startingPosition, destination);

        return boardPositionClone;
    }

    public static void MakeMove(this Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        Piece pieceAtStartingPosition = boardPosition.GetPieceAtPosition(startingPosition);

        Piece movedPiece = PieceFactory.Create(pieceAtStartingPosition.PieceType, pieceAtStartingPosition.Side, destination);
        Piece emptyPiece = PieceFactory.CreateEmptyPiece();

        boardPosition.SetPieceAtPosition(startingPosition, emptyPiece);
        boardPosition.SetPieceAtPosition(destination, movedPiece);
    }
}
