namespace XiangqiCore.Services.Engine;

public interface IXiangqiEngineService
{
	// Engine lifecycle management
	public Task LoadEngineAsync(string enginePath);
	public void StopEngine();

	// Engine analysis
	public Task<AnalysisResult> AnalyzePositionAsync(string fen, EngineAnalysisOptions options);
	public Task<string> SuggestMoveAsync(string fen, EngineAnalysisOptions options);

	// Engine Configuration
	public Task SetEngineConfigAsync();

	// Custom command execution
	public Task<string> SendCustomCommand(string command);

	// Utility
	public Task<bool> IsReady();
}
