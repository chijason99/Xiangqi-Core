﻿namespace XiangqiCore.Misc;

public static class FileHelper
{
	public static string PrepareFilePath(string filePath, string expectedExtension, string defaultFileName = "Unknown")
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(expectedExtension, nameof(expectedExtension));

		if (!Path.IsPathFullyQualified(filePath))
			throw new ArgumentException("The specified file path is not fully qualified.");

		string directoryPath = Path.GetDirectoryName(filePath);
		string fileName = Path.GetFileName(filePath);

		if (string.IsNullOrWhiteSpace(fileName))
			fileName = $"{defaultFileName}.{expectedExtension}";
		else
		{
			string providedExtension = Path.GetExtension(fileName);

			if (!string.Equals(providedExtension, $".{expectedExtension}", StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException($"The file extension '{providedExtension}' does not match the expected extension '.{expectedExtension}'.");
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

	public static void WriteBytesToFile(string filePath, byte[] bytes)
	{
		try
		{
			File.WriteAllBytes(filePath, bytes);
		}
		catch (Exception ex)
		{
			throw new IOException($"Failed to write to file {filePath}", ex);
		}
	}

	public static async Task WriteBytesToFileAsync(string filePath, byte[] bytes, CancellationToken cancellationToken = default)
	{
		try
		{
			await File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
		}
		catch (Exception ex)
		{
			throw new IOException($"Failed to write to file {filePath}", ex);
		}
	}

	public static string CreateTempDirectory(string folderName)
	{
		string tempDirectory = Path.Combine(Path.GetTempPath(), folderName);
		Directory.CreateDirectory(tempDirectory);

		return tempDirectory;
	}
}
