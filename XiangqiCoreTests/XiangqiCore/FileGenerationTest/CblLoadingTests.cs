using System.IO;
using XiangqiCore.Game;
using XiangqiCore.Services.CblLoading;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public class CblLoadingTests
{
	private const int SampleIndexOffset = 0x10440;
	private const int SampleIndexEntryLength = 0x114;
	private const int CbrBoardOffset = 0x848;

	private static readonly string[] ExpectedEntryTitles =
	[
		"先和方舟",
		"後勝王崇霖",
		"後勝面壁者",
		"先負衛俊發",
		"後勝劉方博",
		"先勝高永",
		"先負孫鵬彬"
	];

	[Fact]
	public void LoadFromFile_WhenProvidedSampleCblIsValid_ShouldExposeMetadataAndOpenSelectedEntry()
	{
		// Arrange
		ICblLoadingService cblLoadingService = new DefaultCblLoadingService();
		string filePath = GetSampleFilePath();

		// Act
		CblLibrary library = cblLoadingService.LoadFromFile(filePath);
		XiangqiGame openedGame = library.Entries[3].LoadGame();

		// Assert
		library.Title.Should().Be("2022全國城市大學生網絡團體賽");
		library.Entries.Should().HaveCount(ExpectedEntryTitles.Length);
		library.Entries.Select(entry => entry.EntryNumber).Should().Equal([1, 2, 3, 4, 5, 6, 7]);
		library.Entries.Select(entry => entry.Title).Should().Equal(ExpectedEntryTitles);
		library.Entries.Should().OnlyContain(entry => entry.PayloadOffset > 0);
		library.Entries.Should().OnlyContain(entry => entry.PayloadLength > 0);
		openedGame.GameName.Should().Be(ExpectedEntryTitles[3]);
		openedGame.GetMoveHistory(false, null).Count.Should().BeGreaterThan(0);
	}

	[Fact]
	public void LoadFromFile_WhenEmbeddedGamePayloadIsMalformed_ShouldDeferFailureUntilThatEntryIsOpened()
	{
		// Arrange
		ICblLoadingService cblLoadingService = new DefaultCblLoadingService();
		byte[] sampleFileContents = LoadSampleFileContents();
		CblLibrary sampleLibrary;

		using (MemoryStream sampleStream = new(sampleFileContents, writable: false))
		{
			sampleLibrary = cblLoadingService.Load(sampleStream);
		}

		byte[] malformedFileContents = (byte[])sampleFileContents.Clone();
		CblLibraryEntry malformedEntry = sampleLibrary.Entries[1];
		malformedFileContents[malformedEntry.PayloadOffset + CbrBoardOffset] = 0x7F;
		string tempFilePath = CreateWritableFilePath("lazy-cbl");
		File.WriteAllBytes(tempFilePath, malformedFileContents);

		// Act
		CblLibrary library = cblLoadingService.LoadFromFile(tempFilePath);

		// Assert
		library.Entries.Should().HaveCount(ExpectedEntryTitles.Length);
		library.Entries.Select(entry => entry.Title).Should().Equal(ExpectedEntryTitles);
		library.Entries[0].LoadGame().GameName.Should().Be(ExpectedEntryTitles[0]);
		Action openMalformedEntry = () => library.Entries[1].LoadGame();
		openMalformedEntry.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*unknown board piece code*");
	}

	[Fact]
	public void LoadFromStream_WhenIndexEntriesAreMissing_ShouldFallbackToEmbeddedCbrTitles()
	{
		// Arrange
		ICblLoadingService cblLoadingService = new DefaultCblLoadingService();
		byte[] fileContents = LoadSampleFileContents();
		Array.Clear(fileContents, SampleIndexOffset, SampleIndexEntryLength * ExpectedEntryTitles.Length);

		// Act
		using MemoryStream stream = new(fileContents);
		CblLibrary library = cblLoadingService.Load(stream);

		// Assert
		library.Entries.Should().HaveCount(ExpectedEntryTitles.Length);
		library.Entries.Select(entry => entry.Title).Should().Equal(ExpectedEntryTitles);
	}

	[Fact]
	public void LoadFromStream_WhenHeaderSignatureIsInvalid_ShouldThrowHelpfulMessage()
	{
		// Arrange
		ICblLoadingService cblLoadingService = new DefaultCblLoadingService();
		byte[] fileContents = LoadSampleFileContents();
		fileContents[0] = (byte)'X';

		// Act
		Action action = () =>
		{
			using MemoryStream stream = new(fileContents);
			cblLoadingService.Load(stream);
		};

		// Assert
		action.Should()
			.Throw<InvalidDataException>()
			.WithMessage("*expected CBL header signature*");
	}

	private static byte[] LoadSampleFileContents()
		=> File.ReadAllBytes(GetSampleFilePath());

	private static string CreateWritableFilePath(string prefix)
		=> Path.Combine(AppContext.BaseDirectory, $"{prefix}-{Guid.NewGuid():N}.cbl");

	private static string GetSampleFilePath()
		=> Path.Combine(AppContext.BaseDirectory, "Assets", "2022全國城市大學生網絡團體賽.CBL");
}
