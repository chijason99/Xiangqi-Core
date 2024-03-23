using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Boards;
public class BoardConfig
{
    public Dictionary<Coordinate, Piece> PiecesToAdd { get; } = [];

    public void AddPiece(PieceType targetPieceType, Side targetColor, Coordinate targetCoordinate)
    {
        PiecesToAdd[targetCoordinate] = PieceFactory.Create(targetPieceType, targetColor, targetCoordinate);
    }
}
