using System;
using XiangqiCore.Game;

namespace XiangqiCore.Services.CblLoading;

/// <summary>
/// Represents a CCBridge library and the Xiangqi games embedded in it.
/// </summary>
/// <param name="Title">The library title, when available.</param>
/// <param name="Entries">The embedded game entries.</param>
public sealed record CblLibrary(
	string? Title,
	IReadOnlyList<CblLibraryEntry> Entries);

/// <summary>
/// Represents a single game entry within a CCBridge library.
/// </summary>
/// <param name="EntryNumber">The 1-based position of the entry within the library.</param>
/// <param name="Title">The display title for the entry.</param>
/// <param name="PayloadOffset">The byte offset of the embedded CBR payload within the source CBL payload.</param>
/// <param name="PayloadLength">The byte length of the embedded CBR payload.</param>
public sealed record CblLibraryEntry
{
	private readonly Func<XiangqiGame> _loadGame;

	/// <summary>
	/// Initializes a new instance of the <see cref="CblLibraryEntry"/> class.
	/// </summary>
	/// <param name="entryNumber">The 1-based position of the entry within the library.</param>
	/// <param name="title">The display title for the entry.</param>
	/// <param name="payloadOffset">The byte offset of the embedded CBR payload within the source CBL payload.</param>
	/// <param name="payloadLength">The byte length of the embedded CBR payload.</param>
	/// <param name="loadGame">The deferred loader that materializes the embedded Xiangqi game on demand.</param>
	public CblLibraryEntry(int entryNumber, string title, int payloadOffset, int payloadLength, Func<XiangqiGame> loadGame)
	{
		if (entryNumber <= 0)
			throw new ArgumentOutOfRangeException(nameof(entryNumber), entryNumber, "The entry number must be greater than zero.");

		ArgumentException.ThrowIfNullOrWhiteSpace(title);
		ArgumentOutOfRangeException.ThrowIfNegative(payloadOffset);

		if (payloadLength <= 0)
			throw new ArgumentOutOfRangeException(nameof(payloadLength), payloadLength, "The payload length must be greater than zero.");

		ArgumentNullException.ThrowIfNull(loadGame);

		EntryNumber = entryNumber;
		Title = title;
		PayloadOffset = payloadOffset;
		PayloadLength = payloadLength;
		_loadGame = loadGame;
	}

	/// <summary>
	/// Gets the 1-based position of the entry within the library.
	/// </summary>
	public int EntryNumber { get; }

	/// <summary>
	/// Gets the display title for the entry.
	/// </summary>
	public string Title { get; }

	/// <summary>
	/// Gets the byte offset of the embedded CBR payload within the source CBL payload.
	/// </summary>
	public int PayloadOffset { get; }

	/// <summary>
	/// Gets the byte length of the embedded CBR payload.
	/// </summary>
	public int PayloadLength { get; }

	/// <summary>
	/// Materializes the embedded Xiangqi game for this entry.
	/// </summary>
	/// <returns>The loaded <see cref="XiangqiGame"/>.</returns>
	public XiangqiGame LoadGame() => _loadGame();
}
