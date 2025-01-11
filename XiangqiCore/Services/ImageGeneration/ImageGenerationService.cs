using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Misc.Images.Interfaces;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Services.ImageGeneration;

public class ImageGenerationService(IImageResourcePathManager imageResourcePathManager, ImageCache imageCache) : IImageGenerationService
{
	private readonly IImageResourcePathManager _imageResourcePathManager = imageResourcePathManager;
	private readonly ImageCache _imageCache = imageCache;

	public void GenerateGif(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null)
	{
		Image<Rgba32> gif = GenerateGifCore(moveHistory: moveHistory);

		try
		{
			gif.SaveAsGif(filePath);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			gif.Dispose();
		}
	}

	public async Task GenerateGifAsync(
		string filePath, 
		List<MoveHistoryObject> moveHistory,
		ImageConfig? imageConfig = null,
		CancellationToken cancellationToken = default)
	{
		Image<Rgba32> gif = GenerateGifCore(moveHistory: moveHistory);

		try
		{
			await gif.SaveAsGifAsync(filePath, cancellationToken);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			gif.Dispose();
		}
	}

	public void GenerateGif(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null)
	{
		Image<Rgba32> gif = GenerateGifCore(fens: fens);

		try
		{
			gif.SaveAsGif(filePath);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			gif.Dispose();
		}
	}

	public async Task GenerateGifAsync(
		string filePath, 
		IEnumerable<string> fens,
		ImageConfig? imageConfig = null,
		CancellationToken cancellationToken = default)
	{
		Image<Rgba32> gif = GenerateGifCore(fens: fens);

		try
		{
			await gif.SaveAsGifAsync(filePath, cancellationToken);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			gif.Dispose();
		}
	}

	public void GenerateImage(string filePath, string fen, ImageConfig? imageConfig = null)
	{
		Image<Rgba32> image = GenerateImageCore(fen);

		image.Save(filePath);
	}

	public async Task GenerateImageAsync(string filePath, string fen, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)
	{
		Image<Rgba32> image = GenerateImageCore(fen);

		await image.SaveAsync(filePath, cancellationToken);
	}

	private Image<Rgba32> GenerateImageCore(string fen)
	{
		Piece[,] position = FenHelper.CreatePositionFromFen(fen);

		byte[] bytes = GenerateBoardImage(position);

		return Image.Load<Rgba32>(bytes);
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

			Coordinate? previousPosition = null;
			Coordinate? currentPosition = null;

			// If the move indicator is enabled and it is not the initial FEN
			if (imageConfig.UseMoveIndicator && i > 0 && moveHistory is not null)
			{
				previousPosition = moveHistory[i - 1].StartingPosition;
				currentPosition = moveHistory[i - 1].Destination;
			}

			Piece[,] position = FenHelper.CreatePositionFromFen(fen);

			byte[] imageBytes = GenerateBoardImage(
				position,
				previousPosition: previousPosition,
				currentPosition: currentPosition);

			using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);
			var frame = image.Frames.CloneFrame(0);

			// Set the delay until the next image is displayed.
			metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
			metadata.FrameDelay = frameDelayInCentiSeconds;

			gif.Frames.AddFrame(image.Frames.RootFrame);
		}

		return gif;
	}

	public byte[] GenerateBoardImage(Piece[,] position, Coordinate? previousPosition = null, Coordinate? currentPosition = null, ImageConfig? imageConfig = null)
	{
		PreloadImages();

		imageConfig ??= new();

		using Image<Rgba32> boardImage = _imageCache.GetImage(_imageResourcePathManager.GetBoardResourcePath());

		boardImage.Mutate(x => x.Resize(ImageConfig.DefaultBoardWidth, ImageConfig.DefaultBoardHeight));

		foreach (Piece piece in position.Cast<Piece>().Where(p => p is not EmptyPiece))
		{
			using Image<Rgba32> pieceImage = _imageCache.GetImage(
				_imageResourcePathManager.GetPieceResourcePath(piece.PieceType, piece.Side));

			(int xCoordinate, int yCoordinate) = GetCoordinatesAfterRotation(piece.Coordinate);

			boardImage.Mutate(ctx => ctx.DrawImage(pieceImage,
				new Point(xCoordinate * ImageConfig.DefaultSquareSize, yCoordinate * ImageConfig.DefaultSquareSize), 1f));
		}

		if (imageConfig.UseMoveIndicator)
		{
			if (previousPosition is not null)
			{
				using Image<Rgba32> moveIndicatorImage = _imageCache.GetImage(_imageResourcePathManager.GetMoveIndicatorResourcePath());

				(int xCoordinate, int yCoordinate) = GetCoordinatesAfterRotation(previousPosition.Value);

				boardImage.Mutate(ctx => ctx.DrawImage(moveIndicatorImage,
					new Point(xCoordinate * ImageConfig.DefaultSquareSize, yCoordinate * ImageConfig.DefaultSquareSize), 1f));
			}

			if (currentPosition is not null)
			{
				using Image<Rgba32> moveIndicatorImage = _imageCache.GetImage(_imageResourcePathManager.GetMoveIndicatorResourcePath());

				(int xCoordinate, int yCoordinate) = GetCoordinatesAfterRotation(currentPosition.Value);

				boardImage.Mutate(ctx => ctx.DrawImage(moveIndicatorImage,
					new Point(xCoordinate * ImageConfig.DefaultSquareSize, yCoordinate * ImageConfig.DefaultSquareSize), 1f));
			}
		}

		using MemoryStream memoryStream = new();
		boardImage.Save(memoryStream, new PngEncoder());

		return memoryStream.ToArray();
	}

	private void PreloadImages()
	{
		PieceType[] pieceTypes = Enum.GetValues(typeof(PieceType))
			.Cast<PieceType>()
			.Where(pt => pt != PieceType.None)
			.ToArray();

		Side[] sides = Enum.GetValues(typeof(Side))
			.Cast<Side>()
			.Where(pt => pt != Side.None)
			.ToArray();

		foreach (Side side in sides)
			foreach (PieceType pieceType in pieceTypes)
				_imageCache.GetImage(_imageResourcePathManager.GetPieceResourcePath(pieceType, side));

		// Preload board image
		_imageCache.GetImage(_imageResourcePathManager.GetBoardResourcePath());

		// Preload move indicator image
		_imageCache.GetImage(_imageResourcePathManager.GetMoveIndicatorResourcePath());
	}

	private (int xCoordinate, int yCoordinate) GetCoordinatesAfterRotation(Coordinate originalCoordinate, ImageConfig? imageConfig = null)
	{
		imageConfig ??= new();

		const int NUMBER_OF_ROWS = 10;
		const int NUMBER_OF_COLUMNS = 9;

		int xCoordinate = originalCoordinate.Column - 1;
		int yCoordinate = NUMBER_OF_ROWS - originalCoordinate.Row;

		// If flipping the board horizontally, both the x-coordinate and y-coordinate should be flipped
		// If flipping the board vertically, then the x-coordinate should be flipped
		// If flipping the board vertically and horizontally, then only the y-coordinate should be flipped because the x-coordinate is flipped twice
		if (imageConfig.FlipVertical && imageConfig.FlipHorizontal)
			yCoordinate = originalCoordinate.Row - 1;
		else if (imageConfig.FlipVertical)
			xCoordinate = NUMBER_OF_COLUMNS - originalCoordinate.Column;
		else if (imageConfig.FlipHorizontal)
		{
			yCoordinate = originalCoordinate.Row - 1;
			xCoordinate = NUMBER_OF_COLUMNS - originalCoordinate.Column;
		}

		return (xCoordinate, yCoordinate);
	}
}