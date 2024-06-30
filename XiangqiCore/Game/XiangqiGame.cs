using LinqKit;
using System.Text;
using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Pieces;

namespace XiangqiCore.Game;

public class XiangqiGame
{
    internal XiangqiGame() { }

    private XiangqiGame(string initialFenString,
                         Side sideToMove,
                         Player redPlayer,
                         Player blackPlayer,
                         Competition competition,
                         GameResult result)
    {
        InitialFenString = initialFenString;
        SideToMove = sideToMove;
        RedPlayer = redPlayer;
        BlackPlayer = blackPlayer;
        Competition = competition;
        GameResult = result;

        CreatedDate = DateTime.Today;
        UpdatedDate = DateTime.Today;
    }
    public string InitialFenString { get; private set; }

    public Side SideToMove { get; private set; }

    public Player RedPlayer { get; private set; }
    public Player BlackPlayer { get; private set; }

    public Competition Competition { get; private set; }

    public DateTime GameDate { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }

    public Board Board { get; private set; }

    public Piece[,] BoardPosition => Board.Position;

    public string CurrentFen => _moveHistory.LastOrDefault()?.FenOfPosition ?? "";

    public int NumberOfMovesWithoutCapture { get; private set; } = 0;
    public int RoundNumber { get; private set; } = 0;

    private List<MoveHistoryObject> _moveHistory { get; set; } = [];
    public IReadOnlyList<MoveHistoryObject> MoveHistory => _moveHistory.AsReadOnly();

    public GameResult GameResult { get; private set; } = GameResult.Unknown;
    public string GameResultString => EnumHelper<GameResult>.GetDisplayName(GameResult);

    public static XiangqiGame Create(string initialFenString, Player redPlayer, Player blackPlayer,
                                     Competition competition, bool useBoardConfig = false, BoardConfig? boardConfig = null, 
                                     GameResult gameResult = GameResult.Unknown, string moveRecord = "")
    {
        bool isFenValid = FenHelper.Validate(initialFenString);

        if (!isFenValid) throw new InvalidFenException(initialFenString);

        Side sideToMoveFromFen = FenHelper.GetSideToMoveFromFen(initialFenString);

        XiangqiGame createdGameInstance = new(initialFenString, sideToMoveFromFen, redPlayer, blackPlayer, competition, gameResult)
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

    public bool Move(Coordinate startingPosition, Coordinate destination)
    {
        try
        {
            MoveHistoryObject moveHistoryObject = Board.MakeMove(startingPosition, destination, SideToMove);

            UpdateGameInfo(moveHistoryObject);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid move: {ex.Message}");
            return false;
        }
    }

    public bool Move(string moveNotation, MoveNotationType moveNotationType)
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
            Console.WriteLine($"Invalid move: {ex.Message}");
            return false;
        }
    }

    public string ExportMoveHistory()
    {
        List<string> movesOfEachRound = [];

        _moveHistory
            .Select(moveHistoryItem => new { moveHistoryItem.RoundNumber, moveHistoryItem.MovingSide, moveHistoryItem.MoveNotation })
            .GroupBy(moveHistoryItem => moveHistoryItem.RoundNumber)
            .OrderBy(roundGroup => roundGroup.Key)
            .ForEach(roundGroup =>
            {
                StringBuilder roundMoves = new();

                string? moveNotationFromRed = roundGroup.SingleOrDefault(move => move.MovingSide == Side.Red)?.MoveNotation ?? "...";
                string? moveNotationFromBlack = roundGroup.SingleOrDefault(move => move.MovingSide == Side.Black)?.MoveNotation;

                roundMoves.Append($"{roundGroup.Key}. {moveNotationFromRed}  {moveNotationFromBlack}");

                movesOfEachRound.Add(roundMoves.ToString());
            });

        return string.Join("\n", movesOfEachRound);
    }

    public string ExportGameAsPgn()
    {
        StringBuilder pgnBuilder = new();

        AddPgnTag(pgnBuilder, PgnTagType.Game, "Chinese Chess");
        AddPgnTag(pgnBuilder, PgnTagType.Event, Competition.Name);
        AddPgnTag(pgnBuilder, PgnTagType.Site, Competition.Location);
        AddPgnTag(pgnBuilder, PgnTagType.Date, Competition.GameDate.ToString("yyyy.MM.dd"));
        AddPgnTag(pgnBuilder, PgnTagType.Red, RedPlayer.Name);
        AddPgnTag(pgnBuilder, PgnTagType.RedTeam, RedPlayer.Team);
        AddPgnTag(pgnBuilder, PgnTagType.Black, BlackPlayer.Name);
        AddPgnTag(pgnBuilder, PgnTagType.BlackTeam, BlackPlayer.Team);
        AddPgnTag(pgnBuilder, PgnTagType.Result, GameResultString);
        AddPgnTag(pgnBuilder, PgnTagType.FEN, InitialFenString);
        
        pgnBuilder.AppendLine(ExportMoveHistory());

        return pgnBuilder.ToString();
    }

    private void AddPgnTag(StringBuilder pgnBuilder, PgnTagType pgnTagKey, string pgnTagValue)
    {
        string pgnTagDisplayName = EnumHelper<PgnTagType>.GetDisplayName(pgnTagKey);
        pgnBuilder.AppendLine($"[{pgnTagDisplayName} \"{pgnTagValue}\"]");
    }

    private void IncrementRoundNumberIfNeeded()
    {
        if (SideToMove == Side.Red || RoundNumber == 0)
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
            Move(move, MoveNotationType.Chinese);
    }
}
