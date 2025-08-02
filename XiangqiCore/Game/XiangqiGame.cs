using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.Commands;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Services.MoveParsing;

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
	
	public MoveNode CurrentMove => MoveManager.CurrentMove;
	
	/// <summary>
	/// The move manager that handles the moves in the game.
	/// </summary>
	public MoveManager MoveManager { get; private set; }

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
	public string CurrentFen => CurrentMove.MoveHistoryObject.FenAfterMove;

	/// <summary>
	/// Gets the number of moves without capture.
	/// </summary>
	public int NumberOfMovesWithoutCapture { get; private set; } = 0;

	/// <summary>
	/// Gets the round number.
	/// </summary>
	public int RoundNumber { get; private set; } = 0;

	/// <inheritdoc cref="MoveManager.GetMoveHistory(bool, VariationPath?)"/>
	public IReadOnlyList<MoveHistoryObject> GetMoveHistory(
		bool includeRootNode = false, 
		VariationPath? variationsPath = null) 
		=> MoveManager.GetMoveHistory(includeRootNode, variationsPath).AsReadOnly();
	
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
	/// <param name="gameName">The game name</param>
	/// <param name="moveNotationType">The move notation type</param>
	/// <returns>A new instance of the <see cref="XiangqiGame"/> class.</returns>
	internal static XiangqiGame Create(
		string initialFenString,
		Player redPlayer,
		Player blackPlayer,
		Competition competition,
		bool useBoardConfig = false,
		BoardConfig? boardConfig = null,
		GameResult gameResult = GameResult.Unknown,
		List<string>? moveRecord = null,
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

		// Set up the MoveHistoryObject for the root move
		// NOTE: place this after the board is created, so that the initial fen string is correct if the user is 
		// randomizing the board.
		MoveHistoryObject rootMoveHistory = new(
			fenAfterMove: createdGameInstance.InitialFenString, 
			fenBeforeMove: createdGameInstance.InitialFenString,
			isCapture: false,
			isCheck: false,
			isCheckMate: gameResult != GameResult.Unknown && gameResult != GameResult.Draw,
			pieceMoved: PieceType.None,
			pieceCaptured: PieceType.None,
			side: sideToMoveFromFen,
			startingPosition: Coordinate.Empty,
			destination: Coordinate.Empty,
			pieceOrder: PieceOrder.Unknown,
			hasMultiplePieceOfSameTypeOnSameColumn: false);
		
		MoveCommandInvoker commandInvoker = new(createdGameInstance.Board);
		createdGameInstance.MoveManager = new MoveManager(commandInvoker, rootMoveHistory, createdGameInstance.Board);
		
		if (moveRecord is not null)
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
		CoordinateMoveCommand coordinateMoveCommand = new(startingPosition, destination, SideToMove);

		return MakeMove(coordinateMoveCommand);
	}

	/// <summary>
	/// Makes a move on the game board using the specified move notation.
	/// </summary>
	/// <param name="moveNotation">The move notation.</param>
	/// <param name="moveNotationType">The type of move notation.</param>
	/// <returns><c>true</c> if the move is valid and successful; otherwise, <c>false</c>.</returns>
	public bool MakeMove(string moveNotation, MoveNotationType moveNotationType)
	{
		NotationMoveCommand notationMoveCommand = new (
			moveNotation,
			SideToMove,
			moveNotationType);

		return MakeMove(notationMoveCommand);
	}

	public bool MakeMove(IMoveCommand moveCommand)
	{
		try
		{
			MoveHistoryObject latestMove = MoveManager.AddMove(moveCommand);

			UpdateGameInfoAfterMove(latestMove);

			latestMove.UpdateFenWithGameInfo(RoundNumber, NumberOfMovesWithoutCapture);

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	/// <inheritdoc cref="MoveManager.DeleteSubsequentMoves"/>
	public bool DeleteSubsequentMoves()
	{
		try
		{
			MoveManager.DeleteSubsequentMoves();

			UpdateGameInfo(resetGameResult: true);

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private void IncrementRoundNumberIfNeeded()
	{
		if (SideToMove == Side.Red && (GetMoveHistory().Count > 1 || RoundNumber != 1))
			RoundNumber++;
	}

	private void IncrementNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture++;

	private void ResetNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture = 0;

	private void SwitchSideToMove() => SideToMove = SideToMove.GetOppositeSide();

	private void UpdateGameResult(GameResult result) => GameResult = result;

	/// <summary>
	/// Updates the game information after a move is made.
	/// </summary>
	private void UpdateGameInfoAfterMove(MoveHistoryObject latestMove)
	{
		if (latestMove.IsCapture)
			ResetNumberOfMovesWithoutCapture();
		else
			IncrementNumberOfMovesWithoutCapture();

		IncrementRoundNumberIfNeeded();

		if (latestMove.IsCheckmate)
			UpdateGameResult(latestMove.MovingSide == Side.Red ? GameResult.RedWin : GameResult.BlackWin);
		else
			SwitchSideToMove();
	}

	/// <summary>
	/// Updates the game information according to the CurrentMove.
	/// </summary>
	private void UpdateGameInfo(bool resetGameResult = false)
	{
		RoundNumber = FenHelper.GetRoundNumber(CurrentFen);
		NumberOfMovesWithoutCapture = FenHelper.GetNumberOfMovesWithoutCapture(CurrentFen);
		SideToMove = FenHelper.GetSideToMoveFromFen(CurrentFen);
		
		if (resetGameResult)
			UpdateGameResult(GameResult.Unknown);
	}

	private void SaveMoveRecordToHistory(List<string> moves, MoveNotationType moveNotationType)
	{
		foreach (string move in moves)
		{
			bool isSuccessful = MakeMove(move, moveNotationType);

			if (!isSuccessful)
				break;
		}
	}

	/// <summary>
	/// Navigate to a specific move in the game history.
	/// </summary>
	/// <param name="targetMoveNode">The move node to navigate to.</param>
	public void NavigateToMove(MoveNode targetMoveNode)
	{
		if (targetMoveNode is null)
			throw new ArgumentNullException(nameof(targetMoveNode), "Target move node cannot be null.");
		
		MoveManager.NavigateToMove(targetMoveNode);
		
		UpdateGameInfo();
	}
	
	/// <summary>
	/// Navigate to a specific move in the game history.
	/// </summary>
	/// <param name="moveNumber">The move number to navigate to (For the starting move, this would be 0).</param>
	/// <param name="variationsPath">
	/// <see cref="VariationPath"/>
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if moveNumber or any variation path values are invalid.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the requested path does not exist.</exception>
	public void NavigateToMove(int moveNumber, VariationPath? variationsPath = null)
	{
		if (moveNumber < 0)
			throw new ArgumentOutOfRangeException(
				nameof(moveNumber), 
				"Move number must be non-negative.");
		
		if (variationsPath is not null && variationsPath.Any(kvp => kvp.Key < 0 || kvp.Value < 0))
			throw new ArgumentOutOfRangeException(
				nameof(variationsPath), 
				"Variation number must be non-negative.");
		
		MoveManager.NavigateToMove(moveNumber, variationsPath);
		
		UpdateGameInfo();
	}

	/// <summary>
	/// A shortcut to navigate to the start of the game.
	/// </summary>
	public void NavigateToStart()
	{
		NavigateToMove(MoveManager.RootMove);
	}
	
	/// <summary>
	/// A shortcut to navigate to the previous move.
	/// </summary>
	public void NavigateToPreviousMove()
	{
		NavigateToMove(CurrentMove.Parent ?? throw new InvalidOperationException("No previous move available."));
	}
	
	/// <summary>
	/// A shortcut to navigate to the next move in the current variation.
	/// </summary>
	/// <param name="variationNumber">The variation number to navigate to (default is 0).</param>
	/// <exception cref="InvalidOperationException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public void NavigateToNextMove(int variationNumber = 0)
	{
		if (variationNumber < 0)
			throw new ArgumentOutOfRangeException(nameof(variationNumber), "Variation number must be non-negative.");
		
		if (CurrentMove.Variations.Count == 0 || variationNumber >= CurrentMove.Variations.Count)
			throw new InvalidOperationException(
				$"{nameof(variationNumber)} is invalid");
		
		NavigateToMove(CurrentMove.Variations.ElementAt(variationNumber));
	}

	/// <summary>
	/// A shortcut to navigate to the end of the game.
	/// </summary>
	/// <param name="variationsPath">
	/// <see cref="VariationPath"/>
	/// </param>
	public void NavigateToEnd(VariationPath? variationsPath = null)
	{
		if (variationsPath is not null && variationsPath.Any(kvp => kvp.Key < 0 || kvp.Value < 0))
			throw new ArgumentOutOfRangeException(
				nameof(variationsPath), 
				"Variation number must be non-negative.");

		var lastMove = MoveManager.GetLastMove(variationsPath);
		
		NavigateToMove(lastMove);
	}
}