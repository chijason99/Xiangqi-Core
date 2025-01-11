using System.Text.RegularExpressions;
using XiangqiCore.Attributes;
using XiangqiCore.Boards;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move;
using XiangqiCore.Services.ImageGeneration;

namespace XiangqiCore.Game;

/// <summary>
/// Represents a builder class for creating instances of the Xiangqi game.
/// </summary>
public class XiangqiBuilder : IXiangqiBuilder
{
	private const string _defaultStartingPositionFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";
	private const string _emptyStartingPositionFen = "9/9/9/9/9/9/9/9/9/9 w - - 0 1";

	private string _initialFen { get; set; } = _defaultStartingPositionFen;
	private Side _sideToMove { get; set; }

	private Player _redPlayer { get; set; } = new();
	private Player _blackPlayer { get; set; } = new();

	private Competition _competition { get; set; } = new CompetitionBuilder().Build();
	private GameResult _gameResult { get; set; } = GameResult.Unknown;

	private bool _useBoardConfig { get; set; } = false;
	private BoardConfig? _boardConfig { get; set; } = null;

	private string _moveRecord { get; set; } = "";
	private MoveNotationType _moveNotationType { get; set; } = MoveNotationType.TraditionalChinese;

	private string _gameName { get; set; } = "";

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
	public XiangqiBuilder WithDefaultConfiguration()
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
	/// Sets the starting position according to the provided FEN (Forsyth-Edwards Notation).
	/// </summary>
	/// <param name="customFen">The custom FEN string representing the desired game configuration.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
	public XiangqiBuilder WithStartingFen(string customFen)
	{
		_initialFen = customFen;

		return this;
	}

	/// <summary>
	/// Sets the Xiangqi game configuration to an empty board.
	/// </summary>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
	public XiangqiBuilder WithEmptyBoard()
	{
		_initialFen = _emptyStartingPositionFen;

		return this;
	}

	/// <summary>
	/// Builds an instance of the Xiangqi game asynchronously.
	/// </summary>
	/// <returns>An instance of the <see cref="XiangqiGame"/> class.</returns>
	public XiangqiGame Build()
	{
			return XiangqiGame.Create(
			_initialFen,
			_redPlayer,
			_blackPlayer,
			_competition,
			_useBoardConfig,
			_boardConfig,
			_gameResult,
			_moveRecord,
			_gameName,
			_moveNotationType);
	}

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
	public XiangqiBuilder WithBoardConfig(BoardConfig config)
	{
		_boardConfig = config;
		_useBoardConfig = true;

		return this;
	}

	/// <summary>
	/// Sets the move record for the Xiangqi game. The move record should be in the format of the specified <see cref="MoveNotationType"/>.
	/// </summary>
	/// <param name="moveRecord">The move record.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
	public XiangqiBuilder WithMoveRecord(string moveRecord, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)
	{
		_moveRecord = moveRecord;
		_moveNotationType = moveNotationType;

		return this;
	}

	/// <summary>
	/// Sets the board configuration for the Xiangqi game using the APIs from the <see cref="BoardConfig"/> class.
	/// NOTE: calling this method will override the initial FEN string provided by the <see cref="WithStartingFen(string)"/>.
	/// </summary>
	/// <param name="action">An action that configures the board configuration.</param>
	/// <returns>The current instance of the <see cref="XiangqiBuilder"/> class.</returns>
	public XiangqiBuilder WithBoardConfig(Action<BoardConfig> action)
	{
		BoardConfig config = new();

		action.Invoke(config);

		_boardConfig = config;
		_useBoardConfig = true;

		return this;
	}

	public XiangqiBuilder WithGameName(string gameName)
	{
		_gameName = gameName;

		return this;
	}

	/// <summary>
	/// Sets the Xiangqi game configuration using a Dpxq game record. This will by default use the Simplified Chinese move notation to parse the record.
	/// </summary>
	/// <param name="dpxqGameRecord"></param>
	/// <returns></returns>
	[BetaMethod("This method would require more testing because of the lack of standard of the game records on dpxq.com")]
	public XiangqiBuilder WithDpxqGameRecord(string dpxqGameRecord, MoveNotationType moveNotationType = MoveNotationType.SimplifiedChinese)
	{
		ExtractGameInfoFromDpxqRecord(dpxqGameRecord);
		ExtractMoveRecordFromDpxqRecord(dpxqGameRecord);

		_moveNotationType = moveNotationType;

		return this;
	}

	/// <summary>
	/// Randomises the piece position on the board. This will try to randomise the pieces based on the FEN provided, or the boardConfig you have used in the fluent API if fromFen is set to false.
	/// </summary>
	/// <param name="fromFen">Randomise the pieces from the FEN provided</param>
	/// <param name="allowCheck">Allow checks on the new position</param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public XiangqiBuilder RandomisePosition(bool fromFen = true, bool allowCheck = false)
	{
		if (!fromFen && _boardConfig is null)
			throw new InvalidOperationException("The board configuration must be set before randomising the piece position if fromFen is set to false.");

		PieceCounts pieceCounts = fromFen ? FenHelper.ExtractPieceCounts(_initialFen) : _boardConfig!.PieceCounts;

		_boardConfig ??= new();
		_boardConfig.SetPieceCounts(pieceCounts);

		_boardConfig.RandomisePiecePositions(allowCheck);
		_useBoardConfig = true;

		return this;
	}

