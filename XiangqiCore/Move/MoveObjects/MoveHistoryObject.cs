using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.MoveObject;

public record MoveHistoryObject
{
    public MoveHistoryObject() { }

    public MoveHistoryObject(string fenAfterMove, string fenBeforeMove, bool isCapture, bool isCheck, bool isCheckMate, PieceType pieceMoved, Side side, Coordinate startingPosition, Coordinate destination)
    {
        FenAfterMove = fenAfterMove;
        FenBeforeMove = fenBeforeMove;
        IsCapture = isCapture;
        IsCheck = isCheck;
        IsCheckmate = isCheckMate;
        PieceMoved = pieceMoved;
        MovingSide = side;
        StartingPosition = startingPosition;
        Destination = destination;
    }

    public string FenAfterMove { get; private set; }
    public string FenBeforeMove { get; private set; }

    public string FenAfter { get; private set; }
    public bool IsCapture { get; init; }
    public bool IsCheck { get; init; }
    public bool IsCheckmate { get; init; }
    public PieceType PieceMoved { get; init; }
    public int RoundNumber => FenHelper.GetRoundNumber(FenAfterMove);

    // The Side that made the move
    public Side MovingSide { get; init; }
    public Coordinate StartingPosition { get; init; }
    public Coordinate Destination { get; init; }

    public string MoveNotation { get; private set; }
    public MoveNotationType MoveNotationType { get; private set; }

    public void UpdateFenWithGameInfo(int roundNumber, int numberOfMovesWithoutCapture) => FenAfterMove = FenAfterMove.AppendGameInfoToFen(MovingSide.GetOppositeSide(), roundNumber, numberOfMovesWithoutCapture);

    public void UpdateMoveNotation(string moveNotation, MoveNotationType moveNotationType)
    {
        MoveNotation = moveNotation;
        MoveNotationType = moveNotationType;
    }

    public string TransalateNotation(MoveNotationType targetNotationType)
        => MoveNotationType.TranslateTo(this, targetNotationType);
}
