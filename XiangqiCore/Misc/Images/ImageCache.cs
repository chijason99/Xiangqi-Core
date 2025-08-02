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
		var cachedImage = _imageCache.GetOrAdd(resourceName, static key =>
		{
			using Stream stream = Assembly.GetAssembly(typeof(XiangqiBuilder))!.GetManifestResourceStream(key)
			                      ?? throw new FileNotFoundException($"Resource '{key}' not found.");

			return Image.Load<Rgba32>(stream);
		});

		return cachedImage.Clone();
	}
}
