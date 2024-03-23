using XiangqiCore.Pieces;

namespace XiangqiCore.Boards;
public class Board
{
    public Board()
    {
        _squares = new Piece[10, 9];
    }

    public Piece[,] _squares { get; private set; }


}
