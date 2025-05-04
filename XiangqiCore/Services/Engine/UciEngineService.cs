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
		await _processManager.ReadResponseAsync("uciok");
	}

	public Task<string> SendCustomCommand(string command)
	{
		throw new NotImplementedException();
	}

	public Task SetEngineConfigAsync()
	{
		throw new NotImplementedException();
	}

	public void StopEngine()
		=> _processManager.Stop();

	public Task<string> SuggestMoveAsync(string fen, EngineAnalysisOptions options)
	{
		throw new NotImplementedException();
	}
}
