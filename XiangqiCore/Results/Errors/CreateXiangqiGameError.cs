using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiangqiCore.Results.Errors;
public static class CreateXiangqiGameError
{
    public static Error InvalidFen => new($"{nameof(CreateXiangqiGameError)}.${nameof(InvalidFen)}", "Invalid FEN provided");
}
