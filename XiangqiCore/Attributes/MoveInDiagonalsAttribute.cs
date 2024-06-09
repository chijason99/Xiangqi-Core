namespace XiangqiCore.Attributes;

// TODO: Change it such that it can only be applied on a piece class
[AttributeUsage(AttributeTargets.Class)]
public class MoveInDiagonalsAttribute : Attribute
{
    public MoveInDiagonalsAttribute() { }
}
