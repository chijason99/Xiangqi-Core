using XiangqiCore.Misc;

namespace XiangqiCore.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class SymbolAttribute : Attribute
{
	public SymbolAttribute(Language language, string redSymbol, string blackSymbol = "")
	{
		Language = language;
		RedSymbol = [redSymbol];

		if (string.IsNullOrWhiteSpace(blackSymbol))
			BlackSymbol = RedSymbol;
		else
			BlackSymbol = [blackSymbol];
	}

	public SymbolAttribute(Language language, string[] redSymbol, string[] blackSymbol = null)
	{
		Language = language;
		RedSymbol = redSymbol;

		if (blackSymbol == null)
			BlackSymbol = redSymbol;
		else
			BlackSymbol = blackSymbol;
	}

	public Language Language { get; }
	public string[] RedSymbol { get; }
	public string[] BlackSymbol { get; }
}
