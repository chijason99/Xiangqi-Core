using XiangqiCore.Game;
using XiangqiCore.Move;

namespace XiangqiCore.Services.PgnGeneration;

public interface IPgnGenerationService
{
	/// <summary>
	/// Generate a PGN byte array of the game.
	/// </summary>
	/// <param name="game"></param>
	/// <param name="moveNotationType"></param>
	/// <returns></returns>
	public byte[] GeneratePgn(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese);
	
	/// <summary>
	/// Generate a PGN string of the game.
	/// </summary>
	/// <param name="game"></param>
	/// <param name="moveNotationType"></param>
	/// <returns></returns>
	public string GeneratePgnString(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese);

	/// <summary>
	/// Export the move history of the game.
	/// </summary>
	/// <param name="game"></param>
	/// <param name="targetNotationType"></param>
	/// <returns></returns>
	public string ExportMoveHistory(XiangqiGame game, MoveNotationType targetNotationType = MoveNotationType.TraditionalChinese);
}
