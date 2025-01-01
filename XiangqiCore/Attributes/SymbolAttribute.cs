using XiangqiCore.Misc;

namespace XiangqiCore.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class SymbolAttribute(Language language, string redSymbol, string blackSymbol = "") : Attribute
{
	public Language Language { get; } = language;
	public string RedSymbol { get; } = redSymbol;
	public string BlackSymbol { get; } = string.IsNullOrWhiteSpace(blackSymbol) ? redSymbol : blackSymbol;
}
