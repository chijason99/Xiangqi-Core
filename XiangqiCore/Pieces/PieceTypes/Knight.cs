using XiangqiCore.Attributes;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;

[MoveInDiagonals]
public sealed class Knight : Piece
{
    public Knight(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new KnightValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }
    public override PieceType PieceType => PieceType.Knight;
    public override char FenCharacter => 'n';

    public override Coordinate GetDestinationCoordinateFromNotation(MoveDirection moveDirection, int fourthCharacterInNotation)
    {
        int actualTargetColumn = fourthCharacterInNotation.ConvertToColumnBasedOnSide(Side);
        bool isMovingTwoRows = Math.Abs(Coordinate.Column - actualTargetColumn) == 1; 

        if(moveDirection == MoveDirection.Forward)
            return isMovingTwoRows ? new Coordinate(actualTargetColumn, Coordinate.Row + 2.ConvertStepsBaseOnSide(Side)) : 
                                     new Coordinate(actualTargetColumn, Coordinate.Row + 1.ConvertStepsBaseOnSide(Side));
        else
            return isMovingTwoRows ? new Coordinate(actualTargetColumn, Coordinate.Row - 2.ConvertStepsBaseOnSide(Side)) :
                                     new Coordinate(actualTargetColumn, Coordinate.Row - 1.ConvertStepsBaseOnSide(Side));
    }

	public override IEnumerable<Coordinate> GeneratePotentialMoves(Piece[,] position)
	{
        List<Coordinate> potentialMoves = [];

        List<(int potentialColumn, int potentialRow)> potentialCoordinates = 
            [(Coordinate.Column - 1, Coordinate.Row + 2),
			(Coordinate.Column + 1, Coordinate.Row + 2),
			(Coordinate.Column - 1, Coordinate.Row - 2),
			(Coordinate.Column + 1, Coordinate.Row - 2),
			(Coordinate.Column - 2, Coordinate.Row + 1),
			(Coordinate.Column - 2, Coordinate.Row - 1),
			(Coordinate.Column + 2, Coordinate.Row + 1),
			(Coordinate.Column + 2, Coordinate.Row - 1)];

        foreach ((int potentialColumn, int potentialRow) in potentialCoordinates)
        {
            try
            {
                Coordinate destination = new(potentialColumn, potentialRow);

				// Skip if the destination is a King as it is not a valid move to capture a king
				if (position.GetPieceAtPosition(destination) is King)
					continue;

				if (ValidateMove(position, Coordinate, destination))
                    potentialMoves.Add(destination);
			}
            catch (ArgumentOutOfRangeException)
			{
				continue;
			}
		}

        return potentialMoves;
	}
}
