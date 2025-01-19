using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
using XiangqiCore.Services.ImageGeneration;

namespace XiangqiCore.Services.ImageSaving;

public class DefaultImageSavingService : IImageSavingService
{
	private const string DEFAULT_FILE_NAME = $"xiangqi_core_image";
	private readonly IImageGenerationService _imageGenerationService;

	public DefaultImageSavingService()
	{
		_imageGenerationService = new DefaultImageGenerationService();
	}

	public DefaultImageSavingService(IImageGenerationService imageGenerationService)
	{
		_imageGenerationService = imageGenerationService;
	}

	public void SaveImageToFile(string filePath, string fen, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = _imageGenerationService.GenerateImage(fen, imageConfig);

		WriteBytesToFile(sanitizedFilePath, imageInBytes);
	}

	public void SaveImageToFile(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = _imageGenerationService.GenerateImage(moveHistoryObject, imageConfig );

		WriteBytesToFile(sanitizedFilePath, imageInBytes);
	}

	public void SaveImageToFile(string filePath, Piece[,] position, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = _imageGenerationService.GenerateImage(position, imageConfig);

		WriteBytesToFile(sanitizedFilePath, imageInBytes);
	}

	public async Task SaveImageToFileAsync(string filePath, string fen, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = await _imageGenerationService.GenerateImageAsync(fen, imageConfig, cancellationToken);

		await WriteBytesToFileAsync(sanitizedFilePath, imageInBytes, cancellationToken);
	}

	public async Task SaveImageToFileAsync(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = await _imageGenerationService.GenerateImageAsync(moveHistoryObject, imageConfig, cancellationToken);

		await WriteBytesToFileAsync(sanitizedFilePath, imageInBytes, cancellationToken);
	}

	public async Task SaveImageToFileAsync(string filePath, Piece[,] position, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = await _imageGenerationService.GenerateImageAsync(position, imageConfig, cancellationToken);

		await WriteBytesToFileAsync(sanitizedFilePath, imageInBytes, cancellationToken);
	}

	private string PrepareFilePath(string filePath)
		=> FilePathHelper.PrepareFilePath(filePath, ".jpg", defaultFileName: DEFAULT_FILE_NAME);

	private void WriteBytesToFile(string filePath, byte[] bytes)
	{
		try
		{
			File.WriteAllBytes(filePath, bytes);
		}
		catch (Exception ex)
		{
			throw new IOException($"Failed to write to file {filePath}", ex);
		}
	}

	private async Task WriteBytesToFileAsync(string filePath, byte[] bytes, CancellationToken cancellationToken)
	{
		try
		{
			await File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
		}
		catch (Exception ex)
		{
			throw new IOException($"Failed to write to file {filePath}", ex);
		}
	}
}
