using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;
using System.Reflection;
using XiangqiCore.Game;
using XiangqiCore.Misc.Images.Interfaces;

namespace XiangqiCore.Misc.Images;

public class ImageCache
{
	private readonly ConcurrentDictionary<string, Image<Rgba32>> _imageCache = [];
	private readonly IImageLoader _imageLoader;
	
	public ImageCache(IImageLoader? imageLoader = null)
	{
		_imageLoader = imageLoader ?? new DefaultImageLoader();
	}

	public Image<Rgba32> GetImage(string resourceName)
	{
		var cachedImage = _imageCache.GetOrAdd(
			resourceName, 
			key => _imageLoader.LoadImage(key) ?? 
						throw new FileNotFoundException($"Image resource '{key}' not found."));

		return cachedImage.Clone();
	}
}
