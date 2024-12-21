namespace XiangqiCore.Extension;

public static class LinqExtension
{
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> predicate)
		=> condition ? enumerable.Where(predicate) : enumerable;

	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> ifPredicate, Func<T, bool> elsePredicate)
		=> condition ? enumerable.Where(ifPredicate) : enumerable.Where(elsePredicate);
}
