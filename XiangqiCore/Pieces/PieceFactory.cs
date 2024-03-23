using XiangqiCore.Exceptions;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Pieces;
public static class PieceFactory
{
    public static Piece Create(PieceType pieceType, Side color, Coordinate coordinate)
        => pieceType switch
        {
            PieceType.King => new King(coordinate, color),
            PieceType.Rook => new Rook(coordinate, color),
            PieceType.Knight => new Knight(coordinate, color),
            PieceType.Cannon => new Cannon(coordinate, color),
            PieceType.Advisor => new Advisor(coordinate, color),
            PieceType.Bishop => new Bishop(coordinate, color),
            PieceType.Pawn => new Pawn(coordinate, color),
            _ => throw new InvalidPieceTypeException("The given piece Type is invalid")
        };

    public static Piece CreateEmptyPiece() => new EmptyPiece();
}
