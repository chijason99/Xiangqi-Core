using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Reflection;
using XiangqiCore.Extension;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc;

public static class ImageCache
{
	private static readonly Dictionary<string, Image<Rgba32>> _imageCache = [];

	public static Image<Rgba32> GetImage(string resourceName)
	{
		if (!_imageCache.TryGetValue(resourceName, out var image))
		{
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
				?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
			image = Image.Load<Rgba32>(stream);
			_imageCache[resourceName] = image;
		}

		return image.Clone();
	}

	public static void PreloadImages()
	{
		List<string> pieceTypes = EnumHelper<PieceType>.GetAllNames();
		List<string> sides = EnumHelper<Side>.GetAllNames();

		foreach (string side in sides)
		{
			foreach (string pieceType in pieceTypes)
			{
				string imageName = $"{side}_{pieceType}.png".ToLower();
				string resourcePath = $"XiangqiCore.Assets.Board.Pieces.{imageName}";
				GetImage(resourcePath);
			}
		}

		// Preload board image
		GetImage("XiangqiCore.Assets.Board.board.png");
	}
}
