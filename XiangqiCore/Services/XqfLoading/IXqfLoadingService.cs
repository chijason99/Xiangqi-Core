using System.IO;
using XiangqiCore.Game;

namespace XiangqiCore.Services.XqfLoading;

public interface IXqfLoadingService
{
	/// <summary>
	/// Loads an XQF game from the specified file path.
	/// </summary>
	/// <param name="filePath">The path of the XQF file to load.</param>
	/// <returns>The loaded <see cref="XiangqiGame"/>.</returns>
	XiangqiGame LoadFromFile(string filePath);

	/// <summary>
	/// Loads an XQF game from the supplied stream.
	/// </summary>
	/// <param name="stream">The stream that contains XQF data.</param>
	/// <returns>The loaded <see cref="XiangqiGame"/>.</returns>
	XiangqiGame Load(Stream stream);
}
