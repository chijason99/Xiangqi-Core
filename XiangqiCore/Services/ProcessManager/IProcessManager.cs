namespace XiangqiCore.Services.ProcessManager;

public interface IProcessManager
{
	// Process lifecycle management
	public Task StartAsync(string enginePath);
	public void Stop();

	// Command communication
	public Task SendCommandAsync(string command);
	public Task<string> ReadResponseAsync(Func<string, bool> stopCondition, TimeSpan? timeout = null);
	public Task<string> ReadResponseAsync(string expectedResponse, TimeSpan? timeout = null);

	// Process status
	public bool IsRunning { get; }

	// Error handling
	public event EventHandler<string> OnErrorReceived;
}