using XiangqiCore.Game;
using XiangqiCore.Move;

namespace XiangqiCore.Services.PgnSaving;

public interface IPgnSavingService
{
	/// <summary>
	/// Save the game to a PGN file.
	/// </summary>
	/// <param name="filePath">Please include the file extension .pgn in the file path provided</param>
	/// <param name="game"></param>
	/// <param name="moveNotationType"></param>
	public void Save(string filePath, XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese);

	/// <summary>
	/// Save the game to a PGN file asynchronously.
	/// </summary>
	/// <param name="filePath">Please include the file extension .pgn in the file path provided</param>
	/// <param name="game"></param>
	/// <param name="moveNotationType"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task SaveAsync(string filePath, XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default);
}
