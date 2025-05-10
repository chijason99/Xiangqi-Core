# Engine Service

The `EngineService` provides functionality to interact with local Xiangqi engines using the Universal Chess Interface (UCI) protocol. It allows users to analyze positions, suggest moves, and configure engine options programmatically.

## IXiangqiEngineService

The `IXiangqiEngineService` interface defines the contract for interacting with Xiangqi engines.

### Methods

#### `Task LoadEngineAsync(string enginePath)`
Loads the UCI engine from the specified path.

- **Parameters:**
  - `enginePath` (string): The path to the UCI engine executable.
- **Exceptions:**
  - Throws `ArgumentException` if the path is invalid or not an executable.

#### `void StopEngine()`
Stops the currently running engine process.

#### `IAsyncEnumerable<AnalysisResult> AnalyzePositionAsync(string fen, MoveNotationType notationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default)`
Analyzes the given position indefinitely, providing real-time updates.

- **Parameters:**
  - `fen` (string): The position to analyze in FEN format.
  - `notationType` (MoveNotationType): The desired move notation type for the analysis results.
  - `cancellationToken` (CancellationToken): A token to cancel the analysis.
- **Returns:**
  - An asynchronous stream of `AnalysisResult` objects.

#### `Task<string> SuggestMoveAsync(string fen, EngineAnalysisOptions options, MoveNotationType notationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default)`
Suggests the best move for the given position.

- **Parameters:**
  - `fen` (string): The position to analyze in FEN format.
  - `options` (EngineAnalysisOptions): Options for the analysis, such as depth and move time.
  - `notationType` (MoveNotationType): The desired move notation type for the result.
  - `cancellationToken` (CancellationToken): A token to cancel the operation.
- **Returns:**
  - The best move in the specified notation.

#### `Task StopAnalysisAsync()`
Stops the ongoing analysis.

#### `Task SetEngineConfigAsync(string optionName, string value)`
Sets a configuration option for the engine.

- **Parameters:**
  - `optionName` (string): The name of the option to set.
  - `value` (string): The value to set for the option.
- **Exceptions:**
  - Throws `InvalidOperationException` if the engine fails to set the option.

#### `Task<bool> IsReady()`
Checks if the engine is ready to accept commands.

- **Returns:**
  - `true` if the engine is ready; otherwise, `false`.

---

## DefaultUciEngineService

The `DefaultUciEngineService` is the default implementation of the `IXiangqiEngineService` interface. It uses a UCI-compatible engine to perform operations such as position analysis and move suggestion.

### Constructor

#### `DefaultUciEngineService()`
Initializes a new instance of the `DefaultUciEngineService` with default dependencies:
- `DefaultProcessManager` for managing the engine process.
- `DefaultMoveTranslationService` for translating move notations.

#### `DefaultUciEngineService(IProcessManager processManager, IMoveTranslationService moveTranslationService)`
Initializes a new instance of the `DefaultUciEngineService` with custom dependencies:
- `processManager`: An implementation of `IProcessManager` for managing the engine process.
- `moveTranslationService`: An implementation of `IMoveTranslationService` for translating move notations.

---

## Examples (Using the interaction with FairyStockfish as an example)

### Analyzing a Position
```C#

var engineService = new DefaultUciEngineService(); 

await engineService.LoadEngineAsync(@"C:\path\to\engine.exe");
await engineService.SetEngineConfigAsync("UCI_Variant", "xiangqi");

await foreach (var result in engineService.AnalyzePositionAsync("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0")) 
{ 
	Console.WriteLine($"Depth: {result.Depth}, Score: {result.Score}, PV: {string.Join(" ", result.PrincipalVariation)}"); 
}

```

### Suggesting a Move
```C#

var engineService = new DefaultUciEngineService(); 

await engineService.LoadEngineAsync(@"C:\path\to\engine.exe");
await engineService.SetEngineConfigAsync("UCI_Variant", "xiangqi");

var options = new EngineAnalysisOptions { Depth = 10 }; 
string bestMove = await engineService.SuggestMoveAsync("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0", options);

Console.WriteLine($"Best Move: {bestMove}");
```