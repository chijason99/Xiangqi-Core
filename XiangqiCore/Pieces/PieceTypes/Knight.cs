﻿using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;
public sealed class Knight : Piece
{
    public Knight(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new KnightValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }
    public override PieceType GetPieceType => PieceType.Knight;
    public override char FenCharacter => 'n';
}