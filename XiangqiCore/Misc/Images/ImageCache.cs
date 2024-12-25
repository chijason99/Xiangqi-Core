using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;
using System.Reflection;

namespace XiangqiCore.Misc.Images;

public static class ImageCache
{
	private static readonly ConcurrentDictionary<string, Image<Rgba32>> _imageCache = [];

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
}
