using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
using XiangqiCore.Services.ImageGeneration;

namespace XiangqiCore.Services.ImageSaving;

public class DefaultImageSavingService : IImageSavingService
{
	private const string DEFAULT_FILE_NAME = $"xiangqi_core";
	private readonly IImageGenerationService _imageGenerationService;

	public DefaultImageSavingService()
	{
		_imageGenerationService = new DefaultImageGenerationService();
	}

	public DefaultImageSavingService(IImageGenerationService imageGenerationService)
	{
		_imageGenerationService = imageGenerationService;
	}

	public void Save(string filePath, string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = _imageGenerationService.GenerateImage(
			fen, 
			previousLocation: previousLocation, 
			currentLocation: currentLocation, 
			imageConfig);

		FileHelper.WriteBytesToFile(sanitizedFilePath, imageInBytes);
	}

	public void Save(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = _imageGenerationService.GenerateImage(moveHistoryObject, imageConfig );

		FileHelper.WriteBytesToFile(sanitizedFilePath, imageInBytes);
	}

	public void Save(string filePath, Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = _imageGenerationService.GenerateImage(
			position,
			previousLocation: previousLocation,
			currentLocation: currentLocation,
			imageConfig);

		FileHelper.WriteBytesToFile(sanitizedFilePath, imageInBytes);
	}

	public async Task SaveAsync(string filePath, string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = await _imageGenerationService.GenerateImageAsync(
			fen,
			previousLocation: previousLocation,
			currentLocation: currentLocation,
			imageConfig, 
			cancellationToken);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, imageInBytes, cancellationToken);
	}

	public async Task SaveAsync(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = await _imageGenerationService.GenerateImageAsync(moveHistoryObject, imageConfig, cancellationToken);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, imageInBytes, cancellationToken);
	}

	public async Task SaveAsync(string filePath, Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = PrepareFilePath(filePath);

		byte[] imageInBytes = await _imageGenerationService.GenerateImageAsync(
			position,
			previousLocation: previousLocation,
			currentLocation: currentLocation,
			imageConfig, 
			cancellationToken);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, imageInBytes, cancellationToken);
	}

	private string PrepareFilePath(string filePath)
		=> FileHelper.PrepareFilePath(filePath, ".jpg", defaultFileName: DEFAULT_FILE_NAME);
}
