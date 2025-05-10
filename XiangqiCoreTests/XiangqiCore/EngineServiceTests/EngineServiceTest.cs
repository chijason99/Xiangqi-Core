using Moq;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Services.Engine;
using XiangqiCore.Services.MoveTransalation;
using XiangqiCore.Services.ProcessManager;

namespace xiangqi_core_test.XiangqiCore.EngineServiceTests;

public static class EngineServiceTest
{
	[Fact]
	public static async Task StartEngine_ShouldStartProcess()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		mockProcessManager
			.Setup(pm => pm.StartAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);

		DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

		// Act
		await engineService.LoadEngineAsync(@"C:\path\to\engine.exe");

		// Assert
		mockProcessManager.Verify(pm => pm.StartAsync(@"C:\path\to\engine.exe"), Times.Once);
	}

	[Fact]
	public static void StopEngine_ShouldEndProcess()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		mockProcessManager
			.Setup(pm => pm.Stop())
			.Verifiable();

		DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

		// Act
		engineService.StopEngine();

		// Assert
		mockProcessManager.Verify(pm => pm.Stop(), Times.Once);
	}

	[Fact]
	public static async Task IsReady_ShouldReturnTrue_AfterStartingTheEngine()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		mockProcessManager
			.Setup(pm => pm.SendCommandAsync("isready"))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync("readyok", It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync("readyok");

		DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

		// Act
		bool isReady = await engineService.IsReady();

		// Assert
		Assert.True(isReady);
		mockProcessManager.Verify(pm => pm.SendCommandAsync("isready"), Times.Once);
		mockProcessManager.Verify(pm => pm.ReadResponseAsync("readyok", It.IsAny<TimeSpan?>(), It.IsAny<bool>()), Times.Once);
	}

	[Fact]
	public static async Task SetEngineConfigAsync_ShouldSucceed_WhenNoResponseIsReceived()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync(string.Empty); // No response

		DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

		// Act
		await engineService.SetEngineConfigAsync("Hash", "128");

		// Assert
		mockProcessManager.Verify(pm => pm.SendCommandAsync("setoption name Hash value 128"), Times.Once);
	}

	[Fact]
	public static async Task SetEngineConfigAsync_ShouldThrowException_WhenErrorResponseIsReceived()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync("No such option: InvalidOption"); // Error response

		DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() =>
			engineService.SetEngineConfigAsync("InvalidOption", "Value"));
	}

	//[Fact]
	//public static async Task SendCustomCommand_ShouldReturnResponse_WithDefaultHandler()
	//{
	//	// Arrange
	//	var mockProcessManager = new Mock<IProcessManager>();
	//	var mockMoveTranslationService = new Mock<IMoveTranslationService>();

	//	mockProcessManager
	//		.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
	//		.Returns(Task.CompletedTask);
	//	mockProcessManager
	//		.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
	//		.ReturnsAsync("Custom Response");

	//	DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

	//	// Act
	//	string response = await engineService.SendCustomCommandAsync("customcommand");

	//	// Assert
	//	Assert.Equal("Custom Response", response);
	//}

	//[Fact]
	//public static async Task SendCustomCommand_ShouldUseCustomResponseHandler()
	//{
	//	// Arrange
	//	var mockProcessManager = new Mock<IProcessManager>();
	//	var mockMoveTranslationService = new Mock<IMoveTranslationService>();

	//	mockProcessManager
	//		.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
	//		.Returns(Task.CompletedTask);
	//	mockProcessManager
	//		.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
	//		.ReturnsAsync("Custom Response");

	//	DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

	//	// Act
	//	string response = await engineService.SendCustomCommandAsync(
	//		"customcommand",
	//		response => response.Contains("Custom")
	//	);

	//	// Assert
	//	Assert.Equal("Custom Response", response);
	//}

	[Fact]
	public static async Task SuggestMove_ShouldReturnTheBestMove()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		// Simulate sending the "position" command
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.Is<string>(cmd => cmd.StartsWith("position"))))
			.Returns(Task.CompletedTask);

		// Simulate sending the "go" command
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.Is<string>(cmd => cmd.StartsWith("go"))))
			.Returns(Task.CompletedTask);

		IEnumerable<string> responses = [
			"info depth 1 seldepth 1 multipv 1 score cp 109 nodes 43 nps 43000 tbhits 0 time 1 pv i10h10",
			"info depth 2 seldepth 2 multipv 1 score cp 135 nodes 88 nps 88000 tbhits 0 time 1 pv i10h10 a1a2 b8b1",
			"info depth 3 seldepth 3 multipv 1 score cp 107 nodes 146 nps 73000 tbhits 0 time 2 pv i10h10 i1i2 f10e9",
			"info depth 4 seldepth 4 multipv 1 score cp 100 nodes 244 nps 122000 tbhits 0 time 2 pv i10h10 b3e3 b8e8",
			"info depth 5 seldepth 5 multipv 1 score cp 148 nodes 361 nps 180500 tbhits 0 time 2 pv i10h10 b3e3 b8e8",
			"info depth 6 seldepth 6 multipv 1 score cp 39 nodes 1287 nps 257400 tbhits 0 time 5 pv i10h10 b3e3 b10c8 h1g3 a10b10 i1h1",
			"info depth 7 seldepth 7 multipv 1 score cp 80 nodes 1723 nps 287166 tbhits 0 time 6 pv i10h10 b3e3 b8g8 a1a2 b10c8",
			"info depth 8 seldepth 9 multipv 1 score cp 90 nodes 4542 nps 378500 tbhits 0 time 12 pv i10h10 b3e3 h8h2 e3e7 h2h6 g1e3 h6a6",
			"info depth 9 seldepth 11 multipv 1 score cp 76 nodes 12036 nps 445777 tbhits 0 time 27 pv i10h10 b3e3 h8e8 h1g3 b8b3 i1h1 h10h1 e3e7 d10e9",
			"info depth 10 seldepth 12 multipv 1 score cp 106 nodes 13136 nps 437866 tbhits 0 time 30 pv i10h10 b3e3 h8e8 h1g3 b8b3 i1h1 h10h1 g3h1",
			"bestmove i10h10 ponder b3e3"
		];

		// Simulate the engine's response to the "go" command
		mockProcessManager
			.Setup(pm => pm.ReadResponsesAsync(It.IsAny<Func<string, bool>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.Returns(MockAsyncEnumerable(responses));

		mockMoveTranslationService
			.Setup(s => s.TranslateMove(It.IsAny<MoveHistoryObject>(), It.IsAny<MoveNotationType>()))
			.Returns("r9=8");

		DefaultUciEngineService engineService = new(mockProcessManager.Object, mockMoveTranslationService.Object);

		EngineAnalysisOptions options = new()
		{
			Depth = 10
		};

		// Act
		string bestMove = await engineService.SuggestMoveAsync(
			"rnbakabCr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C7/9/RNBAKABNR b - - 0 1", 
			options,
			MoveNotationType.English);

		// Assert
		Assert.Equal("r9=8", bestMove);

		// Verify that the correct commands were sent
		mockProcessManager.Verify(pm => pm.SendCommandAsync(It.Is<string>(cmd => cmd.StartsWith("position"))), Times.Once);
		mockProcessManager.Verify(pm => pm.SendCommandAsync(It.Is<string>(cmd => cmd.StartsWith("go"))), Times.Once);
		mockProcessManager.Verify(pm => pm.ReadResponsesAsync(It.IsAny<Func<string, bool>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public static async Task AnalyzePositionAsync_ShouldReturnTheAnalysisResult()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		var mockMoveTranslationService = new Mock<IMoveTranslationService>();

		// Simulate UCI engine responses
		IEnumerable<string> responses = [
			"info depth 1 seldepth 1 multipv 1 score cp 109 nodes 43 nps 43000 tbhits 0 time 1 pv i10h10",
			"info depth 2 seldepth 2 multipv 1 score cp 135 nodes 88 nps 88000 tbhits 0 time 1 pv i10h10 a1a2 b8b1",
			"info depth 3 seldepth 3 multipv 1 score cp 107 nodes 146 nps 73000 tbhits 0 time 2 pv i10h10 i1i2 f10e9",
			"info depth 4 seldepth 4 multipv 1 score cp 100 nodes 244 nps 122000 tbhits 0 time 2 pv i10h10 b3e3 b8e8",
		];

		mockProcessManager
			.Setup(pm => pm.ReadResponsesAsync(It.IsAny<Func<string, bool>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.Returns(MockAsyncEnumerable(responses));

		// Simulate move translation
		mockMoveTranslationService
			.Setup(mt => mt.TranslateMove(It.IsAny<MoveHistoryObject>(), MoveNotationType.English))
			.Returns((MoveHistoryObject move, MoveNotationType _) => move.ToString());

		var engineService = new DefaultUciEngineService(mockProcessManager.Object, mockMoveTranslationService.Object);

		// Act
		var results = new List<AnalysisResult>();
		await foreach (var result in engineService.AnalyzePositionAsync("rnbakabCr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C7/9/RNBAKABNR b - - 0 1", MoveNotationType.English))
			results.Add(result);

		// Assert
		Assert.Equal(4, results.Count);

		// Verify the first result
		Assert.Equal(1, results[0].Depth);
		Assert.Equal(109, results[0].Score);
		Assert.Equal(1, results[0].TimeSpent);

		// Verify the second result
		Assert.Equal(2, results[1].Depth);
		Assert.Equal(135, results[1].Score);
		Assert.Equal(1, results[1].TimeSpent);

		// Verify the third result
		Assert.Equal(3, results[2].Depth);
		Assert.Equal(107, results[2].Score);
		Assert.Equal(2, results[2].TimeSpent);

		// Verify the third result
		Assert.Equal(4, results[3].Depth);
		Assert.Equal(100, results[3].Score);
		Assert.Equal(2, results[3].TimeSpent);

		// Verify that the process manager was called
		mockProcessManager.Verify(pm => pm.SendCommandAsync(It.Is<string>(cmd => cmd.StartsWith("position"))), Times.Once);
		mockProcessManager.Verify(pm => pm.SendCommandAsync("go infinite"), Times.Once);
		mockProcessManager.Verify(pm => pm.ReadResponsesAsync(It.IsAny<Func<string, bool>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
	}


	public static async IAsyncEnumerable<T> MockAsyncEnumerable<T>(IEnumerable<T> responses) 
	{
		foreach (T response in responses)
			yield return response;

		await Task.CompletedTask;
	}
}
