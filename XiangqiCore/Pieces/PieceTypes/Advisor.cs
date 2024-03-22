﻿using XiangqiCore.Attributes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Pieces;

[HasAvailableRows(redRows: [1,2,3], blackRows: [8,9,10])]
[HasAvailableColumns(4,5,6)]
public sealed class Advisor : Piece
{
    public Advisor(Coordinate coordinate, Side side) 
        : base(coordinate, side)
    {
        ValidationStrategy = new AdvisorValidationStrategy();
    }
    public override IValidationStrategy ValidationStrategy { get; }
}
