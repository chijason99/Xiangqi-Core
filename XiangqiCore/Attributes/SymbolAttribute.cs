using XiangqiCore.Misc;

namespace XiangqiCore.Attributes;

/// <summary>
/// An attribute to store the symbols of a piece type in different languages.
/// In the case of more than one symbol, the first symbol is the default symbol for move translation.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class SymbolAttribute : Attribute
{
	public SymbolAttribute(Language language, string redSymbol, string blackSymbol = "")
	{
		Language = language;
		RedSymbols = [redSymbol];

		if (string.IsNullOrWhiteSpace(blackSymbol))
			BlackSymbols = RedSymbols;
		else
			BlackSymbols = [blackSymbol];
	}

	public SymbolAttribute(Language language, string[] redSymbols, string[] blackSymbols = null)
	{
		Language = language;
		RedSymbols = redSymbols;

		if (blackSymbols == null)
			BlackSymbols = redSymbols;
		else
			BlackSymbols = blackSymbols;
	}

	public Language Language { get; }

	public string[] RedSymbols { get; }
	public string[] BlackSymbols { get; }

	public string DefaultRedSymbol => RedSymbols.First();
	public string DefaultBlackSymbol => BlackSymbols.First();
}
