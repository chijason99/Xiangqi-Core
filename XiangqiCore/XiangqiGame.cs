using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Move;
using XiangqiCore.Move.Move;
using XiangqiCore.Pieces;

namespace XiangqiCore;

public class XiangqiGame
{
    internal XiangqiGame() { }

    private XiangqiGame(string initialFenString,
                         Side sideToMove,
                         Player redPlayer,
                         Player blackPlayer,
                         string competition,
                         DateTime gameDate)
    {
        InitialFenString = initialFenString;
        SideToMove = sideToMove;
        RedPlayer = redPlayer;
        BlackPlayer = blackPlayer;
        Competition = competition;
        GameDate = gameDate;

        CreatedDate = DateTime.Today;
        UpdatedDate = DateTime.Today;
    }
    public string InitialFenString { get; private set; }

    public Side SideToMove { get; private set; }

    public Player RedPlayer { get; private set; }
    public Player BlackPlayer { get; private set; }

    public string Competition { get; private set; }

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

    public static XiangqiGame Create(string initialFenString, Side sideToMove, Player redPlayer, Player blackPlayer,
                                     string competition, DateTime gameDate, bool useBoardConfig = false, BoardConfig? boardConfig = null)
    {
        bool isFenValid = FenHelper.Validate(initialFenString);

        if (!isFenValid) throw new InvalidFenException(initialFenString);

        Side sideToMoveFromFen = FenHelper.GetSideToMoveFromFen(initialFenString);

        XiangqiGame createdGameInstance = new(initialFenString, sideToMoveFromFen, redPlayer, blackPlayer, competition, gameDate)
        {
            Board = useBoardConfig ? new Board(initialFenString, boardConfig!) : new Board(initialFenString),
            RoundNumber = FenHelper.GetRoundNumber(initialFenString),
            NumberOfMovesWithoutCapture = FenHelper.GetNumberOfMovesWithoutCapture(initialFenString),
        };

        if (useBoardConfig)
            createdGameInstance.InitialFenString = FenHelper.GetFenFromPosition(createdGameInstance.Board.Position);

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

    private void IncrementRoundNumberIfNeeded()
    {
        if (SideToMove == Side.Black || RoundNumber == 0)
            RoundNumber++;
    }

    private void IncrementNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture++;

    private void ResetNumberOfMovesWithoutCapture() => NumberOfMovesWithoutCapture = 0;

    private void SwitchSideToMove() => SideToMove = SideToMove.GetOppositeSide();

    private void AddMoveToHistory(MoveHistoryObject moveHistoryObj) => _moveHistory.Add(moveHistoryObj);

    private void UpdateGameInfo(MoveHistoryObject latestMove)
    {
        if (latestMove.IsCapture)
            ResetNumberOfMovesWithoutCapture();
        else
            IncrementNumberOfMovesWithoutCapture();

        latestMove.UpdateFenWithGameInfo(RoundNumber, NumberOfMovesWithoutCapture);

        IncrementRoundNumberIfNeeded();

        SwitchSideToMove();
        AddMoveToHistory(latestMove);
    }
}
