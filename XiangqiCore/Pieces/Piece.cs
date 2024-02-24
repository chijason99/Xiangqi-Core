using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiangqiCore.Pieces;
public abstract class Piece(Coordinate coordinate, Side side, IValidationStrategy validationStrategy)
{
    public Coordinate Coordinate { get; private set; } = coordinate;
    public Side Side { get; init; } = side;
    private IValidationStrategy _validationStrategy { get; init; } = validationStrategy;
}
