namespace XiangqiCore.Attributes;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HasAvailableColumnsAttribute(params int[] columns) : Attribute
{
    public int[] Columns { get; } = columns;
}
