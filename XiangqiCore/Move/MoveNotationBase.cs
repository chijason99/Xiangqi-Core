using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Move.NotationParsers;

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

    public virtual string TranslateToChinese(MoveHistoryObject moveHistoryObject)
        => throw new NotImplementedException();
    public virtual string TranslateToEnglish(MoveHistoryObject moveHistoryObject)
		=> throw new NotImplementedException();

	public virtual string TranslateToUcci(MoveHistoryObject moveHistoryObject)
		=> throw new NotImplementedException();
}
