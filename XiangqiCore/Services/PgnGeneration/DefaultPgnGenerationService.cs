using System.Text;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Services.MoveParsing;

namespace XiangqiCore.Services.PgnGeneration;

public class DefaultPgnGenerationService : IPgnGenerationService
{
	public void SavePgnToFile(
		string filePath, 
		XiangqiGame game, 
		MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)
	{
		Encoding gb2312Encoding = CodePagesEncodingProvider.Instance.GetEncoding(936) ?? Encoding.UTF8;
		string pgnString = GeneratePgnString(game, moveNotationType);

		byte[] pgnBytes = gb2312Encoding.GetBytes(pgnString);

		using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
		using StreamWriter streamWriter = new(fileStream);

		fileStream.Write(pgnBytes);
	}

	public async Task SavePgnToFileAsync(
		string filePath, 
		XiangqiGame game, 
		MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese, 
		CancellationToken cancellationToken = default)
	{
		Encoding gb2312Encoding = CodePagesEncodingProvider.Instance.GetEncoding(936) ?? Encoding.UTF8;
		string pgnString = GeneratePgnString(game, moveNotationType);

		byte[] pgnBytes = gb2312Encoding.GetBytes(pgnString);

		using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
		using StreamWriter streamWriter = new(fileStream);

		await fileStream.WriteAsync(pgnBytes, cancellationToken);
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
