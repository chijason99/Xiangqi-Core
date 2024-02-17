namespace XiangqiCore;

public class XiangqiGame
{
    internal XiangqiGame() { }

    internal XiangqiGame(string initialFenString, Side sideToMove, Player redPlayer, Player blackPlayer)
    {
        InitialFenString = initialFenString;
        SideToMove = sideToMove;
        RedPlayer = redPlayer;
        BlackPlayer = blackPlayer;
    }
    public string InitialFenString { get; init; }

    public Side SideToMove { get; private set; }

    public Player RedPlayer { get; private set; }
    public Player BlackPlayer { get; private set; }
}
