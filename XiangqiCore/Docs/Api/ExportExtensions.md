# Export Extension Methods

Xiangqi-Core provides convenient extension methods on `XiangqiGame` for common export operations. These methods are designed for simplicity and ease of use in most scenarios.

## Quick Start

Extension methods provide the simplest way to export game data:

```csharp
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Move;

var builder = new XiangqiBuilder();
var game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);

// Export to JSON
string json = game.ToJson();

// Export to PGN
string pgn = game.ToPgn();

// Save to files
await game.SaveAsJsonAsync("game.json");
await game.SaveAsPgnAsync("game.pgn");
```

---

## JSON Export Methods

### `ToJson()`
Exports the game as a JSON string.

```csharp
string json = game.ToJson();

// With specific notation type
string ucciJson = game.ToJson(MoveNotationType.UCCI);
```

**Parameters:**
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** JSON string representation of the game.

---

### `ToJsonAsync()`
Asynchronously exports the game as a JSON string.

```csharp
string json = await game.ToJsonAsync();
string ucciJson = await game.ToJsonAsync(MoveNotationType.UCCI);
```

**Parameters:**
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** `Task<string>` containing the JSON representation.

---

### `ToJsonBytes()`
Exports the game as a UTF-8 encoded byte array.

```csharp
byte[] jsonBytes = game.ToJsonBytes();
```

**Parameters:**
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** UTF-8 encoded byte array.

---

### `ToJsonBytesAsync()`
Asynchronously exports the game as a UTF-8 encoded byte array.

```csharp
byte[] jsonBytes = await game.ToJsonBytesAsync();
```

**Parameters:**
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** `Task<byte[]>` containing the UTF-8 encoded byte array.

---

### `SaveAsJsonAsync()`
Saves the game as a JSON file.

```csharp
await game.SaveAsJsonAsync("my_game.json");

// With specific notation
await game.SaveAsJsonAsync("my_game.json", MoveNotationType.UCCI);
```

**Parameters:**
- `filePath`: Path where the JSON file will be saved.
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** `Task` representing the asynchronous operation.

---

## PGN Export Methods

### `ToPgn()`
Exports the game as a PGN string.

```csharp
string pgn = game.ToPgn();

// With specific notation type
string ucciPgn = game.ToPgn(MoveNotationType.UCCI);
```

**Parameters:**
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** PGN string representation of the game.

---

### `ToPgnBytes()`
Exports the game as a PGN byte array (GB2312 encoded).

```csharp
byte[] pgnBytes = game.ToPgnBytes();
byte[] ucciPgnBytes = game.ToPgnBytes(MoveNotationType.UCCI);
```

**Parameters:**
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** GB2312 encoded byte array.

---

### `SaveAsPgnAsync()`
Saves the game as a PGN file.

```csharp
await game.SaveAsPgnAsync("my_game.pgn");

// With specific notation
await game.SaveAsPgnAsync("my_game.pgn", MoveNotationType.English);
```

**Parameters:**
- `filePath`: Path where the PGN file will be saved.
- `notationType` (optional): Move notation type to use. Defaults to `TraditionalChinese`.

**Returns:** `Task` representing the asynchronous operation.

---

## Complete Example

```csharp
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Move;

// Create and configure a game
var builder = new XiangqiBuilder();
var game = builder
    .WithDefaultConfiguration()
    .WithRedPlayer(player =>
    {
        player.Name = "王斌";
        player.Team = "廣東";
    })
    .WithBlackPlayer(player =>
    {
        player.Name = "許銀川";
        player.Team = "廣東";
    })
    .WithCompetition(competition =>
    {
        competition.WithName("1999年全國象棋個人錦標賽");
        competition.WithLocation("北京");
        competition.WithGameDate(new DateTime(1999, 10, 15));
    })
    .Build();

// Play some moves
game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);
game.MakeMove("馬８進７", MoveNotationType.TraditionalChinese);
game.MakeMove("馬二進三", MoveNotationType.TraditionalChinese);

// Export in different formats
string json = game.ToJson();
string jsonUcci = game.ToJson(MoveNotationType.UCCI);
string pgn = game.ToPgn();

// Save to files
await game.SaveAsJsonAsync("game.json");
await game.SaveAsPgnAsync("game.pgn");

Console.WriteLine("Game exported successfully!");
```

---

## Advanced Usage: Custom Translation Services

Extension methods use default translation services internally. If you need custom translation behavior, use the service classes directly:

```csharp
using XiangqiCore.Services.JsonGeneration;
using XiangqiCore.Services.MoveTransalation;

// Create a custom translation service
var customTranslator = new MyCustomMoveTranslationService();

// Use it with the JSON generation service
var jsonService = new DefaultJsonGenerationService(customTranslator);
string json = jsonService.GenerateGameJson(game, MoveNotationType.UCCI);
```

See [JsonService.md](./JsonService.md) and [PgnService.md](./PgnService.md) for detailed information about the underlying services.

---

## Performance Notes

Extension methods use lazy-initialized singleton services internally:
- Services are created once and reused across all calls
- Thread-safe via `Lazy<T>`
- No performance penalty compared to direct service usage
- Suitable for both single-game and batch processing scenarios

---

## Namespace

```csharp
using XiangqiCore.Extension;
```

All extension methods are in the `XiangqiCore.Extension` namespace. Import this namespace to access the extension methods on `XiangqiGame` instances.
