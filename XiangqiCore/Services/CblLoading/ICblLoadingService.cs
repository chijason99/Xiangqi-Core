using System.IO;

namespace XiangqiCore.Services.CblLoading;

/// <summary>
/// Loads a CCBridge library (.cbl) file that contains one or more embedded CBR games.
/// </summary>
public interface ICblLoadingService
{
	/// <summary>
	/// Loads a CBL library from the specified file path.
	/// </summary>
	/// <param name="filePath">The path of the CBL file to load.</param>
	/// <returns>The loaded <see cref="CblLibrary"/>.</returns>
	CblLibrary LoadFromFile(string filePath);

	/// <summary>
	/// Loads a CBL library from the supplied stream.
	/// </summary>
	/// <param name="stream">The stream that contains CBL data.</param>
	/// <returns>The loaded <see cref="CblLibrary"/>.</returns>
	CblLibrary Load(Stream stream);
}
