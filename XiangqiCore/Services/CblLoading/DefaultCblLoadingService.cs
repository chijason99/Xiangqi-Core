using System.Buffers.Binary;
using System.IO;
using System.Text;
using XiangqiCore.Game;
using XiangqiCore.Services.CbrLoading;

namespace XiangqiCore.Services.CblLoading;

public sealed class DefaultCblLoadingService : ICblLoadingService
{
	private const int HeaderTitleLengthOffset = 0x3C;
	private const int HeaderTitleOffset = 0x40;
	private const int DefaultHeaderTitleLength = 128;
	private const int IndexEntryLength = 0x114;
	private const int IndexEntryUsedCbrLengthOffset = 0x0C;
	private const int IndexEntryIdentifierOffset = 0x14;
	private const int IndexEntryIdentifierLength = 80;
	private const int IndexEntryTitleOffset = 0x64;
	private const int IndexEntryTitleLength = 128;
	private const int MinimumCbrMetadataLength = 0x8AA;
	private const int StreamSearchBufferSize = 64 * 1024;
	private static readonly Encoding Utf16Encoding = Encoding.Unicode;

	private static ReadOnlySpan<byte> CblSignature => "CCBridgeLibrary\0"u8;
	private static ReadOnlySpan<byte> CbrSignature => "CCBridge Record\0"u8;

	private readonly DefaultCbrLoadingService _cbrLoadingService = new();

	/// <inheritdoc />
	public CblLibrary LoadFromFile(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		if (!File.Exists(filePath))
			throw new FileNotFoundException("The CBL file could not be found.", filePath);

		using FileStream stream = File.OpenRead(filePath);
		return LoadFromFile(filePath, stream);
	}

	/// <inheritdoc />
	public CblLibrary Load(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		if (!stream.CanRead)
			throw new ArgumentException("The supplied CBL stream must be readable.", nameof(stream));

		using MemoryStream buffer = new();
		stream.CopyTo(buffer);

		return Load(buffer.ToArray());
	}

	internal CblLibrary Load(byte[] fileContents)
	{
		ArgumentNullException.ThrowIfNull(fileContents);

		ValidateHeader(fileContents);

		int[] payloadOffsets = FindEmbeddedPayloadOffsets(fileContents);

		if (payloadOffsets.Length == 0)
			throw new InvalidDataException("The CBL does not contain any embedded CBR payloads.");

		List<CblIndexEntry> indexEntries = ParseIndexEntries(fileContents, payloadOffsets[0], payloadOffsets.Length);
		List<CblLibraryEntry> entries = new(payloadOffsets.Length);

		for (int index = 0; index < payloadOffsets.Length; index++)
		{
			CblIndexEntry? indexEntry = index < indexEntries.Count ? indexEntries[index] : null;
			(int payloadOffset, int payloadLength) = GetEmbeddedPayloadBounds(fileContents.Length, payloadOffsets, index, indexEntry);
			ReadOnlySpan<byte> embeddedPayload = fileContents.AsSpan(payloadOffset, payloadLength);
			string? entryTitle = !string.IsNullOrWhiteSpace(indexEntry?.Title)
				? indexEntry!.Title
				: _cbrLoadingService.ReadGameTitle(embeddedPayload);

			if (string.IsNullOrWhiteSpace(entryTitle))
				entryTitle = $"Game {index + 1}";

			entries.Add(new CblLibraryEntry(
				index + 1,
				entryTitle,
				payloadOffset,
				payloadLength,
				() => _cbrLoadingService.Load(fileContents.AsSpan(payloadOffset, payloadLength))));
		}

		return new CblLibrary(
			Title: ReadLibraryTitle(fileContents),
			Entries: entries);
	}

	private CblLibrary LoadFromFile(string filePath, FileStream stream)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
		ArgumentNullException.ThrowIfNull(stream);

