using System.Reflection;
using XiangqiCore.Attributes;

namespace XiangqiCore.Extension;
public class EnumHelper<T> where T : Enum 
{
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
}
