using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace XiangqiCore.Misc.Images.Interfaces;

/// <summary>
/// An interface for loading images. For future support of different image sources or formats by the user.
/// </summary>
public interface IImageLoader
{
    Image<Rgba32>? LoadImage(string resourceName);
}