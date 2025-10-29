using System.Text;
using System.Text.Json;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Services.MoveTransalation;

namespace XiangqiCore.Services.JsonGeneration;

public class DefaultJsonGenerationService : IJsonGenerationService
{
	private readonly IMoveTranslationService _moveTranslationService;

	public DefaultJsonGenerationService(IMoveTranslationService moveTranslationService)
	{
		_moveTranslationService = moveTranslationService;
	}

	public DefaultJsonGenerationService()
	{
		_moveTranslationService = new DefaultMoveTranslationService();
	}

	public async Task<string> GenerateGameJsonAsync(XiangqiGame game, MoveNotationType? notationType = null)
	{
		return await Task.Run(() => GenerateGameJson(game, notationType));
	}

	public string GenerateGameJson(XiangqiGame game, MoveNotationType? notationType = null)
	{
		MoveNotationType targetNotationType = notationType ?? MoveNotationType.TraditionalChinese;

		var gameData = new
		{
			gameName = game.GameName,
			gameInfo = new
			{
				redPlayer = new
				{
					name = game.RedPlayer.Name,
					team = game.RedPlayer.Team
				},
				blackPlayer = new
				{
					name = game.BlackPlayer.Name,
					team = game.BlackPlayer.Team
				},
				competition = new
				{
					name = game.Competition.Name,
					location = game.Competition.Location,
					date = game.Competition.GameDate?.ToString("yyyy-MM-dd"),
					round = game.Competition.Round
				},
				result = game.GameResultString
			},
			boardState = new
			{
				initialFen = game.InitialFenString,
				currentFen = game.CurrentFen,
				sideToMove = game.SideToMove.ToString(),
				roundNumber = game.RoundNumber,
				numberOfMovesWithoutCapture = game.NumberOfMovesWithoutCapture
			},
			moveHistory = game.GetMoveHistory()
				.Select(moveHistoryItem => new
				{
					roundNumber = moveHistoryItem.RoundNumber,
					side = moveHistoryItem.MovingSide.ToString(),
					notation = _moveTranslationService.TranslateMove(moveHistoryItem, targetNotationType),
					notationType = targetNotationType.ToString(),
					pieceMoved = moveHistoryItem.PieceMoved.ToString(),
					from = new
					{
						row = moveHistoryItem.StartingPosition.Row,
						column = moveHistoryItem.StartingPosition.Column
					},
					to = new
					{
						row = moveHistoryItem.Destination.Row,
						column = moveHistoryItem.Destination.Column
					},
					isCapture = moveHistoryItem.IsCapture,
					isCheck = moveHistoryItem.IsCheck,
					isCheckmate = moveHistoryItem.IsCheckmate,
					fenAfterMove = moveHistoryItem.FenAfterMove
				})
				.ToList()
		};

		var options = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		return JsonSerializer.Serialize(gameData, options);
	}

	public byte[] GenerateGameJsonBytes(XiangqiGame game, MoveNotationType? notationType = null)
	{
		string jsonString = GenerateGameJson(game, notationType);
		return Encoding.UTF8.GetBytes(jsonString);
	}

	public async Task<byte[]> GenerateGameJsonBytesAsync(XiangqiGame game, MoveNotationType? notationType = null)
	{
		string jsonString = await GenerateGameJsonAsync(game, notationType);
		return Encoding.UTF8.GetBytes(jsonString);
	}
}
