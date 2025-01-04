using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using XiangqiCore.Attributes;
using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
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
		string gameName = "",
		MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)
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
			Board = useBoardConfig ? new Board(boardConfig!) : new Board(initialFenString),
			RoundNumber = FenHelper.GetRoundNumber(initialFenString),
			NumberOfMovesWithoutCapture = FenHelper.GetNumberOfMovesWithoutCapture(initialFenString),
		};

		if (useBoardConfig)
			createdGameInstance.InitialFenString = FenHelper.GetFenFromPosition(createdGameInstance.Board.Position)
				.AppendGameInfoToFen(
					createdGameInstance.SideToMove, 
					createdGameInstance.RoundNumber, 
					createdGameInstance.NumberOfMovesWithoutCapture);

		if (!string.IsNullOrEmpty(moveRecord))
			createdGameInstance.SaveMoveRecordToHistory(moveRecord, moveNotationType);

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
			INotationParser parser = NotationParserFactory.GetParser(moveNotationType);
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
	public string ExportMoveHistory(MoveNotationType targetNotationType = MoveNotationType.TraditionalChinese)
	{
		List<string> movesOfEachRound = [];

		var groupedMoveHitories = _moveHistory
			.Select(moveHistoryItem =>
				new
				{
					moveHistoryItem.RoundNumber,
					moveHistoryItem.MovingSide,
					MoveNotation = moveHistoryItem.TranslateTo(targetNotationType)
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

	private byte[] GeneratePgnFileCore()
	{
		Encoding gb2312Encoding = CodePagesEncodingProvider.Instance.GetEncoding(936) ?? Encoding.UTF8;

		string pgnString = ExportGameAsPgnString();

		return gb2312Encoding.GetBytes(pgnString);
	}

	public void GeneratePgnFile(string filePath)
	{
		string preparedFilePath = PrepareFilePath(filePath, "pgn");
		
		byte[] pgnBytes = GeneratePgnFileCore();

		using FileStream fileStream = new(preparedFilePath, FileMode.Create, FileAccess.Write);
		using StreamWriter streamWriter = new(fileStream);

		fileStream.Write(pgnBytes);
	}

	public async Task GeneratePgnFileAsync(string filePath, CancellationToken cancellationToken = default)
	{
		string preparedFilePath = PrepareFilePath(filePath, "pgn");

		byte[] pgnBytes = GeneratePgnFileCore();

		using FileStream fileStream = new(preparedFilePath, FileMode.Create, FileAccess.Write);
		using StreamWriter streamWriter = new(fileStream);
		
		await fileStream.WriteAsync(pgnBytes, cancellationToken);
	}

	/// <summary>
	/// Shared logic for generating an image
	/// </summary>
	/// <param name="filePath"></param>
	/// <param name="moveCount"></param>
	/// <param name="imageConfig"></param>
	/// <returns></returns>
	private Image<Rgba32> GenerateImageCore(
		string filePath,
		int moveCount = 0,
		ImageConfig? imageConfig = null)
	{
		string targetFen = InitialFenString;

		if (moveCount > 0)
			targetFen = MoveHistory.Skip(Math.Max(moveCount - 1, 0)).First().FenAfterMove;

		Piece[,] position = FenHelper.CreatePositionFromFen(targetFen);

		byte[] bytes = position.GenerateBoardImage(imageConfig);

		return Image.Load<Rgba32>(bytes);
	}

	public void GenerateImage(string filePath, int moveCount = 0, ImageConfig? config = null)
	{
		string preparedFilePath = PrepareFilePath(filePath, "jpg");

		using Image<Rgba32> image = GenerateImageCore(filePath, moveCount, config);

		image.Save(preparedFilePath);
	}

	public async Task GenerateImageAsync(
		string filePath, 
		int moveCount = 0,
		ImageConfig? config = null,
		CancellationToken cancellationToken = default)
	{
		string preparedFilePath = PrepareFilePath(filePath, "jpg");

		using Image<Rgba32> image = GenerateImageCore(filePath, moveCount, config);

		await image.SaveAsync(preparedFilePath, cancellationToken);
	}

	/// <summary>
	/// Shared Logic for generating GIF
	/// </summary>
	/// <param name="filePath"></param>
	/// <param name="config"></param>
	/// <param name="frameDelayInSecond"></param>
	/// <returns></returns>
	private Image<Rgba32> GenerateGifCore(string filePath,
		ImageConfig? config = null,
		decimal frameDelayInSecond = 1)
	{
		config ??= new ImageConfig();

		List<string> fens = [InitialFenString, .. MoveHistory.Select(x => x.FenAfterMove)];

		// Note : Will have to dispose the image after use
		Image<Rgba32> gif = new(width: ImageConfig.DefaultBoardWidth, height: ImageConfig.DefaultBoardHeight);
		GifMetadata gifMetaData = gif.Metadata.GetGifMetadata();

		// Infinite loop
		gifMetaData.RepeatCount = 0;

		int frameDelayInCentiSeconds = (int)Math.Ceiling(frameDelayInSecond * 100);

		// Set the delay until the next image is displayed.
		GifFrameMetadata metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
		metadata.FrameDelay = frameDelayInCentiSeconds;

		for (int i = 0; i < fens.Count; i++)
		{
			string fen = fens[i];

			Coordinate? previousPosition = null;
			Coordinate? currentPosition = null;

			// If the move indicator is enabled and it is not the initial FEN
			if (config.UseMoveIndicator && i > 0)
			{
				previousPosition = MoveHistory[i - 1].StartingPosition;
				currentPosition = MoveHistory[i - 1].Destination;
			}

			byte[] imageBytes = FenHelper.CreatePositionFromFen(fen).GenerateBoardImage(
				config,
				previousPosition: previousPosition,
				currentPosition: currentPosition);

			using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);
			var frame = image.Frames.CloneFrame(0);

			// Set the delay until the next image is displayed.
			metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
			metadata.FrameDelay = frameDelayInCentiSeconds;

			gif.Frames.AddFrame(image.Frames.RootFrame);
		}

		return gif;
	}

	public void GenerateGif(string filePath, 
		ImageConfig? config = null,
		decimal frameDelayInSecond = 1)
	{
		string preparedFilePath = PrepareFilePath(filePath, "gif");

		Image<Rgba32> gif = GenerateGifCore(filePath, config, frameDelayInSecond);

		try
		{
			gif.SaveAsGif(preparedFilePath);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			gif.Dispose();
		}
	}

	public async Task GenerateGifAsync(string filePath,
		ImageConfig? config = null,
		decimal frameDelayInSecond = 1, 
		CancellationToken cancellationToken = default)
	{
		string preparedFilePath = PrepareFilePath(filePath, "gif");

		Image<Rgba32> gif = GenerateGifCore(filePath, config, frameDelayInSecond);

		try
		{
			await gif.SaveAsGifAsync(preparedFilePath, cancellationToken);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			gif.Dispose();
		}
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

	private void SaveMoveRecordToHistory(string moveRecord, MoveNotationType moveNotationType)
	{
		List<string> moves = GameRecordParser.Parse(moveRecord);

		foreach (string move in moves)
		{
			bool isSuccessful = MakeMove(move, moveNotationType);

			if (!isSuccessful)
				break;
		}
	}

	private string PrepareFilePath(string filePath, string fileExtension)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fileExtension, nameof(fileExtension));

		if (!Path.IsPathFullyQualified(filePath))
			throw new ArgumentException("The specified file path is not fully qualified.");

		string directoryPath = Path.GetDirectoryName(filePath);
		string fileName = Path.GetFileName(filePath);

		if (string.IsNullOrWhiteSpace(fileName))
			fileName = $"{GameName}.{fileExtension}";
		else
		{
			string providedExtension = Path.GetExtension(fileName);

			if (!string.Equals(providedExtension, $".{fileExtension}", StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException($"The file extension '{providedExtension}' does not match the expected extension '.{fileExtension}'.");
		}

		if (!Directory.Exists(directoryPath))
			Directory.CreateDirectory(directoryPath);

		char[] invalidFileCharacters = Path.GetInvalidFileNameChars();

		string sanitizedFileName = string.Concat(fileName.Select(character =>
		{
			return invalidFileCharacters.Contains(character) ? '_' : character;
		}));

		return Path.Combine(directoryPath, sanitizedFileName);
	}
}
