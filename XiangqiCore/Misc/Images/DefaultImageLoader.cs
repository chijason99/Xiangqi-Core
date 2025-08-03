using System.Reflection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XiangqiCore.Game;
using XiangqiCore.Misc.Images.Interfaces;

namespace XiangqiCore.Misc.Images;

/// <summary>
///  A default implementation of the IImageLoader interface that loads images from embedded resources.
/// </summary>
public class DefaultImageLoader : IImageLoader
{
    public Image<Rgba32>? LoadImage(string resourceName)
    {
        using Stream stream = Assembly.GetAssembly(typeof(XiangqiBuilder))!.GetManifestResourceStream(resourceName)
                              ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");

        return Image.Load<Rgba32>(stream);
    }
}