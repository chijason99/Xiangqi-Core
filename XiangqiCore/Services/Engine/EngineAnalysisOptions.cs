namespace XiangqiCore.Services.Engine;

public class EngineAnalysisOptions
{
	/// <summary>
	/// Depth of analysis
	/// </summary>
	public int? Depth { get; set; }

	/// <summary>
	/// Move time for the engine in milliseconds
	/// </summary>
	public int? MoveTime { get; set; }

	/// <summary>
	/// Indicates whether the engine should use pondering during analysis.
	/// Pondering is a feature in chess engines where the engine continues to think
	/// during the opponent's turn, attempting to predict the opponent's move
	/// and prepare a response in advance. This can improve performance but may
	/// consume additional computational resources.
	/// </summary>
	public bool UsePonder { get; set; } = false;

	public IEnumerable<string> Moves { get; set; } = [];
}
