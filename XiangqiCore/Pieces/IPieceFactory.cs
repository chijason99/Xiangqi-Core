using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Pieces;
public interface IPieceFactory
{
   Piece Create(PieceType pieceType, Side color, Coordinate coordinate);
}
