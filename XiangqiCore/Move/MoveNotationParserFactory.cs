namespace XiangqiCore.Move;
public static class MoveNotationParserFactory
{
    public static IMoveNotationParser GetParser(MoveNotationType notationType)
        => notationType switch
        {
            MoveNotationType.Chinese => MoveNotationBase.GetMoveNotationParserInstance<ChineseNotationParser>(),
            MoveNotationType.English => MoveNotationBase.GetMoveNotationParserInstance<EnglishNotationParser>(),
            MoveNotationType.UCCI => MoveNotationBase.GetMoveNotationParserInstance<UcciNotationParser>(),
            _ => throw new ArgumentException("Invalid Move Notation Type")
        };
}
