using XiangqiCore.Game;

namespace XiangqiCore.Services.PgnLoading;

public interface IPgnLoadingService
{
	/// <summary>
	/// Loads a Xiangqi game from a PGN file on disk.
	/// </summary>
	/// <param name="filePath">The full path to the PGN file.</param>
	/// <returns>The imported game.</returns>
	XiangqiGame LoadFromFile(string filePath);

	/// <summary>
	/// Loads a Xiangqi game from PGN content.
	/// </summary>
	/// <param name="pgnContent">The PGN content to import.</param>
	/// <returns>The imported game.</returns>
	XiangqiGame LoadFromString(string pgnContent);
}