	/// <summary>
	/// Randomises the piece position on the board with the <see cref="PieceCounts"/> object.
	/// </summary>
	/// <param name="pieceCounts"></param>
	/// <param name="allowCheck"></param>
	/// <returns></returns>
	public XiangqiBuilder RandomisePosition(PieceCounts pieceCounts, bool allowCheck = false)
	{
		ArgumentNullException.ThrowIfNull(pieceCounts, nameof(pieceCounts));

		_boardConfig ??= new();
		_boardConfig.SetPieceCounts(pieceCounts);

		_boardConfig.RandomisePiecePositions(allowCheck);
		_useBoardConfig = true;

		return this;
	}

	private void ExtractGameInfoFromDpxqRecord(string dpxqGameRecord)
	{
		const string competitionNameKey = "赛事";
		const string redTeamKey = "红方团体";
		const string redPlayerNameKey = "红方姓名";
		const string blackTeamKey = "黑方团体";
		const string blackPlayerNameKey = "黑方姓名";
		const string gameDateKey = "日期";
		const string locationKey = "地点";
		const string resultKey = "结果";

		// (?<key>[^:]+): Named capturing group key that matches one or more characters that are not a colon (:)
		// (?<value>.+): Named capturing group value that matches one or more characters
		// ^ and $: Match the start and end of a line, respectively
		Regex gameInfoPattern = new (@"^(?<key>[^:]+): (?<value>.+)$", RegexOptions.Multiline);
		MatchCollection gameInfoMatched = gameInfoPattern.Matches(dpxqGameRecord);

		CompetitionBuilder competitionBuilder = new();

		foreach (Match match in gameInfoMatched)
		{
			string key = match.Groups["key"].Value;
			string value = match.Groups["value"].Value.Trim();

			switch (key)
			{
				case competitionNameKey:
					competitionBuilder.WithName(value);
					break;
				case redTeamKey:
					_redPlayer.Team = value;
					break;
				case redPlayerNameKey:
					_redPlayer.Name = value;
					break;
				case blackTeamKey:
					_blackPlayer.Team = value;
					break;
				case blackPlayerNameKey:
					_blackPlayer.Name = value;
					break;
				case gameDateKey:
					bool successfulParse = DateTime.TryParse(value, out DateTime competiitonDate);
					competitionBuilder.WithGameDate(successfulParse ? competiitonDate : DateTime.Today);
					break;
				case locationKey:
					competitionBuilder.WithLocation(value);
					break;
				case resultKey:
					_gameResult = value switch
					{
						"红胜" or "红方胜" => GameResult.RedWin,
						"黑胜" or "黑方胜" => GameResult.BlackWin,
						"和棋" => GameResult.Draw,
						_ => GameResult.Unknown
					};
					break;
				default:
					continue;
			}
		}

		_competition = competitionBuilder.Build();
	}

	private void ExtractMoveRecordFromDpxqRecord(string dpxqGameRecord)
	{
		// Replace the full width space character with a regular space character
		// Otherwise, the move record will not be parsed correctly
		string sanitizedDpxqGameRecord = Regex.Replace(dpxqGameRecord, @"\u3000", " ");

		// \d+: Matches one or more digits.
		// \.: Matches a literal dot.
		// \s{0,2}: Matches 0 to 2 whitespace characters.
		// .*[\u4e00-\u9fa5]: Lazily matches any character(except for line terminators) as few times as possible, make sure it contains at least one chinese character.
		// (?=\d +\.\s +|$): Positive lookahead to ensure the match is followed by another round number or the end of the string.
		Regex moveRecordPattern = new(@"(\d+\.\s{0,2}.*?[\u4e00-\u9fa5]+.*?(?=\d+\.\s+|$))", RegexOptions.Multiline);
		MatchCollection moveRecordMatches = moveRecordPattern.Matches(sanitizedDpxqGameRecord);

		List<string> validMoves = ValidateMoveRecordByRoundNumber(moveRecordMatches);

		string moveRecord = string.Join("\n\r", validMoves);

		_moveRecord = moveRecord;
	}

	private List<string> ValidateMoveRecordByRoundNumber(MatchCollection moveRecordMatches)
	{
		const int roundNumberModifierForBrokenMoveRecord = 1;
		int expectedRoundNumber = 1;
		List<string> validMoves = [];

		foreach (Match moveRecord in moveRecordMatches)
		{
			int numberOfDigitsOfMove = (int)Math.Floor(Math.Log10(expectedRoundNumber));
			int substringEndIndex = numberOfDigitsOfMove + 1;
			string roundNumberString = moveRecord.Value[..substringEndIndex];

			bool successfulParse = int.TryParse(roundNumberString, out int parsedRoundNumber);

			// If the round number is not the expected round number, but it is the expected round number plus 1, then it is a broken move record
			bool isBrokenMoveRecord = parsedRoundNumber != expectedRoundNumber && 
									  parsedRoundNumber + roundNumberModifierForBrokenMoveRecord == expectedRoundNumber;

			bool isRoundNumberValid = successfulParse && (parsedRoundNumber == expectedRoundNumber || isBrokenMoveRecord);

			if (!isRoundNumberValid)
				continue;

			validMoves.Add(moveRecord.Value);

			if (!isBrokenMoveRecord)
				expectedRoundNumber++;
		}

		return validMoves;
	}
}