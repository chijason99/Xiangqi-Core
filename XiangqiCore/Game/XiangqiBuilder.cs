using XiangqiCore.Boards;
using XiangqiCore.Misc;

namespace XiangqiCore.Game;

public class XiangqiBuilder : IXiangqiBuilder
{
    private const string _defaultStartingPositionFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0";
    private const string _emptyStartingPositionFen = "9/9/9/9/9/9/9/9/9/9 w - - 0 0";

    public XiangqiBuilder()
    {
    }
    private string _initialFen { get; set; } = _defaultStartingPositionFen;
    private Side _sideToMove { get; set; }

    private Player _redPlayer { get; set; }
    private Player _blackPlayer { get; set; }

    private Competition _competition { get; set; }
    private GameResult _gameResult { get; set; } = GameResult.Unknown;

    private bool _useBoardConfig { get; set; } = false;
    private BoardConfig? _boardConfig { get; set; } = null;

    private string _moveRecord { get; set; } = "";

    public XiangqiBuilder UseDefaultConfiguration()
    {
        _initialFen = _defaultStartingPositionFen;
        _sideToMove = Side.Red;

        _redPlayer = new();
        _blackPlayer = new();

        CompetitionBuilder competitionBuilder = new();

        // Create a default competition
        _competition = competitionBuilder.Build();
        _gameResult = GameResult.Unknown;

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
         => XiangqiGame.Create(_initialFen, _redPlayer, _blackPlayer, _competition, _useBoardConfig, _boardConfig, _gameResult, _moveRecord);

    public XiangqiBuilder WithRedPlayer(Action<Player> action)
    {
        Player redPlayer = new();

        action(redPlayer);

        _redPlayer = redPlayer;

        return this;
    }

    public XiangqiBuilder WithBlackPlayer(Action<Player> action)
    {
        Player blackPlayer = new();

        action(blackPlayer);

        _blackPlayer = blackPlayer;

        return this;
    }

    public XiangqiBuilder WithGameResult(GameResult gameResult)
    {
        _gameResult = gameResult;

        return this;
    }

    public XiangqiBuilder WithCompetition(Action<CompetitionBuilder> action)
    {
        CompetitionBuilder competitionBuilder = new();

        action.Invoke(competitionBuilder);

        _competition = competitionBuilder.Build();

        return this;
    }

    public XiangqiBuilder UseBoardConfig(BoardConfig config)
    {
        _boardConfig = config;
        _useBoardConfig = true;

        return this;
    }

    /// <summary>
    /// This method is used to set the move record for the game. Currently, it only supports Chinese move records
    /// </summary>
    /// <param name="moveRecord"></param>
    /// <returns></returns>
    public XiangqiBuilder WithMoveRecord(string moveRecord)
    {
        _moveRecord = moveRecord;

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