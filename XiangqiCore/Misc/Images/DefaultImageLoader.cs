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
        if (Uri.TryCreate(resourceName, UriKind.Absolute, out Uri? uri) && uri.IsFile)
        {
            using Stream fileStream = File.OpenRead(uri.LocalPath);
            return Image.Load<Rgba32>(fileStream);
        }

        if (Path.IsPathRooted(resourceName) || File.Exists(resourceName))
        {
            using Stream fileStream = File.OpenRead(resourceName);
            return Image.Load<Rgba32>(fileStream);
        }

        using Stream stream = Assembly.GetAssembly(typeof(XiangqiBuilder))!.GetManifestResourceStream(resourceName)
                              ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");

        return Image.Load<Rgba32>(stream);
    }
}
