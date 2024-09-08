using System.ComponentModel;
using System.Reflection;
using XiangqiCore.Attributes;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveNotationTranslators;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Pieces.PieceTypes;

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

        foreach(T element in Enum.GetValues(typeof(T)))
        {
            MemberInfo memberInfo = typeof(T).GetMember(element.ToString()).First();

            bool shouldBeIgnored = memberInfo.GetCustomAttribute<IgnoreFromRandomPickAttribute>() is not null;

            if (!shouldBeIgnored) 
                result.Add(element);
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

    public static string TranslateTo(this MoveNotationType originalNotationType, MoveHistoryObject moveObject, MoveNotationType targetNotationType)
    {
        if (originalNotationType == targetNotationType)
            return moveObject.MoveNotation;

        if (originalNotationType == MoveNotationType.Chinese)
            return ChineseNotationTranslator.Tanslate(moveObject, targetNotationType);

        //if (originalNotationType == MoveNotationType.English)
        //	return EnglishNotationTranslator.Tanslate(moveObject, targetNotationType);

        //if (originalNotationType == MoveNotationType.UCCI)
        //	return UcciNotationTranslator.Tanslate(moveObject, targetNotationType);

        return string.Empty;
    }

	public static char GetEnglishMoveNotationCharacter(this PieceType pieceType)
        => pieceType switch
		{
			PieceType.Rook => 'R',
			PieceType.Cannon => 'C',
			PieceType.Bishop => 'E',
			PieceType.King => 'K',
			PieceType.Knight => 'H',
			PieceType.Pawn => 'P',
			PieceType.Advisor => 'A',
			_ => throw new ArgumentException("Invalid piece type"),
		};
}