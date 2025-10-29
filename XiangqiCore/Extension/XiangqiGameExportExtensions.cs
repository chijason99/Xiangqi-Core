using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;
using XiangqiCore.Services.MoveTransalation;
using XiangqiCore.Services.PgnGeneration;

namespace XiangqiCore.Extension;

/// <summary>
/// Provides convenient extension methods for exporting XiangqiGame data.
/// These methods use default services internally. For advanced scenarios requiring
/// custom translation services, use the service classes directly.
/// </summary>
public static class XiangqiGameExportExtensions
{
	// Shared singleton services - created once, reused for all calls
	private static readonly Lazy<IMoveTranslationService> _translationService = 
		new(() => new DefaultMoveTranslationService());
	
	private static readonly Lazy<IJsonGenerationService> _jsonService = 
		new(() => new DefaultJsonGenerationService(_translationService.Value));
	
	private static readonly Lazy<IPgnGenerationService> _pgnService = 
		new(() => new DefaultPgnGenerationService(_translationService.Value));

	#region JSON Export Methods

	/// <summary>
	/// Exports the game as a JSON string using default translation service.
	/// </summary>
	/// <param name="game">The game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese if not specified.</param>
	/// <returns>A JSON string representation of the game.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IJsonGenerationService"/> directly.
	/// </remarks>
	/// <example>
	/// <code>
	/// var game = new XiangqiBuilder().WithDefaultConfiguration().Build();
	/// game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
	/// 
	/// string json = game.ToJson();
	/// // or with specific notation
	/// string ucciJson = game.ToJson(MoveNotationType.UCCI);
	/// </code>
	/// </example>
	public static string ToJson(
		this XiangqiGame game, 
		MoveNotationType? notationType = null)
		=> _jsonService.Value.GenerateGameJson(game, notationType);

	/// <summary>
	/// Asynchronously exports the game as a JSON string using default translation service.
	/// </summary>
	/// <param name="game">The game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese if not specified.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the JSON string.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IJsonGenerationService"/> directly.
	/// </remarks>
	public static Task<string> ToJsonAsync(
		this XiangqiGame game,
		MoveNotationType? notationType = null)
		=> _jsonService.Value.GenerateGameJsonAsync(game, notationType);

	/// <summary>
	/// Exports the game as a JSON byte array using default translation service.
	/// </summary>
	/// <param name="game">The game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese if not specified.</param>
	/// <returns>A UTF-8 encoded byte array containing the JSON representation.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IJsonGenerationService"/> directly.
	/// </remarks>
	public static byte[] ToJsonBytes(
		this XiangqiGame game,
		MoveNotationType? notationType = null)
		=> _jsonService.Value.GenerateGameJsonBytes(game, notationType);

	/// <summary>
	/// Asynchronously exports the game as a JSON byte array using default translation service.
	/// </summary>
	/// <param name="game">The game to export.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese if not specified.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the UTF-8 encoded byte array.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IJsonGenerationService"/> directly.
	/// </remarks>
	public static Task<byte[]> ToJsonBytesAsync(
		this XiangqiGame game,
		MoveNotationType? notationType = null)
		=> _jsonService.Value.GenerateGameJsonBytesAsync(game, notationType);

	/// <summary>
	/// Saves the game as a JSON file using default translation service.
	/// </summary>
	/// <param name="game">The game to save.</param>
	/// <param name="filePath">The path where the JSON file will be saved.</param>
	/// <param name="notationType">Optional move notation type. Defaults to Traditional Chinese if not specified.</param>
	/// <returns>A task that represents the asynchronous save operation.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IJsonGenerationService"/> directly.
	/// </remarks>
	/// <example>
	/// <code>
	/// var game = new XiangqiBuilder().WithDefaultConfiguration().Build();
	/// game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
	/// 
	/// await game.SaveAsJsonAsync("game.json");
	/// </code>
	/// </example>
	public static async Task SaveAsJsonAsync(
		this XiangqiGame game,
		string filePath,
		MoveNotationType? notationType = null)
	{
		byte[] jsonBytes = await _jsonService.Value.GenerateGameJsonBytesAsync(game, notationType);
		await File.WriteAllBytesAsync(filePath, jsonBytes);
	}

	#endregion

	#region PGN Export Methods

	/// <summary>
	/// Exports the game as a PGN string using default translation service.
	/// </summary>
	/// <param name="game">The game to export.</param>
	/// <param name="notationType">Move notation type to use. Defaults to Traditional Chinese.</param>
	/// <returns>A PGN string representation of the game.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IPgnGenerationService"/> directly.
	/// </remarks>
	/// <example>
	/// <code>
	/// var game = new XiangqiBuilder().WithDefaultConfiguration().Build();
	/// game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
	/// 
	/// string pgn = game.ToPgn();
	/// // or with specific notation
	/// string ucciPgn = game.ToPgn(MoveNotationType.UCCI);
	/// </code>
	/// </example>
	public static string ToPgn(
		this XiangqiGame game,
		MoveNotationType notationType = MoveNotationType.TraditionalChinese)
		=> _pgnService.Value.GeneratePgnString(game, notationType);

	/// <summary>
	/// Exports the game as a PGN byte array using default translation service.
	/// </summary>
	/// <param name="game">The game to export.</param>
	/// <param name="notationType">Move notation type to use. Defaults to Traditional Chinese.</param>
	/// <returns>A byte array containing the PGN representation (GB2312 encoded).</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IPgnGenerationService"/> directly.
	/// </remarks>
	public static byte[] ToPgnBytes(
		this XiangqiGame game,
		MoveNotationType notationType = MoveNotationType.TraditionalChinese)
		=> _pgnService.Value.GeneratePgn(game, notationType);

	/// <summary>
	/// Saves the game as a PGN file using default translation service.
	/// </summary>
	/// <param name="game">The game to save.</param>
	/// <param name="filePath">The path where the PGN file will be saved.</param>
	/// <param name="notationType">Move notation type to use. Defaults to Traditional Chinese.</param>
	/// <returns>A task that represents the asynchronous save operation.</returns>
	/// <remarks>
	/// This is a convenience method for common use cases. For advanced scenarios
	/// requiring custom translation services, use <see cref="IPgnGenerationService"/> directly.
	/// </remarks>
	/// <example>
	/// <code>
	/// var game = new XiangqiBuilder().WithDefaultConfiguration().Build();
	/// game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
	/// 
	/// await game.SaveAsPgnAsync("game.pgn");
	/// </code>
	/// </example>
	public static async Task SaveAsPgnAsync(
		this XiangqiGame game,
		string filePath,
		MoveNotationType notationType = MoveNotationType.TraditionalChinese)
	{
		byte[] pgnBytes = _pgnService.Value.GeneratePgn(game, notationType);
		await File.WriteAllBytesAsync(filePath, pgnBytes);
	}

	#endregion
}
