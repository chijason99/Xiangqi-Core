using XiangqiCore.Game;
using XiangqiCore.Move;

namespace XiangqiCore.Services.JsonGeneration;

public interface IJsonGenerationService
{
	/// <summary>
	/// Generates a JSON string representation of the game asynchronously.
	/// </summary>
	/// <param name="game">The Xiangqi game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese.</param>
	/// <returns>A JSON string representing the game.</returns>
	Task<string> GenerateGameJsonAsync(XiangqiGame game, MoveNotationType? notationType = null);

	/// <summary>
	/// Generates a JSON string representation of the game.
	/// </summary>
	/// <param name="game">The Xiangqi game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese.</param>
	/// <returns>A JSON string representing the game.</returns>
	string GenerateGameJson(XiangqiGame game, MoveNotationType? notationType = null);

	/// <summary>
	/// Generates a JSON byte array representation of the game.
	/// </summary>
	/// <param name="game">The Xiangqi game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese.</param>
	/// <returns>A UTF-8 encoded byte array of the JSON representation.</returns>
	byte[] GenerateGameJsonBytes(XiangqiGame game, MoveNotationType? notationType = null);

	/// <summary>
	/// Generates a JSON byte array representation of the game asynchronously.
	/// </summary>
	/// <param name="game">The Xiangqi game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese.</param>
	/// <returns>A UTF-8 encoded byte array of the JSON representation.</returns>
	Task<byte[]> GenerateGameJsonBytesAsync(XiangqiGame game, MoveNotationType? notationType = null);
}