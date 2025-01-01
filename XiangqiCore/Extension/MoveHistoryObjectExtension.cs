﻿using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Move.NotationTranslators;

namespace XiangqiCore.Extension;

public static class MoveHistoryObjectExtension
{
	public static string TranslateTo(
		this MoveHistoryObject moveHistoryObject, 
		MoveNotationType targetNotationType = MoveNotationType.TraditionalChinese, 
		Language language = Language.TraditionalChinese)
	{
		INotationTranslator translator = NotationTranslatorFactory.GetTranslator(targetNotationType, language);

		return translator.Translate(moveHistoryObject);
	}
}