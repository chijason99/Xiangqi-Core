namespace XiangqiCore.Move;
public abstract class MoveNotationBase : IMoveNotationParser
{
    private static Dictionary<Type, MoveNotationBase> _instances = [];
    protected MoveNotationBase() { }

    public static T GetMoveNotationParserInstance<T>() where T : MoveNotationBase, new()
    {
        if(!_instances.ContainsKey(typeof(T)))
            _instances[typeof(T)] = new T();

        return (T)_instances[typeof(T)];
    }

    public abstract ParsedMoveObject Parse(string notation);
}
