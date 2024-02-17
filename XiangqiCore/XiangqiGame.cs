namespace XiangqiCore;

public class XiangqiGame
{
    internal XiangqiGame() { }

    internal XiangqiGame(string initialFenString, 
                         Side sideToMove, 
                         Player redPlayer, 
                         Player blackPlayer, 
                         string competition,
                         DateTime gameDate)
    {
        InitialFenString = initialFenString;
        SideToMove = sideToMove;
        RedPlayer = redPlayer;
        BlackPlayer = blackPlayer;
        Competition = competition;
        GameDate = gameDate;
    }
    public string InitialFenString { get; init; }

    public Side SideToMove { get; private set; }

    public Player RedPlayer { get; private set; }
    public Player BlackPlayer { get; private set; }

    public string Competition { get; private set; }

    public DateTime GameDate { get; private set; }
}
