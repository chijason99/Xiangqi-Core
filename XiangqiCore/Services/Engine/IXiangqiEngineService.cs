using XiangqiCore.Move;

namespace XiangqiCore.Services.Engine;

public interface IXiangqiEngineService
{
	// Engine lifecycle management
	public Task LoadEngineAsync(string enginePath);
	public void StopEngine();

	// Engine analysis

	/// <summary>
	/// Equilvalent to calling "go infinite" in an UCI engine
	/// </summary>
	/// <param name="fen">The postiion you wish to analyse</param>
	/// <param name="notationType">The move notation type you want to see in the response</param>
	/// <returns>An enumerable of the AnalysisResult</returns>
	public IAsyncEnumerable<AnalysisResult> AnalyzePositionAsync(string fen, MoveNotationType notationType = MoveNotationType.TraditionalChinese);

	/// <summary>
	/// Equilvalent to calling "go" in an UCI engine
	/// </summary>
	/// <param name="fen">The postiion you wish to analyse</param>
	/// <param name="options"></param>
	/// <returns>The best move returned by the engine</returns>
	public Task<string> SuggestMoveAsync(string fen, EngineAnalysisOptions options);

	/// <summary>
	/// Stop the engine analysis. Equilvalent to calling "stop" in an UCI engine
	/// </summary>
	/// <returns></returns>
	public Task StopAnalysisAsync();

	// Engine Configuration
	public Task SetEngineConfigAsync(string optionName, string value);

	// Utility
	public Task<bool> IsReady();
}
