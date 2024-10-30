using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.Versioning;
using System.Text;
using XiangqiCore.Attributes;
using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Move.NotationParsers;
using XiangqiCore.Pieces;

namespace XiangqiCore.Game;

/// <summary>
/// Represents a game of Xiangqi.
/// </summary>
public class XiangqiGame
{
	internal XiangqiGame() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="XiangqiGame"/> class.
	/// </summary>
	/// <param name="initialFenString">The initial FEN string representing the board position.</param>
	/// <param name="sideToMove">The side to move.</param>
	/// <param name="redPlayer">The red player.</param>
	/// <param name="blackPlayer">The black player.</param>
	/// <param name="competition">The competition.</param>
	/// <param name="result">The game result.</param>
	internal XiangqiGame(
		string initialFenString,
		Side sideToMove,
		Player redPlayer,
		Player blackPlayer,
		Competition competition,
		GameResult result,
		string gameName)
	{
		InitialFenString = initialFenString;
		SideToMove = sideToMove;
		RedPlayer = redPlayer;
		BlackPlayer = blackPlayer;
		Competition = competition;
		GameResult = result;

		if (string.IsNullOrWhiteSpace(gameName))
		{
			string gameResultChinese = GameResult switch
			{
				GameResult.RedWin => "先勝",
				GameResult.BlackWin => "先負",
				GameResult.Draw => "先和",
				_ => "對"
			};

			GameName = $"{RedPlayer.Name}{gameResultChinese}{BlackPlayer.Name}";
		}
		else
		{
			GameName = gameName;
		}
	}

	/// <summary>
	/// Gets the initial FEN string when the game is first created.
	/// </summary>
	public string InitialFenString { get; private set; }

	/// <summary>
	/// Gets the side to move.
	/// </summary>
	public Side SideToMove { get; private set; }

	/// <summary>
	/// Gets the red player.
	/// </summary>
	public Player RedPlayer { get; private set; }

	/// <summary>
	/// Gets the black player.
	/// </summary>
	public Player BlackPlayer { get; private set; }

	/// <summary>
	/// Gets the competition details of the game.
	/// </summary>
	public Competition Competition { get; private set; }

	/// <summary>
	/// Gets the name of the game.
	/// </summary>
	public string GameName { get; private set; }

	/// <summary>
	/// Gets the game date.
	/// </summary>
	public DateTime? GameDate => Competition.GameDate;

	/// <summary>
	/// Gets the game board.
	/// </summary>
	public Board Board { get; private set; }

	/// <summary>
	/// Gets the board position.
	/// </summary>
	public Piece[,] BoardPosition => Board.Position;

	/// <summary>
	/// Gets the current FEN string representing the board position.
	/// </summary>
	public string CurrentFen => _moveHistory.LastOrDefault()?.FenAfterMove ?? InitialFenString;

	/// <summary>
	/// Gets the number of moves without capture.
	/// </summary>
	public int NumberOfMovesWithoutCapture { get; private set; } = 0;

	/// <summary>
	/// Gets the round number.
	/// </summary>
	public int RoundNumber { get; private set; } = 0;

	private List<MoveHistoryObject> _moveHistory { get; set; } = [];

	/// <summary>
	/// Gets the move history.
	/// </summary>
	public IReadOnlyList<MoveHistoryObject> MoveHistory => _moveHistory.AsReadOnly();

	/// <summary>
	/// Gets the game result.
	/// </summary>
	public GameResult GameResult { get; private set; } = GameResult.Unknown;

	/// <summary>
	/// Gets the game result string.
	/// </summary>
	public string GameResultString => EnumHelper<GameResult>.GetDisplayName(GameResult);

	/// <summary>
	/// Creates a new instance of the <see cref="XiangqiGame"/> class.
	/// </summary>
	/// <param name="initialFenString">The initial FEN string representing the board position.</param>
	/// <param name="redPlayer">The red player.</param>
	/// <param name="blackPlayer">The black player.</param>
	/// <param name="competition">The competition.</param>
	/// <param name="useBoardConfig">A flag indicating whether to use a custom board configuration.</param>
	/// <param name="boardConfig">The custom board configuration.</param>
	/// <param name="gameResult">The game result.</param>
	/// <param name="moveRecord">The move record.</param>
	/// <returns>A new instance of the <see cref="XiangqiGame"/> class.</returns>
	internal static XiangqiGame Create(
		string initialFenString,
		Player redPlayer,
		Player blackPlayer,
		Competition competition,
		bool useBoardConfig = false,
		BoardConfig? boardConfig = null,
		GameResult gameResult = GameResult.Unknown,
		string moveRecord = "",
		string gameName = "")
	{
		bool isFenValid = FenHelper.Validate(initialFenString);

		if (!isFenValid) throw new InvalidFenException(initialFenString);

		Side sideToMoveFromFen = FenHelper.GetSideToMoveFromFen(initialFenString);

		XiangqiGame createdGameInstance = new(
			initialFenString,
			sideToMoveFromFen,
			redPlayer,
			blackPlayer,
			competition,
			gameResult,
			gameName)
		{
			Board = useBoardConfig ? new Board(initialFenString, boardConfig!) : new Board(initialFenString),
			RoundNumber = FenHelper.GetRoundNumber(initialFenString),
			NumberOfMovesWithoutCapture = FenHelper.GetNumberOfMovesWithoutCapture(initialFenString),
		};

		if (useBoardConfig)
			createdGameInstance.InitialFenString = FenHelper.GetFenFromPosition(createdGameInstance.Board.Position);

		if (!string.IsNullOrEmpty(moveRecord))
			createdGameInstance.SaveMoveRecordToHistory(moveRecord);

		return createdGameInstance;
	}

