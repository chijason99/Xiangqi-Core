namespace XiangqiCore.Services.Engine;

public class AnalysisResult
{
	public string BestMove { get; set; }

	/// <summary>
	/// Postiive for advantage to Red, Negative for advantage to Black
	/// </summary>
	public double Score { get; set; }

	/// <summary>
	/// The sequence of moves that is proposed by the engine
	/// </summary>
	public string PrincipalVairation { get; set; }

	public int Depth { get; set; }

	public TimeSpan AnalysisTime { get; set; }
}
