using XiangqiCore.Pieces;

namespace XiangqiCore.Boards;
public class Board
{
    private const string _emptyBoardFen = "9/9/9/9/9/9/9/9/9/9 w - - 0 0";

    public Board()
    {
        Position = FenHelper.CreatePositionFromFen(_emptyBoardFen);
    }

    public Board(string fenString)
    {
        Position = FenHelper.CreatePositionFromFen(fenString);
    }

    public Board(BoardConfig config) : this()
    {
        foreach (var keyValuePair in config.PiecesToAdd)
            SetPieceAtPosition(keyValuePair.Key, keyValuePair.Value);
    }

    public Piece[,] Position { get; private set; }

    public void SetPieceAtPosition(Coordinate targetCoordinates, Piece targetPiece)
    {
        int row = targetCoordinates.Row - 1;
        int column = targetCoordinates.Column - 1;

        Position[row, column] = targetPiece;
    }

    public Piece GetPieceAtPosition(Coordinate targetCoordinates)
    {
        int row = targetCoordinates.Row - 1;
        int column = targetCoordinates.Column - 1;

        return Position[row, column];
    }
        
}
