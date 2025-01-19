using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Services.GifGeneration;

namespace XiangqiCore.Services.GifSaving;

public class DefaultGifSavingService : IGifSavingService
{
	private const string DEFAULT_FILE_NAME = $"xiangqi_core";
	private readonly IGifGenerationService _gifGenerationService;

	public DefaultGifSavingService()
	{
		_gifGenerationService = new DefaultGifGenerationService();
	}

	public DefaultGifSavingService(IGifGenerationService gifGenerationService)
	{
		_gifGenerationService = gifGenerationService;
	}

	public void SaveGifToFile(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);
		byte[] gifBytes = _gifGenerationService.GenerateGif(fens, imageConfig);

		FileHelper.WriteBytesToFile(sanitizedFilePath, gifBytes);
	}

	public void SaveGifToFile(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);
		byte[] gifBytes = _gifGenerationService.GenerateGif(moveHistory, imageConfig);

		FileHelper.WriteBytesToFile(sanitizedFilePath, gifBytes);
	}

	public void SaveGifToFile(string filePath, XiangqiGame game, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);
		byte[] gifBytes = _gifGenerationService.GenerateGif(game, imageConfig);

		FileHelper.WriteBytesToFile(sanitizedFilePath, gifBytes);
	}

	public async Task SaveGifToFileAsync(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);
		byte[] gifBytes = await _gifGenerationService.GenerateGifAsync(fens, imageConfig, cancellationToken);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, gifBytes, cancellationToken);
	}

	public async Task SaveGifToFileAsync(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);
		byte[] gifBytes = await _gifGenerationService.GenerateGifAsync(
			moveHistory, 
			imageConfig, 
			cancellationToken);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, gifBytes, cancellationToken);
	}

	public async Task SaveGifToFileAsync(string filePath, XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);
		byte[] gifBytes = await _gifGenerationService.GenerateGifAsync(
			game,
			imageConfig,
			cancellationToken);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, gifBytes, cancellationToken);
	}

	private string PrepareFilePath(string filePath)
		=> FileHelper.PrepareFilePath(filePath, ".gif", defaultFileName: DEFAULT_FILE_NAME);
}
