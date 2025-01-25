using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
using XiangqiCore.Services.ImageGeneration;

namespace XiangqiCore.Services.GifGeneration;

public class DefaultGifGenerationService : IGifGenerationService
{
	private readonly IImageGenerationService _imageGenerationService;

	public DefaultGifGenerationService(IImageGenerationService imageGenerationService)
	{
		_imageGenerationService = imageGenerationService;
	}

	public DefaultGifGenerationService()
	{
		_imageGenerationService = new DefaultImageGenerationService();
	}

	private Image<Rgba32> GenerateGifCore(IEnumerable<string>? fens = null, List<MoveHistoryObject>? moveHistory = null, ImageConfig? imageConfig = null)
	{
		if (fens is null && moveHistory is null)
			throw new ArgumentNullException("Both fens and moveHistory cannot be null");

		imageConfig ??= new();

		List<string> fensList = moveHistory is not null ? [.. moveHistory.Select(x => x.FenAfterMove)] : fens!.ToList();

		// Note : Will have to dispose the image after use
		Image<Rgba32> gif = new(width: ImageConfig.DefaultBoardWidth, height: ImageConfig.DefaultBoardHeight);
		GifMetadata gifMetaData = gif.Metadata.GetGifMetadata();

		// Infinite loop
		gifMetaData.RepeatCount = 0;

		int frameDelayInCentiSeconds = (int)Math.Ceiling((decimal)imageConfig!.FrameDelayInSecond * 100);

		// Set the delay until the next image is displayed.
		GifFrameMetadata metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
		metadata.FrameDelay = frameDelayInCentiSeconds;

		for (int i = 0; i < fensList.Count; i++)
		{
			string fen = fensList[i];

			Coordinate? previousLocation = null;
			Coordinate? currentLocation = null;

			// If the move indicator is enabled and it is not the initial FEN
			if (imageConfig.UseMoveIndicator && i > 0 && moveHistory is not null)
			{
				previousLocation = moveHistory[i].StartingPosition;
				currentLocation = moveHistory[i].Destination;
			}

			Piece[,] position = FenHelper.CreatePositionFromFen(fen);

			byte[] imageBytes = _imageGenerationService.GenerateImage(
				position, 
				previousLocation, 
				currentLocation, 
				imageConfig);

			using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);
			var frame = image.Frames.CloneFrame(0);

			// Set the delay until the next image is displayed.
			metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
			metadata.FrameDelay = frameDelayInCentiSeconds;

			gif.Frames.AddFrame(image.Frames.RootFrame);
		}

		return gif;
	}

	private async Task<Image<Rgba32>> GenerateGifCoreAsync(
		IEnumerable<string>? fens = null,
		List<MoveHistoryObject>? moveHistory = null,
		ImageConfig? imageConfig = null,
		CancellationToken cancellationToken = default)
	{
		if (fens is null && moveHistory is null)
			throw new ArgumentNullException("Both fens and moveHistory cannot be null");

		imageConfig ??= new();

		List<string> fensList = moveHistory is not null ? [.. moveHistory.Select(x => x.FenAfterMove)] : fens!.ToList();

		// Note : Will have to dispose the image after use
		Image<Rgba32> gif = new(width: ImageConfig.DefaultBoardWidth, height: ImageConfig.DefaultBoardHeight);
		GifMetadata gifMetaData = gif.Metadata.GetGifMetadata();

		// Infinite loop
		gifMetaData.RepeatCount = 0;

		int frameDelayInCentiSeconds = (int)Math.Ceiling((decimal)imageConfig!.FrameDelayInSecond * 100);

		// Set the delay until the next image is displayed.
		GifFrameMetadata metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
		metadata.FrameDelay = frameDelayInCentiSeconds;

		byte[][] imageBytesArray = new byte[fensList.Count][];

		await Parallel.ForAsync(
		fromInclusive: 0,
		toExclusive: fensList.Count,
		cancellationToken,
		async (i, ct) =>
		{
			string fen = fensList[i];

			Coordinate? previousLocation = null;
			Coordinate? currentLocation = null;

			// If the move indicator is enabled and it is not the initial FEN
			if (imageConfig.UseMoveIndicator && i > 0 && moveHistory is not null)
			{
				previousLocation = moveHistory[i].StartingPosition;
				currentLocation = moveHistory[i].Destination;
			}

			byte[] imageBytes = await _imageGenerationService.GenerateImageAsync(
						fen,
						previousLocation: previousLocation,
						currentLocation: currentLocation,
						imageConfig,
						cancellationToken);

			imageBytesArray[i] = imageBytes;
		});

		foreach (byte[] imageBytes in imageBytesArray)
		{
			using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);
			var frame = image.Frames.CloneFrame(0);

			// Set the delay until the next image is displayed.
			metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
			metadata.FrameDelay = frameDelayInCentiSeconds;

			gif.Frames.AddFrame(image.Frames.RootFrame);
		}

		return gif;
	}

	public byte[] GenerateGif(IEnumerable<string> fens, ImageConfig? imageConfig = null)
	{
		using Image<Rgba32> gif = GenerateGifCore(fens, imageConfig: imageConfig);
		using MemoryStream memoryStream = new();

		gif.Save(memoryStream, new GifEncoder());

		return memoryStream.ToArray();
	}

	public async Task<byte[]> GenerateGifAsync(IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		using Image<Rgba32> gif = await GenerateGifCoreAsync(
			fens, 
			imageConfig: imageConfig, 
			cancellationToken: cancellationToken);

		using MemoryStream memoryStream = new();

		await gif.SaveAsync(memoryStream, new GifEncoder(), cancellationToken);

		return memoryStream.ToArray();
	}

	public byte[] GenerateGif(List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null)
	{
		using Image<Rgba32> gif = GenerateGifCore(moveHistory: moveHistory, imageConfig: imageConfig);
		using MemoryStream memoryStream = new();

		gif.Save(memoryStream, new GifEncoder());

		return memoryStream.ToArray();
	}

	public async Task<byte[]> GenerateGifAsync(List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		using Image<Rgba32> gif = await GenerateGifCoreAsync(
			moveHistory: moveHistory, 
			imageConfig: imageConfig,
			cancellationToken: cancellationToken);

		using MemoryStream memoryStream = new();

		await gif.SaveAsync(memoryStream, new GifEncoder(), cancellationToken);

		return memoryStream.ToArray();
	}

	public byte[] GenerateGif(XiangqiGame game, ImageConfig? imageConfig = null)
	{
		List<MoveHistoryObject> moveHistory = [game.InitialState, ..game.MoveHistory];

		return GenerateGif(moveHistory, imageConfig);
	}

	public async Task<byte[]> GenerateGifAsync(XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		List<MoveHistoryObject> moveHistory = [game.InitialState, .. game.MoveHistory];

		return await GenerateGifAsync(moveHistory, imageConfig, cancellationToken);
	}
}