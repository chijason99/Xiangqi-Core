using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;

public abstract class Piece(Coordinate coordinate, Side side)
{
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        Piece other = (Piece)obj;

        return Coordinate.Equals(other.Coordinate) && Side == other.Side;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Coordinate, Side);
    }

    public Coordinate Coordinate { get; private set; } = coordinate;
    public Side Side { get; init; } = side;
    public abstract IValidationStrategy ValidationStrategy { get; }
    public virtual int[] GetAvailableRows() => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    public virtual int[] GetAvailableColumns() => [1, 2, 3, 4, 5, 6, 7, 8, 9];
    public abstract PieceType PieceType { get; }
    public abstract char FenCharacter { get; }
    public char GetFenCharacter => Side == Side.Red ? char.ToUpper(FenCharacter) : Side == Side.Black ? char.ToLower(FenCharacter) : FenCharacter;

    public virtual Coordinate GetDestinationCoordinateFromNotation(MoveDirection moveDirection, int fourthCharacterInNotation)
    => moveDirection switch
    {
        MoveDirection.Forward => new Coordinate(Coordinate.Column, Coordinate.Row + fourthCharacterInNotation.ConvertStepsBaseOnSide(Side)),
        MoveDirection.Backward => new Coordinate(Coordinate.Column, Coordinate.Row - fourthCharacterInNotation.ConvertStepsBaseOnSide(Side)),
        MoveDirection.Horizontal => new Coordinate(fourthCharacterInNotation.ConvertToColumnBasedOnSide(Side), Coordinate.Row),
        _ => throw new ArgumentException("Invalid move direction"),
    };

    public bool ValidateMove(Piece[,] position, Coordinate startingPoint, Coordinate destination) => ValidationStrategy.IsProposedMoveValid(position, startingPoint, destination);

    public virtual IEnumerable<Coordinate> GeneratePotentialMoves(Piece[,] position)
    {
        foreach (int row in GetAvailableRows())
        {
            foreach (int column in GetAvailableColumns())
            {
				Coordinate destination = new(column, row);

				if (destination.Equals(Coordinate)) continue;

				// Skip if the destination is a King as it is not a valid move to capture a king
				if (position.GetPieceAtPosition(destination) is King) continue;

				if (ValidateMove(position, Coordinate, destination))
					yield return destination;
			}
        }
    }
}
