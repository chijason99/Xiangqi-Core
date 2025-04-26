### GIF Services

The GIF Services in Xiangqi-Core provide tools for generating and saving animated GIFs of the Xiangqi game board. 
These services allow developers to create visual representations of the game's progression, including move animations and custom configurations.

## Overview

The GIF Services include:
- **`IGifGenerationService`**: For generating GIFs of the game board in various formats and configurations.
- **`IGifSavingService`**: For saving generated GIFs to files, either synchronously or asynchronously.
- **`DefaultGifGenerationService`**: The default implementation of `IGifGenerationService`.
- **`DefaultGifSavingService`**: The default implementation of `IGifSavingService`.

These services support customization through the `ImageConfig` class, which allows you to configure board orientation, piece styles, and other visual elements.

---

## `IGifGenerationService`

The `IGifGenerationService` interface provides a set of APIs for generating GIFs of the game.

### Public Methods

#### `GenerateGif(IEnumerable<string> fens, ImageConfig? imageConfig = null)`
Generates a GIF of the game based on a sequence of FEN strings.

**Parameters**:
- `fens` (IEnumerable<string>): A collection of FEN strings representing the game states.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).

**Return Value**:
- Returns a `byte[]` containing the generated GIF.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifGenerationService gifService = new DefaultGifGenerationService();
byte[] gif = gifService.GenerateGif(game.MoveHistory.Select(m => m.FenAfterMove));