	/// <summary>
	/// Makes a move on the game board.
	/// </summary>
	/// <param name="startingPosition">The starting position of the move.</param>
	/// <param name="destination">The destination position of the move.</param>
	/// <returns><c>true</c> if the move is valid and successful; otherwise, <c>false</c>.</returns>
	public bool MakeMove(Coordinate startingPosition, Coordinate destination)
	{
		try
		{
			MoveHistoryObject moveHistoryObject = Board.MakeMove(startingPosition, destination, SideToMove);

			UpdateGameInfo(moveHistoryObject);

			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	/// <summary>
	/// Makes a move on the game board using the specified move notation.
	/// </summary>
	/// <param name="moveNotation">The move notation.</param>
	/// <param name="moveNotationType">The type of move notation.</param>
	/// <returns><c>true</c> if the move is valid and successful; otherwise, <c>false</c>.</returns>
	public bool MakeMove(string moveNotation, MoveNotationType moveNotationType)
	{
		try
		{
			IMoveNotationParser parser = MoveNotationParserFactory.GetParser(moveNotationType);
			ParsedMoveObject parsedMoveObject = parser.Parse(moveNotation);

			MoveHistoryObject moveHistoryObject = Board.MakeMove(parsedMoveObject, SideToMove);
			moveHistoryObject.UpdateMoveNotation(moveNotation, moveNotationType);

			UpdateGameInfo(moveHistoryObject);

			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	/// <summary>
	/// Exports the move history in the specified notation type.
	/// </summary>
	/// <param name="targetNotationType">The target notation type.</param>
	/// <returns>The move history in the specified notation type.</returns>
	[BetaMethod("Currently only supports converting MoveNotationType from Chinese/English to UCCI. The translation would not work for Chinese -> English or English -> Chinese")]
	public string ExportMoveHistory(MoveNotationType targetNotationType = MoveNotationType.Chinese)
	{
		List<string> movesOfEachRound = [];

		var groupedMoveHitories = _moveHistory
			.Select(moveHistoryItem =>
				new
				{
					moveHistoryItem.RoundNumber,
					moveHistoryItem.MovingSide,
					MoveNotation = moveHistoryItem.TransalateNotation(targetNotationType)
				})
			.GroupBy(moveHistoryItem => moveHistoryItem.RoundNumber)
			.OrderBy(roundGroup => roundGroup.Key);

		foreach (var roundGroup in groupedMoveHitories)
		{
			StringBuilder roundMoves = new();

			string? moveNotationFromRed = roundGroup.SingleOrDefault(move => move.MovingSide == Side.Red)?.MoveNotation ?? "...";
			string? moveNotationFromBlack = roundGroup.SingleOrDefault(move => move.MovingSide == Side.Black)?.MoveNotation;

			roundMoves.Append($"{roundGroup.Key}. {moveNotationFromRed}  {moveNotationFromBlack}");

			movesOfEachRound.Add(roundMoves.ToString());
		};

		return string.Join("\n", movesOfEachRound);
	}

	/// <summary>
	/// Exports the game as PGN (Portable Game Notation) format.
	/// </summary>
	/// <returns>The PGN string of the game</returns>
	public string ExportGameAsPgnString()
	{
		StringBuilder pgnBuilder = new();

		AddPgnTag(pgnBuilder, PgnTagType.Game, "Chinese Chess");
		AddPgnTag(pgnBuilder, PgnTagType.Event, Competition.Name);
		AddPgnTag(pgnBuilder, PgnTagType.Site, Competition.Location);
		AddPgnTag(pgnBuilder, PgnTagType.Date, Competition.GameDate?.ToString("yyyy.MM.dd") ?? string.Empty);
		AddPgnTag(pgnBuilder, PgnTagType.Red, RedPlayer.Name);
		AddPgnTag(pgnBuilder, PgnTagType.RedTeam, RedPlayer.Team);
		AddPgnTag(pgnBuilder, PgnTagType.Black, BlackPlayer.Name);
		AddPgnTag(pgnBuilder, PgnTagType.BlackTeam, BlackPlayer.Team);
		AddPgnTag(pgnBuilder, PgnTagType.Result, GameResultString);
		AddPgnTag(pgnBuilder, PgnTagType.FEN, InitialFenString);

		pgnBuilder.AppendLine(ExportMoveHistory());

		return pgnBuilder.ToString();
	}

	public async Task ExportGameAsPgnFileAsync(string filePath)
	{
		if (!Path.IsPathFullyQualified(filePath) || !Path.Exists(filePath))
			throw new ArgumentException("The specified file path does not exist.");

		char[] invalidFileCharacters = Path.GetInvalidFileNameChars();

		string pgnString = ExportGameAsPgnString();
		string sanitizedFileName = string.Concat($"{GameName}.pgn".Select(character =>
		{
			return invalidFileCharacters.Contains(character) ? '_' : character;
		}));

		string sanitizedFilePath = Path.Combine(filePath, sanitizedFileName);

		using FileStream fileStream = new(sanitizedFilePath, FileMode.Create, FileAccess.Write);
		using StreamWriter streamWriter = new(fileStream);
		
		await fileStream.WriteAsync(Encoding.UTF8.GetBytes(pgnString));
	}

	[SupportedOSPlatform("windows6.1")]
	public void GenerateImage(string filePath, int moveCount = 0, bool flipHorizontal = false, bool flipVertical = false)
	{
		string targetFen = InitialFenString;

		if (moveCount > 0)
			targetFen = MoveHistory.Skip(Math.Max(moveCount - 1, 0)).First().FenAfterMove;

		Piece[,] position = FenHelper.CreatePositionFromFen(targetFen);

		byte[] bytes = position.GenerateBoardImage(flipHorizontal, flipVertical);

		File.WriteAllBytes(filePath, bytes);
	}

	[SupportedOSPlatform("windows6.1")]
	public void GenerateGIF(string filePath, 
		bool flipHorizontal = false, 
		bool flipVertical = false,
		int width = 450,
		int height = 500,
		int frameDelay = 100)
	{
		List<string> fens = [InitialFenString, ..MoveHistory.Select(x => x.FenAfterMove)];

		using Image<Rgba32> gif = new(width, height);
		GifMetadata gifMetaData = gif.Metadata.GetGifMetadata();

		// Infinite loop
		gifMetaData.RepeatCount = 0;

		// Set the delay until the next image is displayed.
		GifFrameMetadata metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
		metadata.FrameDelay = frameDelay;

		foreach (string fen in fens)
		{
			byte[] imageBytes = FenHelper.CreatePositionFromFen(fen).GenerateBoardImage(flipHorizontal, flipVertical);

			using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(imageBytes);
			var frame = image.Frames.CloneFrame(0);

			// Set the delay until the next image is displayed.
			metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
			metadata.FrameDelay = frameDelay;

			gif.Frames.AddFrame(image.Frames.RootFrame);
		}

		gif.SaveAsGif(filePath);
	}

	private void AddPgnTag(StringBuilder pgnBuilder, PgnTagType pgnTagKey, string pgnTagValue)
	{
		string pgnTagDisplayName = EnumHelper<PgnTagType>.GetDisplayName(pgnTagKey);
		pgnBuilder.AppendLine($"[{pgnTagDisplayName} \"{pgnTagValue}\"]");
	}

	private void IncrementRoundNumberIfNeeded()
	{
		if (SideToMove == Side.Red && (MoveHistory.Count != 0 || RoundNumber != 1))
			RoundNumber++;
	}

	private void IncrementNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture++;

	private void ResetNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture = 0;

	private void SwitchSideToMove() => SideToMove = SideToMove.GetOppositeSide();

	private void AddMoveToHistory(MoveHistoryObject moveHistoryObj) => _moveHistory.Add(moveHistoryObj);

	private void UpdateGameResult(GameResult result) => GameResult = result;

	private void UpdateGameInfo(MoveHistoryObject latestMove)
	{
		if (latestMove.IsCapture)
			ResetNumberOfMovesWithoutCapture();
		else
			IncrementNumberOfMovesWithoutCapture();

		IncrementRoundNumberIfNeeded();

		latestMove.UpdateFenWithGameInfo(RoundNumber, NumberOfMovesWithoutCapture);

		AddMoveToHistory(latestMove);

		if (latestMove.IsCheckmate)
			UpdateGameResult(latestMove.MovingSide == Side.Red ? GameResult.RedWin : GameResult.BlackWin);
		else
			SwitchSideToMove();
	}

	private void SaveMoveRecordToHistory(string moveRecord)
	{
		List<string> moves = GameRecordParser.Parse(moveRecord);

		foreach (string move in moves)
		{
			bool isSuccessful = MakeMove(move, MoveNotationType.Chinese);

			if (!isSuccessful)
				break;
		}
	}
}
