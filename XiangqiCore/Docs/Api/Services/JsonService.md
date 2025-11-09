### JSON Generation Service Overview

The JSON Generation service in Xiangqi-Core provides tools for exporting Xiangqi games as JSON strings or byte arrays. 
This service is designed to help developers generate machine-readable JSON representations of games, which are ideal for data storage, API responses, and integration with modern web applications.

The JSON service includes:
- **`IJsonGenerationService`**: For generating JSON strings or byte arrays of game data.

The service supports multiple move notation types, including Traditional Chinese, Simplified Chinese, English, and UCCI, making it flexible for different use cases.

---

### `IJsonGenerationService`
The `IJsonGenerationService` interface provides a set of APIs for exporting the game as a JSON string or byte array.

## Public Methods

### `GenerateGameJson(XiangqiGame game, MoveNotationType? notationType = null)`
Generates a JSON string representation of the game.

**Parameters**:
- `game` (XiangqiGame): The game instance to export as JSON.
- `notationType` (MoveNotationType?, optional): The type of move notation to use in the JSON output. Defaults to `TraditionalChinese` if not specified.

**Return Value**:
- Returns a `string` containing the JSON representation of the game.

**Example**:
```c#
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;

XiangqiBuilder builder = new();

XiangqiGame game = builder
    .WithDefaultConfiguration()
    .WithGameName("測試棋局")
    .Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);
game.MakeMove("馬二進三", MoveNotationType.TraditionalChinese);

IJsonGenerationService jsonService = new DefaultJsonGenerationService();
string json = jsonService.GenerateGameJson(game);

Console.WriteLine(json);

// Output (formatted):
// {
//   "gameName": "測試棋局",
//   "gameInfo": {
//     "redPlayer": { "name": "Unknown", "team": "Unknown" },
//     "blackPlayer": { "name": "Unknown", "team": "Unknown" },
//     "competition": { "name": "Unknown", "location": "Unknown", "date": null, "round": "Unknown" },
//     "result": "Unknown"
//   },
//   "boardState": {
//     "initialFen": "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1",
//     "currentFen": "...",
//     "sideToMove": "Black",
//     "roundNumber": 2,
//     "numberOfMovesWithoutCapture": 0
//   },
//   "moveHistory": [
//     {
//       "roundNumber": 1,
//       "side": "Red",
//       "notation": "炮二平五",
//       "notationType": "TraditionalChinese",
//       "pieceMoved": "Cannon",
//       "from": { "row": 9, "column": 1 },
//       "to": { "row": 9, "column": 4 },
//       "isCapture": false,
//       "isCheck": false,
//       "isCheckmate": false,
//       "fenAfterMove": "..."
//     },
//     // ... more moves
//   ]
// }
```

---

### `GenerateGameJsonAsync(XiangqiGame game, MoveNotationType? notationType = null)`
Asynchronously generates a JSON string representation of the game.

**Parameters**:
- `game` (XiangqiGame): The game instance to export as JSON.
- `notationType` (MoveNotationType?, optional): The type of move notation to use in the JSON output. Defaults to `TraditionalChinese` if not specified.

**Return Value**:
- Returns a `Task<string>` containing the JSON representation of the game.

**Example**:
```c#
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;

XiangqiBuilder builder = new();

XiangqiGame game = builder
    .WithDefaultConfiguration()
    .Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

IJsonGenerationService jsonService = new DefaultJsonGenerationService();
string json = await jsonService.GenerateGameJsonAsync(game);

Console.WriteLine(json);
```

---

### `GenerateGameJsonBytes(XiangqiGame game, MoveNotationType? notationType = null)`
Generates a UTF-8 encoded byte array representation of the game in JSON format.

**Parameters**:
- `game` (XiangqiGame): The game instance to export as JSON.
- `notationType` (MoveNotationType?, optional): The type of move notation to use in the JSON output. Defaults to `TraditionalChinese` if not specified.

**Return Value**:
- Returns a `byte[]` containing the UTF-8 encoded JSON representation of the game.

**Example**:
```c#
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;

XiangqiBuilder builder = new();

XiangqiGame game = builder
    .WithDefaultConfiguration()
    .Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

IJsonGenerationService jsonService = new DefaultJsonGenerationService();
byte[] jsonBytes = jsonService.GenerateGameJsonBytes(game);

// Save to file
File.WriteAllBytes("game.json", jsonBytes);
```

---

### `GenerateGameJsonBytesAsync(XiangqiGame game, MoveNotationType? notationType = null)`
Asynchronously generates a UTF-8 encoded byte array representation of the game in JSON format.

**Parameters**:
- `game` (XiangqiGame): The game instance to export as JSON.
- `notationType` (MoveNotationType?, optional): The type of move notation to use in the JSON output. Defaults to `TraditionalChinese` if not specified.

**Return Value**:
- Returns a `Task<byte[]>` containing the UTF-8 encoded JSON representation of the game.

**Example**:
```c#
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;

XiangqiBuilder builder = new();

XiangqiGame game = builder
    .WithDefaultConfiguration()
    .Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

IJsonGenerationService jsonService = new DefaultJsonGenerationService();
byte[] jsonBytes = await jsonService.GenerateGameJsonBytesAsync(game);

// Save to file asynchronously
await File.WriteAllBytesAsync("game.json", jsonBytes);
```

---

## Using Different Move Notations

The JSON service supports exporting moves in different notation types:

```c#
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.JsonGeneration;

XiangqiBuilder builder = new();

XiangqiGame game = builder
    .WithDefaultConfiguration()
    .Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

IJsonGenerationService jsonService = new DefaultJsonGenerationService();

// Export with UCCI notation
string ucciJson = jsonService.GenerateGameJson(game, MoveNotationType.UCCI);

// Export with English notation
string englishJson = jsonService.GenerateGameJson(game, MoveNotationType.English);

// Export with Simplified Chinese notation
string simplifiedJson = jsonService.GenerateGameJson(game, MoveNotationType.SimplifiedChinese);
```

---

## JSON Structure

The generated JSON has the following structure:

- **gameName**: The name of the game
- **gameInfo**: Contains player information, competition details, and game result
  - **redPlayer**: Red player's name and team
  - **blackPlayer**: Black player's name and team
  - **competition**: Competition name, location, date, and round
  - **result**: Game result (e.g., "1-0", "0-1", "1/2-1/2", "Unknown")
- **boardState**: Current state of the board
  - **initialFen**: Starting FEN position
  - **currentFen**: Current FEN position
  - **sideToMove**: Current side to move
  - **roundNumber**: Current round number
  - **numberOfMovesWithoutCapture**: Draw rule counter
- **moveHistory**: Array of all moves made in the game
  - **roundNumber**: The round number
  - **side**: Side that made the move ("Red" or "Black")
  - **notation**: Move in the specified notation type
  - **notationType**: The notation type used
  - **pieceMoved**: Type of piece that moved
  - **from**: Starting coordinate (row, column)
  - **to**: Destination coordinate (row, column)
  - **isCapture**: Whether the move captured a piece
  - **isCheck**: Whether the move gives check
  - **isCheckmate**: Whether the move is checkmate
  - **fenAfterMove**: FEN position after the move

---

## Dependency Injection

The `IJsonGenerationService` can be registered in a dependency injection container:

```c#
using Microsoft.Extensions.DependencyInjection;
using XiangqiCore.Services.JsonGeneration;

var services = new ServiceCollection();
services.AddSingleton<IJsonGenerationService, DefaultJsonGenerationService>();

var serviceProvider = services.BuildServiceProvider();
var jsonService = serviceProvider.GetRequiredService<IJsonGenerationService>();
```
