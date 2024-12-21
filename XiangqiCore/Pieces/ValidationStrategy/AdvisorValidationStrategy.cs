using XiangqiCore.Boards;
using XiangqiCore.Exceptions;
using XiangqiCore.Misc;

namespace XiangqiCore.Pieces.ValidationStrategy;
public class AdvisorValidationStrategy : DefaultValidationStrategy, IHasSpecificPositions
{
    public override int[] GetPossibleColumns() => Board.GetPalaceColumns();

    public override int[] GetPossibleRows(Side color) => Board.GetPalaceRows(color);

	public Coordinate[] GetSpecificPositions(Side side)
	{
		if (side == Side.None)
			throw new InvalidSideException(side);

		if (side == Side.Red)
		{
			return
			[
				new Coordinate(4, 1),
				new Coordinate(4, 3),
				new Coordinate(5, 2),
				new Coordinate(6, 1),
				new Coordinate(6, 3)
			];
		}
		else
		{
			return
			[
				new Coordinate(4, 10),
				new Coordinate(4, 8),
				new Coordinate(5, 9),
				new Coordinate(6, 10),
				new Coordinate(6, 8)
			];
		}
	}

	public override bool ValidateMoveLogicForPiece(Piece[,] boardPosition, Coordinate startingPosition, Coordinate destination)
    {
        bool isMovingUp = destination.Row == startingPosition.Row + 1 ;
        bool isMovingDown = destination.Row == startingPosition.Row - 1 ;
        bool isMovingLeft = destination.Column == startingPosition.Column - 1 ;
        bool isMovingRight = destination.Column == startingPosition.Column + 1 ;

        bool isMovingTopLeft = isMovingUp && isMovingLeft;
        bool isMovingTopRight = isMovingUp && isMovingRight;
        bool isMovingBottomLeft = isMovingDown && isMovingLeft;
        bool isMovingBottomRight = isMovingDown && isMovingRight;

        return isMovingTopLeft || isMovingTopRight || isMovingBottomLeft || isMovingBottomRight;
    }

	public override bool AreCoordinatesValid(Side color, Coordinate destination) => GetSpecificPositions(color).Contains(destination);
}
