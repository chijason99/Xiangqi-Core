﻿using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.Commands;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
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
		string gameName,
		IMoveParsingService moveParsingService)
	{
		InitialFenString = initialFenString;
		SideToMove = sideToMove;
		RedPlayer = redPlayer;
		BlackPlayer = blackPlayer;
		Competition = competition;
		GameResult = result;
		_moveParsingService = moveParsingService;

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
	/// The move command invoker responsible for executing/undoing move commands.
	/// </summary>
	private readonly MoveCommandInvoker _moveCommandInvoker = new();

	private readonly IMoveParsingService _moveParsingService;

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

	private List<MoveHistoryObject> _moveHistory => _moveCommandInvoker.GetMoveHistories();

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
		IMoveParsingService moveParsingService,
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
			gameName,
			moveParsingService)
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
		CoordinateMoveCommand coordinateMoveCommand = new(Board, startingPosition, destination, SideToMove);

		return MakeMove(coordinateMoveCommand);
	}

	/// <summary>
	/// Makes a move on the game board using the specified move notation.
	/// </summary>
	/// <param name="moveNotation">The move notation.</param>
	/// <param name="moveNotationType">The type of move notation.</param>
	/// <param name="moveParsingService">The move parsing service used to create the MoveCommand. By default it would be using the Xiangqi Core default implementation</param>
	/// <returns><c>true</c> if the move is valid and successful; otherwise, <c>false</c>.</returns>
	public bool MakeMove(string moveNotation, MoveNotationType moveNotationType)
	{
		NotationMoveCommand notationMoveCommand = new (
			_moveParsingService, 
			Board, 
			moveNotation,
			SideToMove,
			moveNotationType);

		return MakeMove(notationMoveCommand);
	}

	public bool MakeMove(IMoveCommand moveCommand)
	{
		try
		{
			MoveHistoryObject moveHistoryObject = _moveCommandInvoker.ExecuteCommand(moveCommand);

			UpdateGameInfo(moveHistoryObject);

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool UndoMove(int numberOfMovesToUndo = 1)
	{
		try
		{
			MoveHistoryObject moveHistory = _moveCommandInvoker.UndoCommand(numberOfMovesToUndo);

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private void IncrementRoundNumberIfNeeded()
	{
		if (SideToMove == Side.Red && (MoveHistory.Count > 1 || RoundNumber != 1))
			RoundNumber++;
	}

	private void IncrementNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture++;

	private void ResetNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture = 0;

	private void SwitchSideToMove() => SideToMove = SideToMove.GetOppositeSide();

	private void UpdateGameResult(GameResult result) => GameResult = result;

	private void UpdateGameInfo(MoveHistoryObject latestMove)
	{
		if (latestMove.IsCapture)
			ResetNumberOfMovesWithoutCapture();
		else
			IncrementNumberOfMovesWithoutCapture();

		IncrementRoundNumberIfNeeded();

		latestMove.UpdateFenWithGameInfo(RoundNumber, NumberOfMovesWithoutCapture);

		if (latestMove.IsCheckmate)
			UpdateGameResult(latestMove.MovingSide == Side.Red ? GameResult.RedWin : GameResult.BlackWin);
		else
			SwitchSideToMove();
	}

	private void SaveMoveRecordToHistory(string moveRecord, MoveNotationType moveNotationType)
	{
		List<string> moves = _moveParsingService.ParseGameRecord(moveRecord);

		foreach (string move in moves)
		{
			bool isSuccessful = MakeMove(move, moveNotationType);

			if (!isSuccessful)
				break;
		}
	}
}
