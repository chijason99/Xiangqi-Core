namespace XiangqiCore.Misc;

public static class FileHelper
{
	internal static byte[] ReadAllBytes(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		if (!stream.CanRead)
			throw new ArgumentException("The supplied stream must be readable.", nameof(stream));

		if (stream.CanSeek)
		{
			long remainingLength = stream.Length - stream.Position;

			if (remainingLength is < 0 or > int.MaxValue)
				throw new IOException("The supplied stream is too large to read into memory.");

			byte[] buffer = GC.AllocateUninitializedArray<byte>((int)remainingLength);
			stream.ReadExactly(buffer);
			return buffer;
		}

		using MemoryStream memoryStream = new();
		stream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

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
