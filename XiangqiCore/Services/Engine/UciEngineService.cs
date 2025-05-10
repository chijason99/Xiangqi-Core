using System.Text;
using XiangqiCore.Services.ProcessManager;

namespace XiangqiCore.Services.Engine;

public class UciEngineService : IXiangqiEngineService
{
	private readonly IProcessManager _processManager;

	public UciEngineService(IProcessManager processManager)
		=> _processManager = processManager;

	public Task<AnalysisResult> AnalyzePositionAsync(string fen, EngineAnalysisOptions options)
	{
		throw new NotImplementedException();
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

	public async Task<string> SendCustomCommandAsync(string command, Func<string, bool>? responseHandler = null, TimeSpan? timeout = null)
	{
		await _processManager.SendCommandAsync(command);

		responseHandler ??= response => !string.IsNullOrWhiteSpace(response);
		string response = await _processManager.ReadResponseAsync(responseHandler, timeout);

		return response;
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

	public async Task<string> SuggestMoveAsync(string fen, EngineAnalysisOptions options)
	{
		// Default to start position
		if (string.IsNullOrWhiteSpace(fen))
			fen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0";

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

		await foreach (string response in _processManager.ReadResponsesAsync(
			response => response.StartsWith("bestmove", StringComparison.OrdinalIgnoreCase)))
		{
			Console.WriteLine(response);

			if (response.StartsWith("bestmove", StringComparison.OrdinalIgnoreCase))
				return response;
		}

		throw new Exception();
	}
}