		int fileLength = GetSupportedFileLength(stream);
		ValidateHeader(ReadBytes(stream, 0, Math.Min(fileLength, CblSignature.Length), "CBL header"));

		int[] payloadOffsets = FindEmbeddedPayloadOffsets(stream);

		if (payloadOffsets.Length == 0)
			throw new InvalidDataException("The CBL does not contain any embedded CBR payloads.");

		byte[] metadataPrefix = ReadBytes(stream, 0, payloadOffsets[0], "CBL metadata");
		List<CblIndexEntry> indexEntries = ParseIndexEntries(metadataPrefix, payloadOffsets[0], payloadOffsets.Length);
		List<CblLibraryEntry> entries = new(payloadOffsets.Length);

		for (int index = 0; index < payloadOffsets.Length; index++)
		{
			CblIndexEntry? indexEntry = index < indexEntries.Count ? indexEntries[index] : null;
			(int payloadOffset, int payloadLength) = GetEmbeddedPayloadBounds(fileLength, payloadOffsets, index, indexEntry);
			string? entryTitle = !string.IsNullOrWhiteSpace(indexEntry?.Title)
				? indexEntry!.Title
				: ReadEmbeddedGameTitle(stream, payloadOffset, payloadLength);

			if (string.IsNullOrWhiteSpace(entryTitle))
				entryTitle = $"Game {index + 1}";

			int entryNumber = index + 1;
			entries.Add(new CblLibraryEntry(
				entryNumber,
				entryTitle,
				payloadOffset,
				payloadLength,
				() => LoadEmbeddedGameFromFile(filePath, entryNumber, payloadOffset, payloadLength)));
		}

