using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Results;

namespace XiangqiCore.Pieces;
public interface IPieceFactory
{
    Result<Piece> Create(PieceType pieceType, Side color, Coordinate coordinate);
}
