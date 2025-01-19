using System.Text;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;

namespace XiangqiCore.Services.PgnGeneration;

public class DefaultPgnGenerationService : IPgnGenerationService
{
	public byte[] GeneratePgn(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)
	{
		Encoding gb2312Encoding = CodePagesEncodingProvider.Instance.GetEncoding(936) ?? Encoding.UTF8;
		string pgnString = GeneratePgnString(game, moveNotationType);

		return gb2312Encoding.GetBytes(pgnString);
	}

	public string GeneratePgnString(
		XiangqiGame game, 
		MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)
	{
		StringBuilder pgnStringBuilder = new();

		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Game, "Chinese Chess"));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Event, game.Competition.Name));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Site, game.Competition.Location));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Date, game.Competition.GameDate?.ToString("yyyy.MM.dd") ?? string.Empty));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Red, game.RedPlayer.Name));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.RedTeam, game.RedPlayer.Team));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Black, game.BlackPlayer.Name));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.BlackTeam, game.BlackPlayer.Team));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.Result, game.GameResultString));
		pgnStringBuilder.AppendLine(CreatePgnTag(PgnTagType.FEN, game.InitialFenString));

		string moveHistory = game.ExportMoveHistory(moveNotationType);
		pgnStringBuilder.AppendLine(moveHistory);

		return pgnStringBuilder.ToString();
	}

	private string CreatePgnTag(PgnTagType pgnTagKey, string pgnTagValue)
	{
		string pgnTagDisplayName = EnumHelper<PgnTagType>.GetDisplayName(pgnTagKey);

		return $"[{pgnTagDisplayName} \"{pgnTagValue}\"]";
	}
}
