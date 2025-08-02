using System.Text;
using System.Text.RegularExpressions;
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Services.MoveTransalation;
using XiangqiCore.Services.ProcessManager;

namespace XiangqiCore.Services.Engine;

public class DefaultUciEngineService : IXiangqiEngineService
{
	private readonly IProcessManager _processManager;
	private readonly IMoveTranslationService _moveTranslationService;

	public DefaultUciEngineService()
	{
		_processManager = new DefaultProcessManager();
		_moveTranslationService = new DefaultMoveTranslationService();
	}

	public DefaultUciEngineService(IProcessManager processManager, IMoveTranslationService moveTranslationService)
	{
		_processManager = processManager;
		_moveTranslationService = moveTranslationService;
	}

	public async IAsyncEnumerable<AnalysisResult> AnalyzePositionAsync(string fen, MoveNotationType notationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default)
	{
		Regex infoRegex = new(@"^info(?:\sdepth\s(?<depth>\d+))?(?:\sseldepth\s(?<seldepth>\d+))?(?:\smultipv\s(?<multipv>\d+))?(?:\sscore\scp\s(?<score>\d+))?(?:\snodes\s(?<nodes>\d+))?(?:\snps\s(?<nps>\d+))?(?:\stbhits\s(?<tbhits>\d+))?(?:\stime\s(?<time>\d+))?(?:\spv\s(?<pv>.+))?");

		// Default to start position
		if (string.IsNullOrWhiteSpace(fen))
			fen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0";

		XiangqiBuilder xiangqiBuilder = new();
		XiangqiGame game = xiangqiBuilder.WithStartingFen(fen).Build();

		await _processManager.SendCommandAsync($"position fen {fen}");
		await _processManager.SendCommandAsync("go infinite");

		await foreach (string response in _processManager.ReadResponsesAsync(_ => false, cancellationToken: cancellationToken))
		{
			Console.WriteLine($"Response read: {response}");

			if (response.StartsWith("info", StringComparison.OrdinalIgnoreCase))
			{
				Match match = infoRegex.Match(response);

				if (match.Success)
				{
					int score = int.Parse(match.Groups["score"].Value);
					int depth = int.Parse(match.Groups["depth"].Value);
					decimal time = decimal.Parse(match.Groups["time"].Value);
					string proposedMoves = match.Groups["pv"].Value;

					string[] moves = proposedMoves.Split(' ');
					List<string> principleVariation = [];

					if (game.MoveHistory.Count > 0)
						// Undo all moves before doing new ones
						game.UndoMove();

					foreach (string move in moves)
						game.MakeMove(move, MoveNotationType.UCI);

					foreach (MoveHistoryObject moveHistory in game.MoveHistory)
						principleVariation.Add(_moveTranslationService.TranslateMove(moveHistory, notationType));

					yield return new AnalysisResult()
					{
						Score = score,
						Depth = depth,
						PrincipalVariation = principleVariation,
						TimeSpent = time
					};
				}
				else
				{
					Console.WriteLine("Stopping analysis...");
					await StopAnalysisAsync();

					yield break;
				}
			}
		}
	}

	public async Task<bool> IsReady()
	{
		try
		{
			await _processManager.SendCommandAsync("isready");

			// Read responses until "readyok" is found
			await _processManager.ReadResponseAsync("readyok");

			return true;
		}
		catch (Exception ex) 
		{
			return false;
		}
	}

	public async Task LoadEngineAsync(string enginePath)
	{
		await _processManager.StartAsync(enginePath);
		await _processManager.SendCommandAsync("uci");

		// Read responses until "readyok" is found
		// Don't stop on emtpy response as there might be an empty line break after calling uci
		await _processManager.ReadResponseAsync("uciok", stopOnEmtpyResponse: false);
	}

	public async Task SetEngineConfigAsync(string optionName, string value)
	{
		await _processManager.SendCommandAsync($"setoption name {optionName} value {value}");
		string response = await _processManager.ReadResponseAsync(response => response.Length != 0);

		if (!string.IsNullOrWhiteSpace(response))
			throw new InvalidOperationException($"Failed to set engine option: {response}");

		// If no response is received, assume the command was successful
	}

	public void StopEngine()
		=> _processManager.Stop();

	public async Task<string> SuggestMoveAsync(
		string fen, 
		EngineAnalysisOptions options, 
		MoveNotationType notationType = MoveNotationType.TraditionalChinese, 
		CancellationToken cancellationToken = default)
	{
		// Default to start position
		if (string.IsNullOrWhiteSpace(fen))
			fen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0";

		XiangqiBuilder xiangqiBuilder = new();
		XiangqiGame game = xiangqiBuilder.WithStartingFen(fen).Build();

		StringBuilder builder = new();
		builder.Append($"position fen {fen}");
		
		if (options.Moves.Any())
		{
			builder.Append(" moves ");

			foreach (string move in options.Moves)
				builder.Append($"{move} ");
		}

		string positionCommand = builder.ToString();
		await _processManager.SendCommandAsync(positionCommand);

		builder.Clear();
		builder.Append("go");

		if (options.Depth.HasValue)
			builder.Append($" depth {options.Depth.Value}");

		if (options.MoveTime.HasValue)
			builder.Append($" movetime {options.MoveTime.Value}");

		if (options.UsePonder)
			builder.Append(" ponder");

		string goCommand = builder.ToString();
		await _processManager.SendCommandAsync(goCommand);

		await foreach (string response in _processManager.ReadResponsesAsync(_ => false, cancellationToken: cancellationToken))
		{
			Console.WriteLine(response);

			if (response.StartsWith("bestmove", StringComparison.OrdinalIgnoreCase))
			{
				Regex bestmoveRegex = new(@"^bestmove\s(?<bestMove>\w+)\sponder\s(?<ponder>\w+)");
				Match match = bestmoveRegex.Match(response);

				if (match.Success)
				{
					// Stop the analysis on successful
					await StopAnalysisAsync();

					game.MakeMove(match.Groups["bestMove"].Value, MoveNotationType.UCI);

					string bestMove = _moveTranslationService.TranslateMove(game.MoveHistory.First(), notationType);

					return bestMove;
				}
			}
		}

		throw new Exception();
	}

	public async Task StopAnalysisAsync()
	{
		await _processManager.SendCommandAsync("stop");
		await _processManager.ReadResponseAsync(_ => true, stopOnEmtpyResponse: true);
	}
}
