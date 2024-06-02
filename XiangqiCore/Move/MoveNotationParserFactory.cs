﻿using XiangqiCore.Move.Move;

namespace XiangqiCore.Move;
public static class MoveNotationParserFactory
{
    public static IMoveNotationParser GetParser(MoveNotationType notationType)
        => notationType switch
        {
            MoveNotationType.Chinese => MoveNotationBase.GetMoveNotationParserInstance<ChineseNotationParser>(),
            MoveNotationType.English => MoveNotationBase.GetMoveNotationParserInstance<EnglishNotationParser>(),
            _ => throw new ArgumentException("Invalid Move Notation Type")
        };
}