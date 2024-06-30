using XiangqiCore.Attributes;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;

[MoveInDiagonals]
public sealed class Bishop : Piece
{
    public Bishop(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new BishopValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }

    public override PieceType PieceType => PieceType.Bishop;
    public override char FenCharacter => 'b';

    public override Coordinate GetDestinationCoordinateFromNotation(MoveDirection moveDirection, int fourthCharacterInNotation)
    => moveDirection switch
    {
        MoveDirection.Forward => new Coordinate(fourthCharacterInNotation.ConvertToColumnBasedOnSide(Side), Coordinate.Row + 2.ConvertStepsBaseOnSide(Side)),
        MoveDirection.Backward => new Coordinate(fourthCharacterInNotation.ConvertToColumnBasedOnSide(Side), Coordinate.Row - 2.ConvertStepsBaseOnSide(Side)),
        _ => throw new ArgumentException("Invalid Move Direction")
    };
}
