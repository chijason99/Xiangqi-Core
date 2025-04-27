using XiangqiCore.Services.Engine;
using XiangqiCore.Services.ProcessManager;

namespace xiangqi_core_test.XiangqiCore.EngineServiceTests;

public static class EngineServiceTest
{
	[Fact]
	public static async Task StartEngine_ShouldStartProcess()
	{
		// Arrange
		var processManager = new DefaultProcessManager();
		var engineService = new UciEngineService(processManager);

		// Act
		await engineService.LoadEngineAsync(@"C:\Users\JASON\Downloads\fairy-stockfish_x86-64-modern.exe");

		// Assert
		processManager.IsRunning.Should().BeTrue();
	}

	[Fact]
	public static async Task StopEngine_ShouldEndProcess()
	{
		// Arrange
		var processManager = new DefaultProcessManager();
		var engineService = new UciEngineService(processManager);

		// Act
		await engineService.LoadEngineAsync(@"C:\Users\JASON\Downloads\fairy-stockfish_x86-64-modern.exe");
		processManager.IsRunning.Should().BeTrue();

		// Assert
		engineService.StopEngine();
		processManager.IsRunning.Should().BeFalse();
	}

	[Fact]
	public static async Task IsReady_ShouldReturnTrue_AfterStartingTheEngine()
	{
		// Arrange
		var processManager = new DefaultProcessManager();
		var engineService = new UciEngineService(processManager);

		await engineService.LoadEngineAsync(@"C:\Users\JASON\Downloads\fairy-stockfish_x86-64-modern.exe");

		// Act
		bool isReady = await engineService.IsReady();

		// Assert
		isReady.Should().BeTrue();

		engineService.StopEngine();
	}
}
