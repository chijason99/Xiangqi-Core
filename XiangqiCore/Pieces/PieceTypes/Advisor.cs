using XiangqiCore.Attributes;
using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;

public sealed class Advisor : Piece
{
    public Advisor(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new AdvisorValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }

    public override PieceType PieceType => PieceType.Advisor;

    public override char FenCharacter => 'a';

    public override Coordinate GetDestinationCoordinateFromNotation(MoveDirection moveDirection, int fourthCharacterInNotation)
    => moveDirection switch
    {
        MoveDirection.Forward => new Coordinate(fourthCharacterInNotation.ConvertToColumnBasedOnSide(Side), Coordinate.Row + 1.ConvertStepsBaseOnSide(Side)),
        MoveDirection.Backward => new Coordinate(fourthCharacterInNotation.ConvertToColumnBasedOnSide(Side), Coordinate.Row - 1.ConvertStepsBaseOnSide(Side)),
        _ => throw new ArgumentException("Invalid Move Direction")
    };
}
