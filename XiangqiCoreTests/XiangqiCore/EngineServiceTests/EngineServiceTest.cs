using Moq;
using XiangqiCore.Services.Engine;
using XiangqiCore.Services.ProcessManager;

namespace xiangqi_core_test.XiangqiCore.EngineServiceTests;

public static class EngineServiceTest
{
	[Fact]
	public static async Task StartEngine_ShouldStartProcess()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		mockProcessManager
			.Setup(pm => pm.StartAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);

		var engineService = new UciEngineService(mockProcessManager.Object);

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
		mockProcessManager
			.Setup(pm => pm.Stop())
			.Verifiable();

		var engineService = new UciEngineService(mockProcessManager.Object);

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
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync("isready"))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync("readyok", It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync("readyok");

		var engineService = new UciEngineService(mockProcessManager.Object);

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
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync(string.Empty); // No response

		var engineService = new UciEngineService(mockProcessManager.Object);

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
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync("No such option: InvalidOption"); // Error response

		var engineService = new UciEngineService(mockProcessManager.Object);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() =>
			engineService.SetEngineConfigAsync("InvalidOption", "Value"));
	}

	[Fact]
	public static async Task SendCustomCommand_ShouldReturnResponse_WithDefaultHandler()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync("Custom Response");

		var engineService = new UciEngineService(mockProcessManager.Object);

		// Act
		string response = await engineService.SendCustomCommandAsync("customcommand");

		// Assert
		Assert.Equal("Custom Response", response);
	}

	[Fact]
	public static async Task SendCustomCommand_ShouldUseCustomResponseHandler()
	{
		// Arrange
		var mockProcessManager = new Mock<IProcessManager>();
		mockProcessManager
			.Setup(pm => pm.SendCommandAsync(It.IsAny<string>()))
			.Returns(Task.CompletedTask);
		mockProcessManager
			.Setup(pm => pm.ReadResponseAsync(It.IsAny<Func<string, bool>>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>()))
			.ReturnsAsync("Custom Response");

		var engineService = new UciEngineService(mockProcessManager.Object);

		// Act
		string response = await engineService.SendCustomCommandAsync(
			"customcommand",
			response => response.Contains("Custom")
		);

		// Assert
		Assert.Equal("Custom Response", response);
	}

	[Fact]
	public static async Task SuggestMove_ShouldReturnTheBestMove()
	{
		// Arrange
		var processManager = new DefaultProcessManager();
		var engineService = new UciEngineService(processManager);

		// Act
		await engineService.LoadEngineAsync(@"C:\Users\JASON\Downloads\fairy-stockfish_x86-64-modern.exe");
		await engineService.SetEngineConfigAsync("UCI_Variant", "xiangqi");

		EngineAnalysisOptions options = new()
		{
			Depth = 10
		};

		string bestMove = await engineService.SuggestMoveAsync("rnbakabCr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C7/9/RNBAKABNR b - - 0 1", options);

		engineService.StopEngine();

		// Assert
		Assert.False(string.IsNullOrEmpty(bestMove));
	}
}
