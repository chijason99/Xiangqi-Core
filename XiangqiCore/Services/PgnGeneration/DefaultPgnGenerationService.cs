using System.Text;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Services.MoveTransalation;

namespace XiangqiCore.Services.PgnGeneration;

public class DefaultPgnGenerationService : IPgnGenerationService
{
	private readonly IMoveTranslationService _moveTranslationService;

	public DefaultPgnGenerationService(IMoveTranslationService moveTranslationService)
	{
		_moveTranslationService = moveTranslationService;
	}

	public DefaultPgnGenerationService()
	{
		_moveTranslationService = new DefaultMoveTranslationService();
	}

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

		string moveHistory = ExportMoveHistory(game, moveNotationType);
		pgnStringBuilder.AppendLine(moveHistory);

		return pgnStringBuilder.ToString();
	}

	public string ExportMoveHistory(XiangqiGame game, MoveNotationType targetNotationType = MoveNotationType.TraditionalChinese)
	{
		List<string> movesOfEachRound = [];

		var groupedMoveHitories = game.MoveHistory
			.Select(moveHistoryItem =>
				new
				{
					moveHistoryItem.RoundNumber,
					moveHistoryItem.MovingSide,
					MoveNotation = _moveTranslationService.TranslateMove(moveHistoryItem, targetNotationType)
				})
			.GroupBy(moveHistoryItem => moveHistoryItem.RoundNumber)
			.OrderBy(roundGroup => roundGroup.Key);

		foreach (var roundGroup in groupedMoveHitories)
		{
			StringBuilder roundMoves = new();

			string? moveNotationFromRed = roundGroup.SingleOrDefault(move => move.MovingSide == Side.Red)?.MoveNotation ?? "...";
			string? moveNotationFromBlack = roundGroup.SingleOrDefault(move => move.MovingSide == Side.Black)?.MoveNotation;

			roundMoves.Append($"{roundGroup.Key}. {moveNotationFromRed}  {moveNotationFromBlack}");

			movesOfEachRound.Add(roundMoves.ToString());
		};

		return string.Join("\n", movesOfEachRound);
	}

	private string CreatePgnTag(PgnTagType pgnTagKey, string pgnTagValue)
	{
		string pgnTagDisplayName = EnumHelper<PgnTagType>.GetDisplayName(pgnTagKey);

		return $"[{pgnTagDisplayName} \"{pgnTagValue}\"]";
	}
}