```

---

#### `GenerateGifAsync(IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously generates a GIF of the game based on a sequence of FEN strings.

**Parameters**:
- `fens` (IEnumerable<string>): A collection of FEN strings representing the game states.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- Returns a `Task<byte[]>` containing the generated GIF.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifGenerationService gifService = new DefaultGifGenerationService();
byte[] gif = await gifService.GenerateGifAsync(game.MoveHistory.Select(m => m.FenAfterMove), cancellationToken: default);

```

---

#### `GenerateGif(List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null)`
Generates a GIF of the game based on a list of `MoveHistoryObject` instances.

**Parameters**:
- `moveHistory` (List<MoveHistoryObject>): A list of move history objects representing the game's progression.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).

**Return Value**:
- Returns a `byte[]` containing the generated GIF.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifGenerationService gifService = new DefaultGifGenerationService();
byte[] gif = gifService.GenerateGif(game.MoveHistory);

```

---

#### `GenerateGifAsync(List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously generates a GIF of the game based on a list of `MoveHistoryObject` instances.

**Parameters**:
- `moveHistory` (List<MoveHistoryObject>): A list of move history objects representing the game's progression.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- Returns a `Task<byte[]>` containing the generated GIF.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifGenerationService gifService = new DefaultGifGenerationService();
byte[] gif = await gifService.GenerateGifAsync(game.MoveHistory, cancellationToken: default);

```

---

#### `GenerateGif(XiangqiGame game, ImageConfig? imageConfig = null)`
Generates a GIF of the game based on the `XiangqiGame` instance.

**Parameters**:
- `game` (XiangqiGame): The game instance to generate the GIF for.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).

**Return Value**:
- Returns a `byte[]` containing the generated GIF.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifGenerationService gifService = new DefaultGifGenerationService();
byte[] gif = gifService.GenerateGif(game);

```

---

#### `GenerateGifAsync(XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously generates a GIF of the game based on the `XiangqiGame` instance.

**Parameters**:
- `game` (XiangqiGame): The game instance to generate the GIF for.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- Returns a `Task<byte[]>` containing the generated GIF.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifGenerationService gifService = new DefaultGifGenerationService();
byte[] gif = await gifService.GenerateGifAsync(game, cancellationToken: default);

```

---

## Default Implementation: `DefaultGifGenerationService`

The `DefaultGifGenerationService` is the default implementation of the `IGifGenerationService` interface. It provides methods for generating GIFs of the Xiangqi game board using various input formats.

### Constructors

#### `DefaultGifGenerationService()`
Initializes a new instance of the `DefaultGifGenerationService` class with default dependencies.

- **Dependency**: The parameterless constructor uses the `DefaultImageGenerationService` as its default dependency.

**Example**:
```c#
IGifGenerationService gifService = new DefaultGifGenerationService();
```

#### `DefaultGifGenerationService(IImageGenerationService imageGenerationService)`
Initializes a new instance of the `DefaultGifGenerationService` class with a custom `IImageGenerationService` instance.

**Parameters**:
- `imageGenerationService` (IImageGenerationService): The image generation service to use for creating images for the GIF.

**Example**:
```C#
IImageGenerationService customImageService = new CustomImageGenerationService(); 
IGifGenerationService gifService = new DefaultGifGenerationService(customImageService);
```

---

## `IGifSavingService`

The `IGifSavingService` interface provides a set of APIs for saving generated GIFs to files.

### Public Methods

#### `Save(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null)`
Saves a GIF of the game to the specified file path based on a sequence of FEN strings.

**Parameters**:
- `filePath` (string): The file path where the GIF will be saved.
- `fens` (IEnumerable<string>): A collection of FEN strings representing the game states.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifSavingService gifService = new DefaultGifSavingService();
gifService.Save("USERS/DOWNLOAD/test.gif", game.MoveHistory.Select(m => m.FenAfterMove));

```

---

#### `SaveAsync(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously saves a GIF of the game to the specified file path based on a sequence of FEN strings.

**Parameters**:
- `filePath` (string): The file path where the GIF will be saved.
- `fens` (IEnumerable<string>): A collection of FEN strings representing the game states.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifSavingService gifService = new DefaultGifSavingService();
await gifService.SaveAsync("USERS/DOWNLOAD/test.gif", game.MoveHistory.Select(m => m.FenAfterMove), cancellationToken: default);

```

---

#### `Save(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null)`
Saves a GIF of the game to the specified file path based on a list of `MoveHistoryObject` instances.

**Parameters**:
- `filePath` (string): The file path where the GIF will be saved.
- `moveHistory` (List<MoveHistoryObject>): A list of move history objects representing the game's progression.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifSavingService gifService = new DefaultGifSavingService();
gifService.Save("USERS/DOWNLOAD/test.gif", game.MoveHistory);

```

---

#### `SaveAsync(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously saves a GIF of the game to the specified file path based on a list of `MoveHistoryObject` instances.

**Parameters**:
- `filePath` (string): The file path where the GIF will be saved.
- `moveHistory` (List<MoveHistoryObject>): A list of move history objects representing the game's progression.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifSavingService gifService = new DefaultGifSavingService();
await gifService.SaveAsync("USERS/DOWNLOAD/test.gif", game.MoveHistory, cancellationToken: default);

```

---

#### `Save(string filePath, XiangqiGame game, ImageConfig? imageConfig = null)`
Saves a GIF of the game to the specified file path based on the `XiangqiGame` instance.

**Parameters**:
- `filePath` (string): The file path where the GIF will be saved.
- `game` (XiangqiGame): The game instance to generate the GIF for.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifSavingService gifService = new DefaultGifSavingService();
gifService.Save("USERS/DOWNLOAD/test.gif", game);

```

---

#### `SaveAsync(string filePath, XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously saves a GIF of the game to the specified file path based on the `XiangqiGame` instance.

**Parameters**:
- `filePath` (string): The file path where the GIF will be saved.
- `game` (XiangqiGame): The game instance to generate the GIF for.
- `imageConfig` (ImageConfig?): Configuration for customizing the GIF (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
game.MakeMove("炮二平五", MoveNotationType.Chinese); game.MakeMove("馬8進7", MoveNotationType.Chinese);
IGifSavingService gifService = new DefaultGifSavingService();
await gifService.SaveAsync("USERS/DOWNLOAD/test.gif", game, cancellationToken: default);

```

---

## Default Implementation: `DefaultGifSavingService`

The `DefaultGifSavingService` is the default implementation of the `IGifSavingService` interface. It provides methods for saving generated GIFs to files.

### Constructors

#### `DefaultGifSavingService()`
Initializes a new instance of the `DefaultGifSavingService` class with a default `DefaultGifGenerationService` instance.

- **Dependency**: The parameterless constructor uses the `DefaultGifGenerationService` as its default dependency.

**Example**:
```c#
IGifSavingService gifService = new DefaultGifSavingService();
```

#### `DefaultGifSavingService(IGifGenerationService gifGenerationService)`
Initializes a new instance of the `DefaultGifSavingService` class with a custom `IGifGenerationService` instance.

**Parameters**:
- `gifGenerationService` (IGifGenerationService): The GIF generation service to use for creating GIFs.

**Example**:
```c#
IGifGenerationService customGifService = new CustomGifGenerationService(); 
IGifSavingService gifService = new DefaultGifSavingService(customGifService);
```

### Summary

The GIF Services in Xiangqi-Core provide powerful tools for generating and saving animated GIFs of the game board. These services support various input formats (FEN strings, move history, or game instances) and allow for extensive customization through the `ImageConfig` class. Whether you need to generate GIFs for visualization or save them for sharing, these services offer a flexible and efficient solution.