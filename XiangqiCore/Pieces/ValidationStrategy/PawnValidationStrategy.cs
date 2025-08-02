using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Extension;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class PawnValidationStrategy : DefaultValidationStrategy, IValidationStrategy
{
    public static readonly int[] AvailableColumnsForPawnsNotYetCrossedTheRiver = [1, 3, 5, 7, 9];

    public static int[] AvailableRowsForPawnsNotYetCrossedTheRiver(Side color)
	    => color switch
	    {
		    Side.Red => [4, 5],
		    Side.Black => [6, 7],
		    _ => throw new InvalidSideException(color)
	    };

    public override int[] GetPossibleRows(Side color)
    {
        int[] unavailableRowsForRedPawn = [1, 2, 3];
        int[] unavailableRowsForBlackPawn = [8, 9, 10];

        return color switch
                {
                    Side.Red => Board.GetAllRows().Except(unavailableRowsForRedPawn).ToArray(),
                    Side.Black => Board.GetAllRows().Except(unavailableRowsForBlackPawn).ToArray(),
                    _ => throw new InvalidSideException(color)
                };
    }

	public override bool AreCoordinatesValid(Side color, Coordinate destination)
	{
		bool hasCrossedTheRiver = color == Side.Red ? destination.Row >= 6 : destination.Row <= 5;

		int[] availableColumns = hasCrossedTheRiver ? GetPossibleColumns() : AvailableColumnsForPawnsNotYetCrossedTheRiver;
        
        return availableColumns.Contains(destination.Column) && GetPossibleRows(color).Contains(destination.Row);
	}

	public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        Pawn pawn = (Pawn)boardPosition.GetPieceAtPosition(startingPosition);
        const int redRiverRow = 5;
        const int blackRiverRow = 6;

        bool isOnTheSameColumn = startingPosition.Column == destination.Column;
        bool isOnTheSameRow = startingPosition.Row == destination.Row;

        bool isMovingForward = pawn.Side == Side.Red ? 
                               startingPosition.Row + 1 == destination.Row && isOnTheSameColumn : 
                               startingPosition.Row - 1 == destination.Row && isOnTheSameColumn;

        bool isMovingLeft = startingPosition.Column - 1 == destination.Column && isOnTheSameRow;
        bool isMovingRight = startingPosition.Column + 1 == destination.Column && isOnTheSameRow;
        bool pawnHasCrossedTheRiver = pawn.Side == Side.Red ? startingPosition.Row >= blackRiverRow : startingPosition.Row <= redRiverRow;

        return (isMovingForward && !(isMovingLeft || isMovingRight)) || (pawnHasCrossedTheRiver && ((isMovingLeft || isMovingRight) && !isMovingForward));
    }

	public Coordinate[] GetValidCoordinates(Side side)
	{
		if (side == Side.None)
			throw new InvalidSideException(side);

		var rowsForPawnsNotYetCrossedTheRiver = AvailableRowsForPawnsNotYetCrossedTheRiver(side);
		var availableColumnsForPawnsNotYetCrossedTheRiver = AvailableColumnsForPawnsNotYetCrossedTheRiver;
		var rowsForPawnsCrossedTheRiver = GetPossibleRows(side).Except(rowsForPawnsNotYetCrossedTheRiver).ToArray();
		
		List<Coordinate> validCoordinates = [];

		foreach (int row in rowsForPawnsNotYetCrossedTheRiver)
		{
			validCoordinates.AddRange(availableColumnsForPawnsNotYetCrossedTheRiver
				.Select(column => new Coordinate(column, row)));
		}

		foreach (int row in rowsForPawnsCrossedTheRiver)
		{
			validCoordinates.AddRange(GetPossibleColumns()
				.Select(column => new Coordinate(column, row)));
		}
		
		return validCoordinates.ToArray();
	}
}
