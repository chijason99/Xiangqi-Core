using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;
using System.Reflection;
using XiangqiCore.Game;

namespace XiangqiCore.Misc.Images;

public class ImageCache
{
	private readonly ConcurrentDictionary<string, Image<Rgba32>> _imageCache = [];

	public Image<Rgba32> GetImage(string resourceName)
	{
		if (!_imageCache.TryGetValue(resourceName, out var image))
		{
			using Stream stream = Assembly.GetAssembly(typeof(XiangqiBuilder))!.GetManifestResourceStream(resourceName)
				?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");

			image = Image.Load<Rgba32>(stream);
			_imageCache[resourceName] = image;
		}

		return image.Clone();
	}
}
