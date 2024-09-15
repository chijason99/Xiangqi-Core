using XiangqiCore.Boards;
using XiangqiCore.Misc;

namespace XiangqiCore.Game;

/// <summary>
/// Represents a builder class for creating instances of the Xiangqi game.
/// </summary>
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

	/// <summary>
	/// Initializes a new instance of the <see cref="XiangqiBuilder"/> class.
	/// </summary>
	public XiangqiBuilder()
	{
	}

	/// <summary>
	/// Sets the Xiangqi game configuration to the default starting position.
	/// </summary>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
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

	/// <summary>
	/// Sets the Xiangqi game configuration to a custom FEN (Forsyth-Edwards Notation).
	/// </summary>
	/// <param name="customFen">The custom FEN string representing the desired game configuration.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder UseCustomFen(string customFen)
    {
        _initialFen = customFen;

        return this;
    }

	/// <summary>
	/// Sets the Xiangqi game configuration to an empty board.
	/// </summary>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder UseEmptyBoard()
    {
        _initialFen = _emptyStartingPositionFen;

        return this;
    }

	/// <summary>
	/// Builds an instance of the Xiangqi game asynchronously.
	/// </summary>
	/// <returns>An instance of the <see cref="XiangqiGame"/> class.</returns>
	public async Task<XiangqiGame> BuildAsync()
		=> await XiangqiGame.Create(_initialFen, _redPlayer, _blackPlayer, _competition, _useBoardConfig, _boardConfig, _gameResult, _moveRecord);

	/// <summary>
	/// Sets the configuration for the red player.
	/// </summary>
	/// <param name="action">An action that configures the red player.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder WithRedPlayer(Action<Player> action)
    {
        Player redPlayer = new();

        action(redPlayer);

        _redPlayer = redPlayer;

        return this;
    }

	/// <summary>
	/// Sets the configuration for the black player.
	/// </summary>
	/// <param name="action">An action that configures the black player.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder WithBlackPlayer(Action<Player> action)
    {
        Player blackPlayer = new();

        action(blackPlayer);

        _blackPlayer = blackPlayer;

        return this;
    }

	/// <summary>
	/// Sets the game result for the Xiangqi game.
	/// </summary>
	/// <param name="gameResult">The game result.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder WithGameResult(GameResult gameResult)
    {
        _gameResult = gameResult;

        return this;
    }

	/// <summary>
	/// Sets the configuration for the competition.
	/// </summary>
	/// <param name="action">An action that configures the competition.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder WithCompetition(Action<CompetitionBuilder> action)
    {
        CompetitionBuilder competitionBuilder = new();

        action.Invoke(competitionBuilder);

        _competition = competitionBuilder.Build();

        return this;
    }

	/// <summary>
	/// Sets the board configuration for the Xiangqi game using a <see cref="BoardConfig" /> class instance.
	/// </summary>
	/// <param name="config">The board configuration.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder UseBoardConfig(BoardConfig config)
    {
        _boardConfig = config;
        _useBoardConfig = true;

        return this;
    }

    /// <summary>
	/// Sets the move record for the Xiangqi game.
    /// </summary>
	/// <param name="moveRecord">The move record.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder WithMoveRecord(string moveRecord)
    {
        _moveRecord = moveRecord;

        return this;
    }

	/// <summary>
	/// Sets the board configuration for the Xiangqi game using the APIs from the <see cref="BoardConfig"/> class.
	/// </summary>
	/// <param name="action">An action that configures the board configuration.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
    public XiangqiBuilder UseBoardConfig(Action<BoardConfig> action)
    {
        BoardConfig config = new();

        action.Invoke(config);

        _boardConfig = config;
        _useBoardConfig = true;

        return this;
    }
}