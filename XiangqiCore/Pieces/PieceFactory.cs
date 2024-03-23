using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Results;
using XiangqiCore.Results.Errors;

namespace XiangqiCore.Pieces;
public static class PieceFactory
{
    public static Result<Piece> Create(PieceType pieceType, Side color, Coordinate coordinate)
        => pieceType switch
        {
            PieceType.King => Result<Piece>.Success(new King(coordinate, color)),
            PieceType.Rook => Result<Piece>.Success(new Rook(coordinate, color)),
            PieceType.Knight => Result<Piece>.Success(new Knight(coordinate, color)),
            PieceType.Cannon => Result<Piece>.Success(new Cannon(coordinate, color)),
            PieceType.Advisor => Result<Piece>.Success(new Advisor(coordinate, color)),
            PieceType.Bishop => Result<Piece>.Success(new Bishop(coordinate, color)),
            PieceType.Pawn => Result<Piece>.Success(new Pawn(coordinate, color)),
            _ => Result<Piece>.Failure(CreatePieceError.InvalidPieceTypeError)
        };

    public static Result<Piece> CreateEmptyPiece() => Result<Piece>.Success(new EmptyPiece());
}
