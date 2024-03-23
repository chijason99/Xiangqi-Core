using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiangqiCore.Results.Errors;
public static class CreatePieceError
{
    public static Error InvalidPieceTypeError => new($"{nameof(CreatePieceError)}.{nameof(InvalidPieceTypeError)}", $"Invalid Piece Type Provided");
}
