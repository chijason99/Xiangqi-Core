namespace XiangqiCore;

public class XiangqiBuilder : IXiangqiBuilder
{
    private const string _defaultStartingPositionFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w";

    public XiangqiBuilder()
    {
    }
    private string _initialFen { get; set; } = _defaultStartingPositionFen;
    private Side _sideToMove { get; set; } = Side.Red;

    private Player _redPlayer { get; set; }
    private Player _blackPlayer { get; set; }

    public XiangqiBuilder UseDefaultConfiguration()
    {
        _initialFen = _defaultStartingPositionFen;
        _sideToMove = Side.Red;

        _redPlayer = new();
        _blackPlayer = new();

        return this;
    }

    public XiangqiBuilder UseCustomFen(string customFen)
    {
        _initialFen = customFen;

        return this;
    }

    public XiangqiGame Build()
    {
        return new XiangqiGame(initialFenString: _initialFen, sideToMove:_sideToMove, redPlayer: _redPlayer, blackPlayer: _blackPlayer);
    }

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
}