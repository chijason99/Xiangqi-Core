using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiangqiCore.Misc;

public static class FilePathHelper
{
	public static string PrepareFilePath(string filePath, string fileExtension, string defaultFileName = "Unknown")
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fileExtension, nameof(fileExtension));

		if (!Path.IsPathFullyQualified(filePath))
			throw new ArgumentException("The specified file path is not fully qualified.");

		string directoryPath = Path.GetDirectoryName(filePath);
		string fileName = Path.GetFileName(filePath);

		if (string.IsNullOrWhiteSpace(fileName))
			fileName = $"{defaultFileName}.{fileExtension}";
		else
		{
			string providedExtension = Path.GetExtension(fileName);

			if (!string.Equals(providedExtension, $".{fileExtension}", StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException($"The file extension '{providedExtension}' does not match the expected extension '.{fileExtension}'.");
		}

		if (!Directory.Exists(directoryPath))
			Directory.CreateDirectory(directoryPath);

		char[] invalidFileCharacters = Path.GetInvalidFileNameChars();

		string sanitizedFileName = string.Concat(fileName.Select(character =>
		{
			return invalidFileCharacters.Contains(character) ? '_' : character;
		}));

		return Path.Combine(directoryPath, sanitizedFileName);
	}
}
