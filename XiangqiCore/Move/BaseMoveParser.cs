using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiangqiCore.Move.Move;

namespace XiangqiCore.Move;
public abstract class BaseMoveParser
{
    public string Notation { get; set; }
    public MoveNotationType MoveNotationType { get; set; }
}
