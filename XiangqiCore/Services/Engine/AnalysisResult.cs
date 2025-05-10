namespace XiangqiCore.Services.Engine;

public class AnalysisResult
{
	public string BestMove => PrincipalVariation.FirstOrDefault();

	/// <summary>
	/// Postiive for advantage to Red, Negative for advantage to Black
	/// </summary>
	public int Score { get; set; }

	/// <summary>
	/// The sequence of moves that is proposed by the engine
	/// </summary>
	public List<string> PrincipalVariation { get; set; }

	public int Depth { get; set; }

	public decimal TimeSpent { get; set; }
}
