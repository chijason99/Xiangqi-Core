namespace XiangqiCore.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ChineseNameAttribute(string nameForRed, string nameForBlack = "") : Attribute
{
	public string NameForRed { get; } = nameForRed;
	public string NameForBlack { get; } = string.IsNullOrWhiteSpace(nameForBlack) ? nameForRed : nameForBlack;
}