		return new CblLibrary(
			Title: ReadLibraryTitle(metadataPrefix),
			Entries: entries);
	}

	private static void ValidateHeader(ReadOnlySpan<byte> fileContents)
	{
		if (fileContents.Length < CblSignature.Length)
		{
			throw new InvalidDataException(
				$"The CBL file is too short. Expected at least {CblSignature.Length} bytes but found {fileContents.Length}.");
		}

		if (!fileContents[..CblSignature.Length].SequenceEqual(CblSignature))
			throw new InvalidDataException("The file does not start with the expected CBL header signature ('CCBridgeLibrary\\0').");
	}

	private static string? ReadLibraryTitle(ReadOnlySpan<byte> fileContents)
	{
		if (fileContents.Length <= HeaderTitleOffset)
			return null;

		int declaredLength = fileContents.Length >= HeaderTitleLengthOffset + sizeof(int)
			? BinaryPrimitives.ReadInt32LittleEndian(fileContents.Slice(HeaderTitleLengthOffset, sizeof(int)))
			: 0;

		if (declaredLength <= 0 || fileContents.Length - HeaderTitleOffset < declaredLength)
			declaredLength = Math.Min(DefaultHeaderTitleLength, fileContents.Length - HeaderTitleOffset);

		return ReadUtf16String(fileContents.Slice(HeaderTitleOffset, declaredLength));
	}

	private static int[] FindEmbeddedPayloadOffsets(ReadOnlySpan<byte> fileContents)
	{
		List<int> payloadOffsets = [];
		int searchStart = 0;

		while (searchStart <= fileContents.Length - CbrSignature.Length)
		{
			int relativeOffset = fileContents[searchStart..].IndexOf(CbrSignature);

			if (relativeOffset < 0)
				break;

			int absoluteOffset = searchStart + relativeOffset;
			payloadOffsets.Add(absoluteOffset);
			searchStart = absoluteOffset + CbrSignature.Length;
		}

		return [.. payloadOffsets];
	}

	private static int[] FindEmbeddedPayloadOffsets(FileStream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		stream.Position = 0;

		List<int> payloadOffsets = [];
		byte[] buffer = new byte[StreamSearchBufferSize + CbrSignature.Length - 1];
		int preservedLength = 0;
		int bytesConsumed = 0;

		while (true)
		{
			int bytesRead = stream.Read(buffer, preservedLength, buffer.Length - preservedLength);

			if (bytesRead == 0)
				break;

			int scanLength = preservedLength + bytesRead;
			ReadOnlySpan<byte> scanBuffer = buffer.AsSpan(0, scanLength);
			int searchStart = 0;

			while (searchStart <= scanLength - CbrSignature.Length)
			{
				int relativeOffset = scanBuffer[searchStart..].IndexOf(CbrSignature);

				if (relativeOffset < 0)
					break;

				int matchOffset = searchStart + relativeOffset;
				payloadOffsets.Add(bytesConsumed - preservedLength + matchOffset);
				searchStart = matchOffset + CbrSignature.Length;
			}

			preservedLength = Math.Min(CbrSignature.Length - 1, scanLength);

			if (preservedLength > 0)
				scanBuffer[^preservedLength..].CopyTo(buffer);

			bytesConsumed += bytesRead;
		}

		return [.. payloadOffsets];
	}

	private static List<CblIndexEntry> ParseIndexEntries(ReadOnlySpan<byte> fileContents, int searchEndExclusive, int expectedEntryCount)
	{
		if (searchEndExclusive < IndexEntryLength)
			return [];

		List<(int Offset, CblIndexEntry Entry)> candidates = [];

		for (int offset = CblSignature.Length; offset <= searchEndExclusive - IndexEntryLength; offset += 2)
		{
			if (TryParseIndexEntry(fileContents, offset, out CblIndexEntry? entry))
			{
				ArgumentNullException.ThrowIfNull(entry);
				candidates.Add((offset, entry));
			}
		}

		if (candidates.Count == 0)
			return [];

		int bestRunStart = 0;
		int bestRunLength = 1;
		int bestRunUsefulLength = Math.Min(1, expectedEntryCount);
		int currentRunStart = 0;
		int currentRunLength = 1;

		for (int index = 1; index < candidates.Count; index++)
		{
			if (candidates[index].Offset - candidates[index - 1].Offset == IndexEntryLength)
			{
				currentRunLength++;
			}
			else
			{
				currentRunStart = index;
				currentRunLength = 1;
			}

			int currentUsefulLength = Math.Min(currentRunLength, expectedEntryCount);

			if (currentUsefulLength > bestRunUsefulLength
				|| (currentUsefulLength == bestRunUsefulLength && currentRunLength > bestRunLength))
			{
				bestRunStart = currentRunStart;
				bestRunLength = currentRunLength;
				bestRunUsefulLength = currentUsefulLength;
			}
		}

		int resultCount = Math.Min(bestRunLength, expectedEntryCount);
		List<CblIndexEntry> entries = new(resultCount);

		for (int index = 0; index < resultCount; index++)
			entries.Add(candidates[bestRunStart + index].Entry);

		return entries;
	}

	private static bool TryParseIndexEntry(ReadOnlySpan<byte> fileContents, int offset, out CblIndexEntry? entry)
	{
		entry = null;

		if (fileContents.Length - offset < IndexEntryLength)
			return false;

		ReadOnlySpan<byte> entryBytes = fileContents.Slice(offset, IndexEntryLength);
		int usedCbrLength = BinaryPrimitives.ReadInt32LittleEndian(
			entryBytes.Slice(IndexEntryUsedCbrLengthOffset, sizeof(int)));

		if (usedCbrLength <= CbrSignature.Length)
			return false;

		string? identifier = ReadUtf16String(entryBytes.Slice(IndexEntryIdentifierOffset, IndexEntryIdentifierLength));

		if (string.IsNullOrWhiteSpace(identifier) || !Guid.TryParse(identifier, out _))
			return false;

		string? title = ReadUtf16String(entryBytes.Slice(IndexEntryTitleOffset, IndexEntryTitleLength));

		if (string.IsNullOrWhiteSpace(title))
			return false;

		entry = new CblIndexEntry(title, usedCbrLength);
		return true;
	}

	private static (int Offset, int Length) GetEmbeddedPayloadBounds(
		int fileLength,
		IReadOnlyList<int> payloadOffsets,
		int payloadIndex,
		CblIndexEntry? indexEntry)
	{
		int payloadOffset = payloadOffsets[payloadIndex];
		int payloadLimit = payloadIndex + 1 < payloadOffsets.Count
			? payloadOffsets[payloadIndex + 1]
			: fileLength;
		int fallbackLength = payloadLimit - payloadOffset;

		if (indexEntry is null
			|| indexEntry.UsedCbrLength <= CbrSignature.Length
			|| indexEntry.UsedCbrLength > fallbackLength)
		{
			return (payloadOffset, fallbackLength);
		}

		return (payloadOffset, indexEntry.UsedCbrLength);
	}

	private static int GetSupportedFileLength(FileStream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		if (stream.Length > int.MaxValue)
			throw new NotSupportedException("CBL files larger than 2 GB are not supported.");

		return checked((int)stream.Length);
	}

	private XiangqiGame LoadEmbeddedGameFromFile(string filePath, int entryNumber, int payloadOffset, int payloadLength)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
		ArgumentOutOfRangeException.ThrowIfNegative(payloadOffset);

		if (payloadLength <= 0)
			throw new ArgumentOutOfRangeException(nameof(payloadLength), payloadLength, "The payload length must be greater than zero.");

		using FileStream stream = File.OpenRead(filePath);
		byte[] payloadBytes = ReadBytes(stream, payloadOffset, payloadLength, $"embedded CBR payload for entry {entryNumber}");
		return _cbrLoadingService.Load(payloadBytes);
	}

	private string? ReadEmbeddedGameTitle(FileStream stream, int payloadOffset, int payloadLength)
	{
		ArgumentNullException.ThrowIfNull(stream);
		ArgumentOutOfRangeException.ThrowIfNegative(payloadOffset);

		if (payloadLength <= 0)
			throw new ArgumentOutOfRangeException(nameof(payloadLength), payloadLength, "The payload length must be greater than zero.");

		byte[] payloadMetadata = ReadBytes(
			stream,
			payloadOffset,
			Math.Min(payloadLength, MinimumCbrMetadataLength),
			$"embedded CBR metadata at offset 0x{payloadOffset:X}");
		return _cbrLoadingService.ReadGameTitle(payloadMetadata);
	}

	private static byte[] ReadBytes(FileStream stream, int offset, int length, string fieldName)
	{
		ArgumentNullException.ThrowIfNull(stream);
		ArgumentOutOfRangeException.ThrowIfNegative(offset);
		ArgumentOutOfRangeException.ThrowIfNegative(length);
		ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);

		if (length == 0)
			return [];

		if ((long)offset + length > stream.Length)
		{
			throw new InvalidDataException(
				$"The CBL ended unexpectedly while reading {fieldName} at offset 0x{offset:X}.");
		}

		stream.Position = offset;
		byte[] buffer = new byte[length];
		stream.ReadExactly(buffer);
		return buffer;
	}

	private static string? ReadUtf16String(ReadOnlySpan<byte> fieldBytes)
	{
		int evenLength = fieldBytes.Length - (fieldBytes.Length % 2);
		int stringLength = evenLength;

		for (int index = 0; index < evenLength - 1; index += 2)
		{
			if (fieldBytes[index] == 0x00 && fieldBytes[index + 1] == 0x00)
			{
				stringLength = index;
				break;
			}
		}

		if (stringLength == 0)
			return null;

		string value = Utf16Encoding.GetString(fieldBytes[..stringLength]).Trim('\0', ' ', '\r', '\n', '\t');
		return string.IsNullOrWhiteSpace(value) ? null : value;
	}

	private sealed record CblIndexEntry(
		string Title,
		int UsedCbrLength);
}
