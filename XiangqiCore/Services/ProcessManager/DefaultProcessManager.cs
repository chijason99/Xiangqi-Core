using System.Diagnostics;

namespace XiangqiCore.Services.ProcessManager;

public class DefaultProcessManager : IProcessManager
{
	private Process _process;
	private StreamWriter _standardInput;
	private StreamReader _standardOutput;

	public bool IsRunning => _process is not null && !_process.HasExited;

	public event EventHandler<string> OnErrorReceived;

	public async Task<string> ReadResponseAsync(Func<string, bool> stopCondition, TimeSpan? timeout = null)
	{
		if (!IsRunning || _standardOutput == null)
			throw new InvalidOperationException("The process is not running");

		CancellationTokenSource? cancellationTokenSource = null;

		if (timeout.HasValue)
			cancellationTokenSource = new(timeout.Value);

		try
		{
			while (cancellationTokenSource is null || !cancellationTokenSource.Token.IsCancellationRequested)
			{
				string response = await _standardOutput.ReadLineAsync();
				Console.WriteLine($"Read response: {response}");

				if (stopCondition(response))
					return response;

				if (string.IsNullOrEmpty(response))
					// Break if no response is received (to avoid infinite loop)
					break;
			}
		}
		catch (OperationCanceledException)
		{
			Console.WriteLine($"Timeout while waiting for response");
		}

		return string.Empty;
	}

	public async Task<string> ReadResponseAsync(string expectedResponse, TimeSpan? timeout = null)
		=> await ReadResponseAsync(response => string.Equals(response, expectedResponse, StringComparison.OrdinalIgnoreCase), timeout);

	public async Task SendCommandAsync(string command)
	{
		if (_standardInput == null)
			throw new InvalidOperationException("The process is not running");

		await _standardInput.WriteLineAsync(command);
		await _standardInput.FlushAsync();
	}

	public async Task StartAsync(string enginePath)
	{
		if (Path.GetExtension(enginePath) != ".exe")
			throw new ArgumentException("The enginePath should be pointing to an executable");

		if (!File.Exists(enginePath))
			throw new ArgumentException("Please provide a valid engine path");

		_process= new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = enginePath,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				WorkingDirectory = Path.GetDirectoryName(enginePath)
			},
		};

		_process.ErrorDataReceived += (sender, e) =>
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				OnErrorReceived?.Invoke(this, e.Data);
			}
		};

		_process.Start();
		_standardInput = _process.StandardInput;
		_standardOutput = _process.StandardOutput;

		Console.WriteLine($"Process started: {_process.StartInfo.FileName}");
		Console.WriteLine($"Process ID: {_process.Id}");
		Console.WriteLine($"Is Running: {IsRunning}");

		_process.BeginErrorReadLine();

		await Task.CompletedTask;
	}

	public void Stop()
	{
		if (!IsRunning)
			return;

		Console.WriteLine($"Process ended: {_process.StartInfo.FileName}");
		_process.Kill();

		_process.Dispose();
		_standardInput.Dispose();
		_standardOutput.Dispose();

		_process = null;
		_standardInput = null;
		_standardOutput = null;
	}
}
