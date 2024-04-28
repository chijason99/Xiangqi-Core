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
}
