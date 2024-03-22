namespace XiangqiCore.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HasAvailableRowsAttribute(int[] redRows, int[] blackRows) : Attribute
{
    public int[] RedRows { get; } = redRows;
    public int[] Blackows { get; } = blackRows;
}
