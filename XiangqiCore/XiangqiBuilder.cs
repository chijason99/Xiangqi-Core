using XiangqiCore.Boards;

namespace XiangqiCore;

public class XiangqiBuilder : IXiangqiBuilder
{
    private const string _defaultStartingPositionFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0";
    private const string _emptyStartingPositionFen = "9/9/9/9/9/9/9/9/9/9 w - - 0 0";

    public XiangqiBuilder()
    {
    }
    private string _initialFen { get; set; }
    private Side _sideToMove { get; set; }

    private Player _redPlayer { get; set; }
    private Player _blackPlayer { get; set; }
    private string _competition { get; set; }
    private DateTime _gameDate { get; set; }

    private bool _useBoardConfig { get; set; } = false;
    private BoardConfig? _boardConfig { get; set; } = null;

    public XiangqiBuilder UseDefaultConfiguration()
    {
        _initialFen = _defaultStartingPositionFen;
        _sideToMove = Side.Red;

        _redPlayer = new();
        _blackPlayer = new();

        _competition = "Unknown";
        _gameDate = DateTime.Today;

        return this;
    }

    public XiangqiBuilder UseCustomFen(string customFen)
    {
        _initialFen = customFen;

        return this;
    }

    public XiangqiBuilder UseEmptyBoard()
    {
        _initialFen = _emptyStartingPositionFen;

        return this;
    }

    public XiangqiGame Build()
         => XiangqiGame.Create(initialFenString: _initialFen, sideToMove: _sideToMove, redPlayer: _redPlayer,
                               blackPlayer: _blackPlayer, competition: _competition, gameDate: _gameDate, useBoardConfig: _useBoardConfig, boardConfig: _boardConfig);

    public XiangqiBuilder HasRedPlayer(Action<Player> action)
    {
        Player redPlayer = new();

        action(redPlayer);

        _redPlayer = redPlayer;

        return this;
    }

    public XiangqiBuilder HasBlackPlayer(Action<Player> action)
    {
        Player blackPlayer = new();

        action(blackPlayer);

        _blackPlayer = blackPlayer;

        return this;
    }

    public XiangqiBuilder PlayedInCompetition(string competitionName)
    {
        _competition = competitionName;

        return this;
    }

    public XiangqiBuilder PlayedOnDate(DateTime gameDate)
    {
        _gameDate = gameDate;

        return this;
    }

    public XiangqiBuilder UseBoardConfig(BoardConfig config)
    {
        _boardConfig = config;
        _useBoardConfig = true;

        return this;
    }

    public XiangqiBuilder UseBoardConfig(Action<BoardConfig> action)
    {
        BoardConfig config = new();

        action.Invoke(config);

        _boardConfig = config;
        _useBoardConfig = true;

        return this;
    }
}