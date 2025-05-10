using XiangqiCore.Move;

namespace XiangqiCore.Services.Engine;

public interface IXiangqiEngineService
{
	// Engine lifecycle management
	public Task LoadEngineAsync(string enginePath);
	public void StopEngine();

	// Engine analysis

	/// <summary>
	/// Equivalent to calling "go infinite" in a UCI engine
	/// </summary>
	/// <param name="fen">The position you wish to analyze</param>
	/// <param name="notationType">The move notation type you want to see in the response</param>
	/// <param name="cancellationToken">Token to cancel the analysis</param>
	/// <returns>An enumerable of the AnalysisResult</returns>
	public IAsyncEnumerable<AnalysisResult> AnalyzePositionAsync(string fen, MoveNotationType notationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default);

	/// <summary>
	/// Equivalent to calling "go" in a UCI engine
	/// </summary>
	/// <param name="fen">The position you wish to analyze</param>
	/// <param name="options">Analysis options such as depth and move time</param>
	/// <param name="notationType">The move notation type you want to see in the response</param>
	/// <param name="cancellationToken">Token to cancel the analysis</param>
	/// <returns>The best move returned by the engine in the specified notation</returns>
	public Task<string> SuggestMoveAsync(string fen, EngineAnalysisOptions options, MoveNotationType notationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default);

	/// <summary>
	/// Stops the engine analysis. Equivalent to calling "stop" in a UCI engine
	/// </summary>
	/// <returns></returns>
	public Task StopAnalysisAsync();

	// Engine Configuration

	/// <summary>
	/// Sets a configuration option for the engine
	/// </summary>
	/// <param name="optionName">The name of the option to set</param>
	/// <param name="value">The value to set for the option</param>
	/// <returns></returns>
	public Task SetEngineConfigAsync(string optionName, string value);

	// Utility

	/// <summary>
	/// Checks if the engine is ready to accept commands
	/// </summary>
	/// <returns>True if the engine is ready, false otherwise</returns>
	public Task<bool> IsReady();
}
