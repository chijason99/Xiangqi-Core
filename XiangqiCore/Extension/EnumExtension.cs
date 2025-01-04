using System.ComponentModel;
using System.Reflection;
using XiangqiCore.Attributes;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Move.NotationParser;
using XiangqiCore.Move.NotationParsers;
using XiangqiCore.Move.NotationTranslators;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace XiangqiCore.Extension;

public class EnumHelper<T> where T : Enum
{
    /// <summary>
    /// Gets a random value from the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A random value from the enum type.</returns>
    public static T GetRandomValue()
    {
        List<T> filteredEnumValues = ExcludeIgnoreRandomItemsFromEnum();

        return filteredEnumValues
                    .Cast<T>()
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();
    }

    private static List<T> ExcludeIgnoreRandomItemsFromEnum()
    {
        List<T> result = [];

        foreach (T element in Enum.GetValues(typeof(T)))
        {
            MemberInfo memberInfo = typeof(T).GetMember(element.ToString()).First();

            bool shouldBeIgnored = memberInfo.GetCustomAttribute<IgnoreFromRandomPickAttribute>() is not null;

            if (!shouldBeIgnored)
                result.Add(element);
        }

        return result;
    }

    public static List<string> GetAllNames()
    {
        List<string> result = [];

        foreach (T element in Enum.GetValues(typeof(T)))
        {
            MemberInfo memberInfo = typeof(T).GetMember(element.ToString()).First();

            bool shouldBeIgnored = memberInfo.GetCustomAttribute<IgnoreFromRandomPickAttribute>() is not null;

            if (!shouldBeIgnored)
                result.Add(element.GetType().Name);
        }

        return result;
    }

    /// <summary>
    /// Gets the display name of the specified target element.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="targetElement">The target element.</param>
    /// <returns>The display name of the target element.</returns>
    public static string GetDisplayName(T targetElement)
    {
        MemberInfo targetMember = typeof(T).GetMember(targetElement.ToString()).First();
        DescriptionAttribute descriptionAttribute = targetMember.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute is not null ? descriptionAttribute.Description : targetMember.Name;
    }

    /// <summary>
    /// Gets the symbol for the specified target element, language, and side.
    /// If the specified language is not found, it falls back to Traditional Chinese.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="targetElement">The target element.</param>
    /// <param name="language">The language.</param>
    /// <param name="side">The side (default is Red).</param>
    /// <returns>The symbol for the target element.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no symbol attribute is found.</exception>
    public static string[] GetSymbols(T targetElement, Language language, Side side = Side.Red)
    {
        MemberInfo memberInfo = typeof(T).GetMember(targetElement.ToString()).First();

        SymbolAttribute symbolAttribute = memberInfo.GetCustomAttributes<SymbolAttribute>().SingleOrDefault(x => x.Language == language) ??
            memberInfo.GetCustomAttributes<SymbolAttribute>().SingleOrDefault(x => x.Language == Language.TraditionalChinese) ??
            throw new InvalidOperationException($"Please use the Symbol attribute to set the corresponding symbol for {language}");


        return side == Side.Red ? symbolAttribute.RedSymbols : symbolAttribute.BlackSymbols;
    }

    public static string GetDefaultSymbol(T targetElement, Language language, Side side = Side.Red)
    {
        MemberInfo memberInfo = typeof(T).GetMember(targetElement.ToString()).First();

        SymbolAttribute symbolAttribute = memberInfo.GetCustomAttributes<SymbolAttribute>().SingleOrDefault(x => x.Language == language) ??
            memberInfo.GetCustomAttributes<SymbolAttribute>().SingleOrDefault(x => x.Language == Language.TraditionalChinese) ??
            throw new InvalidOperationException($"Please use the Symbol attribute to set the corresponding symbol for {language}");

        return side == Side.Red ? symbolAttribute.DefaultRedSymbol : symbolAttribute.DefaultBlackSymbol;
    }
}

public static class EnumExtension
{
    /// <summary>
    /// Gets the opposite side of the specified side.
    /// </summary>
    /// <param name="side">The side to get the opposite side of.</param>
    /// <returns>The opposite side.</returns>
    public static Side GetOppositeSide(this Side side)
    {
        if (side == Side.None) throw new ArgumentException("Please use either Side.Red or Side.Black as the parameter");

        return side == Side.Black ? Side.Red : Side.Black;
    }

	public static IValidationStrategy GetValidationStrategy(this PieceType pieceType) => pieceType switch
	{
		PieceType.Advisor => new AdvisorValidationStrategy(),
		PieceType.Bishop => new BishopValidationStrategy(),
		PieceType.Cannon => new CannonValidationStrategy(),
		PieceType.Rook => new RookValidationStrategy(),
		PieceType.King => new KingValidationStrategy(),
		PieceType.Knight => new KnightValidationStrategy(),
		PieceType.Pawn => new PawnValidationStrategy(),
		_ => throw new InvalidOperationException("Please provide a valid piece type")
	};

    public static bool IsMovingInDiagonals(this PieceType pieceType)
        => pieceType == PieceType.Bishop || pieceType == PieceType.Advisor || pieceType == PieceType.Knight;
}