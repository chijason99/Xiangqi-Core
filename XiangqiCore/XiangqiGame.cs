namespace XiangqiCore;

public class XiangqiGame
{
    internal XiangqiGame() { }

    internal XiangqiGame(string initialFenString, Side sideToMove)
    {
        InitialFenString = initialFenString;
        SideToMove = sideToMove;
    }
    public string InitialFenString { get; init; }

    public Side SideToMove { get; private set; }
}
