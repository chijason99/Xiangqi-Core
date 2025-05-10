## IProcessManager

The `IProcessManager` interface defines the contract for managing the lifecycle and communication of the UCI engine process.

### Properties

#### `bool IsRunning`
Indicates whether the engine process is currently running.

### Methods

#### `Task StartAsync(string enginePath)`
Starts the engine process.

#### `void Stop()`
Stops the engine process.

#### `Task SendCommandAsync(string command)`
Sends a command to the engine process.

#### `Task<string> ReadResponseAsync(Func<string, bool> stopCondition, TimeSpan? timeout = null, bool stopOnEmtpyResponse = true)`
Reads a single response from the engine process based on a stop condition.

#### `IAsyncEnumerable<string> ReadResponsesAsync(Func<string, bool> stopCondition, bool stopOnEmtpyResponse = true, CancellationToken cancellationToken = default)`
Reads multiple responses from the engine process based on a stop condition.

---

## DefaultProcessManager

The `DefaultProcessManager` is the default implementation of the `IProcessManager` interface. It manages the lifecycle and communication of the UCI engine process.

### Properties

#### `bool IsRunning`
Indicates whether the engine process is currently running.

### Methods

#### `Task StartAsync(string enginePath)`
Starts the engine process.

- **Parameters:**
  - `enginePath` (string): The path to the UCI engine executable.
- **Exceptions:**
  - Throws `ArgumentException` if the path is invalid or not an executable.

#### `void Stop()`
Stops the engine process.

#### `Task SendCommandAsync(string command)`
Sends a command to the engine process.

- **Parameters:**
  - `command` (string): The command to send.

#### `Task<string> ReadResponseAsync(Func<string, bool> stopCondition, TimeSpan? timeout = null, bool stopOnEmtpyResponse = true)`
Reads a single response from the engine process based on a stop condition.

- **Parameters:**
  - `stopCondition` (Func<string, bool>): A function to determine when to stop reading.
  - `timeout` (TimeSpan?): The maximum time to wait for a response.
  - `stopOnEmtpyResponse` (bool): Whether to stop reading on an empty response.
- **Returns:**
  - The response string.

#### `IAsyncEnumerable<string> ReadResponsesAsync(Func<string, bool> stopCondition, bool stopOnEmtpyResponse = true, CancellationToken cancellationToken = default)`
Reads multiple responses from the engine process based on a stop condition.

- **Parameters:**
  - `stopCondition` (Func<string, bool>): A function to determine when to stop reading.
  - `stopOnEmtpyResponse` (bool): Whether to stop reading on an empty response.
  - `cancellationToken` (CancellationToken): A token to cancel the operation.
- **Returns:**
  - An asynchronous stream of response strings.

---