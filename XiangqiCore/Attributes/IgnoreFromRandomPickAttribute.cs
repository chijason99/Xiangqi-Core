namespace XiangqiCore.Attributes;

/// <summary>
/// Stating that the enum item will be ignored when retrieving a random value from the EnumHelper method
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class IgnoreFromRandomPickAttribute : Attribute
{
    public IgnoreFromRandomPickAttribute() { }
}
