using System.Runtime.CompilerServices;

namespace XiangqiCore.Services.ProcessManager;

public interface IProcessManager
{
	// Process lifecycle management
	public Task StartAsync(string enginePath);
	public void Stop();

	// Command communication
	public Task SendCommandAsync(string command);
	public Task<string> ReadResponseAsync(Func<string, bool> stopCondition, TimeSpan? timeout = null, bool stopOnEmtpyResponse = true);
	public IAsyncEnumerable<string> ReadResponsesAsync(Func<string, bool> stopCondition, bool stopOnEmtpyResponse = true, [EnumeratorCancellation] CancellationToken cancellationToken = default);
	public Task<string> ReadResponseAsync(string expectedResponse, TimeSpan? timeout = null, bool stopOnEmtpyResponse = true);

	// Process status
	public bool IsRunning { get; }
}