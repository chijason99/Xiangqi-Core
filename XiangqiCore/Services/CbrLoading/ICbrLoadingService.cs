using System.IO;
using XiangqiCore.Game;

namespace XiangqiCore.Services.CbrLoading;

public interface ICbrLoadingService
{
	/// <summary>
	/// Loads a CBR game from the specified file path.
	/// </summary>
	/// <param name="filePath">The path of the CBR file to load.</param>
	/// <returns>The loaded <see cref="XiangqiGame"/>.</returns>
	XiangqiGame LoadFromFile(string filePath);

	/// <summary>
	/// Loads a CBR game from the supplied stream.
	/// </summary>
	/// <param name="stream">The stream that contains CBR data.</param>
	/// <returns>The loaded <see cref="XiangqiGame"/>.</returns>
	XiangqiGame Load(Stream stream);
}
