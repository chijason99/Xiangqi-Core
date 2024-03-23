using XiangqiCore.Boards;
using XiangqiCore.Exceptions;

namespace XiangqiCore;

public class XiangqiGame
{
    internal XiangqiGame() { }

    private XiangqiGame(string initialFenString, 
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

        CreatedDate = DateTime.Today;
        UpdatedDate = DateTime.Today;
    }
    public string InitialFenString { get; init; }

    public Side SideToMove { get; private set; }

    public Player RedPlayer { get; private set; }
    public Player BlackPlayer { get; private set; }

    public string Competition { get; private set; }

    public DateTime GameDate { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }

    public Board Board { get; private set; }

    public static XiangqiGame Create(string initialFenString, Side sideToMove, Player redPlayer, Player blackPlayer,
                                     string competition, DateTime gameDate, bool useBoardConfig = false, BoardConfig? boardConfig = null)
    {
        bool isFenValid = FenHelper.Validate(initialFenString);

        if (!isFenValid) throw new InvalidFenException(initialFenString);

        XiangqiGame createdGameInstance = new(initialFenString, sideToMove, redPlayer, blackPlayer, competition, gameDate)
        {
            Board = useBoardConfig ? new Board(boardConfig!) : new Board(initialFenString)
        };

        return createdGameInstance;
    }
}
