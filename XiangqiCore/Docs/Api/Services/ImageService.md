### Image Services

The Image Services in Xiangqi-Core provide tools for generating and saving images of the Xiangqi game board. 
These services allow developers to create visual representations of the game state, including piece positions, move indicators, and custom configurations.

## Overview

The Image Services include:
- **`IImageGenerationService`**: For generating images of the game board in various formats and configurations.
- **`IImageSavingService`**: For saving generated images to files, either synchronously or asynchronously.
- **`DefaultImageGenerationService`**: The default implementation of `IImageGenerationService`.
- **`DefaultImageSavingService`**: The default implementation of `IImageSavingService`.

These services support customization through the `ImageConfig` class, which allows you to configure board orientation, piece styles, and other visual elements.

---

## `IImageGenerationService`

The `IImageGenerationService` interface provides a set of APIs for generating images of the game.

### Public Methods

#### `GenerateImage(string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null)`
Generates an image of the game board based on the provided FEN string.

**Parameters**:
- `fen` (string): The FEN string representing the board state.
- `previousLocation` (Coordinate?): The previous location of the piece being moved (optional).
- `currentLocation` (Coordinate?): The current location of the piece being moved (optional).
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).

**Return Value**:
- Returns a `byte[]` containing the generated image in PNG format.

**Example**:
```c#
XiangqiBuilder builder = new(); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);

IImageGenerationService imageService = new DefaultImageGenerationService();
byte[] image = imageService.GenerateImage(game.CurrentFen);
```
---

#### `GenerateImageAsync(string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously generates an image of the game board based on the provided FEN string.

**Parameters**:
- `fen` (string): The FEN string representing the board state.
- `previousLocation` (Coordinate?): The previous location of the piece being moved (optional).
- `currentLocation` (Coordinate?): The current location of the piece being moved (optional).
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- Returns a `Task<byte[]>` containing the generated image in PNG format.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);

IImageGenerationService imageService = new DefaultImageGenerationService();
byte[] image = await imageService.GenerateImageAsync(game.CurrentFen, cancellationToken: default);
```

---

#### `GenerateImage(MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null)`
Generates an image of the game board based on a `MoveHistoryObject`.

**Parameters**:
- `moveHistoryObject` (MoveHistoryObject): The move history object containing the FEN string and move details.
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).

**Return Value**:
- Returns a `byte[]` containing the generated image in PNG format.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);

MoveHistoryObject moveHistory = game.MoveHistory.Last();
IImageGenerationService imageService = new DefaultImageGenerationService();
byte[] image = imageService.GenerateImage(moveHistory);
```

---

#### `GenerateImage(Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null)`
Generates an image of the game board based on a 2D array of `Piece` objects.

**Parameters**:
- `position` (Piece[,]): The 2D array representing the board state.
- `previousLocation` (Coordinate?): The previous location of the piece being moved (optional).
- `currentLocation` (Coordinate?): The current location of the piece being moved (optional).
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).

**Return Value**:
- Returns a `byte[]` containing the generated image in PNG format.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

Piece[,] position = game.BoardPosition;

IImageGenerationService imageService = new DefaultImageGenerationService();
byte[] image = imageService.GenerateImage(position);
```

---

#### `GenerateImageAsync(Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously generates an image of the game board based on a 2D array of `Piece` objects.

**Parameters**:
- `position` (Piece[,]): The 2D array representing the board state.
- `previousLocation` (Coordinate?): The previous location of the piece being moved (optional).
- `currentLocation` (Coordinate?): The current location of the piece being moved (optional).
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- Returns a `Task<byte[]>` containing the generated image in PNG format.

**Example**:
```c#
XiangqiBuilder builder = new(); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

Piece[,] position = game.BoardPosition;

IImageGenerationService imageService = new DefaultImageGenerationService();
byte[] image = await imageService.GenerateImageAsync(position, cancellationToken: default);
```

---

## Default Implementation: `DefaultImageGenerationService`

The `DefaultImageGenerationService` is the default implementation of the `IImageGenerationService` interface. It provides methods for generating images of the Xiangqi game board using various input formats.

### Constructors

#### `DefaultImageGenerationService()`
Initializes a new instance of the `DefaultImageGenerationService` class with default dependencies.

- **Dependency**: The parameterless constructor uses the `DefaultImageResourcePathManager` and `ImageCache` as its default dependencies.

**Example**:
```c#
IImageGenerationService imageService = new DefaultImageGenerationService();
```

#### `DefaultImageGenerationService(IImageResourcePathManager imageResourcePathManager, ImageCache imageCache)`
Initializes a new instance of the `DefaultImageGenerationService` class with custom dependencies.

**Parameters**:
- `imageResourcePathManager` (IImageResourcePathManager): Manages the paths to image resources.
- `imageCache` (ImageCache): Caches images for efficient reuse.

**Example**:
```c#
IImageResourcePathManager resourcePathManager = new CustomImageResourcePathManager(); 
ImageCache imageCache = new ImageCache(); 

IImageGenerationService imageService = new DefaultImageGenerationService(resourcePathManager, imageCache);
```

---

## `IImageSavingService`

The `IImageSavingService` interface provides a set of APIs for saving generated images to files.

### Public Methods

#### `Save(string filePath, string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null)`
Saves an image of the game board to the specified file path based on the provided FEN string.

**Parameters**:
- `filePath` (string): The file path where the image will be saved.
- `fen` (string): The FEN string representing the board state.
- `previousLocation` (Coordinate?): The previous location of the piece being moved (optional).
- `currentLocation` (Coordinate?): The current location of the piece being moved (optional).
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new(); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);

IImageSavingService imageService = new DefaultImageSavingService();
imageService.Save("USERS/DOWNLOAD/test.jpg", game.CurrentFen);
```

---

#### `SaveAsync(string filePath, string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default)`
Asynchronously saves an image of the game board to the specified file path based on the provided FEN string.

**Parameters**:
- `filePath` (string): The file path where the image will be saved.
- `fen` (string): The FEN string representing the board state.
- `previousLocation` (Coordinate?): The previous location of the piece being moved (optional).
- `currentLocation` (Coordinate?): The current location of the piece being moved (optional).
- `imageConfig` (ImageConfig?): Configuration for customizing the image (optional).
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- This method does not return a value.

**Example**:
```c#
XiangqiBuilder builder = new(); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);

IImageSavingService imageService = new DefaultImageSavingService();
await imageService.SaveAsync("USERS/DOWNLOAD/test.jpg", game.CurrentFen, cancellationToken: default);
```

---

## Default Implementation: `DefaultImageSavingService`

The `DefaultImageSavingService` is the default implementation of the `IImageSavingService` interface. It provides methods for saving generated images to files.

### Constructors

#### `DefaultImageSavingService()`
Initializes a new instance of the `DefaultImageSavingService` class with a default `DefaultImageGenerationService` instance.

- **Dependency**: The parameterless constructor uses the `DefaultImageGenerationService` as its default dependency.

**Example**:
```c#
IImageSavingService imageService = new DefaultImageSavingService();
```

#### `DefaultImageSavingService(IImageGenerationService imageGenerationService)`
Initializes a new instance of the `DefaultImageSavingService` class with a custom `IImageGenerationService` instance.

**Parameters**:
- `imageGenerationService` (IImageGenerationService): The image generation service to use for creating images.

---

## `ImageConfig`

The `ImageConfig` class is used to configure the appearance of the generated images. Below are the properties you can configure (all default to `false`):

- **`FlipVertical`**: Flip the board vertically across the 5th column.
- **`FlipHorizontal`**: Flip the board horizontally across the river.
- **`UseBlackAndWhitePieces`**: Use black and white pieces instead of colored pieces.
- **`UseMoveIndicator`**: Show the move indicator on the image to display where the piece moves from/to.
- **`UseWesternPieces`**: Use pieces with a logo instead of traditional pieces with Chinese characters.
- **`UseBlackAndWhiteBoard`**: Use a black and white board instead of a colored board.

**Example**:
```c#
XiangqiBuilder builder = new ();
XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);
game.MakeMove("馬8進7", MoveNotationType.Chinese);
game.MakeMove("馬二進三", MoveNotationType.Chinese);

ImageConfig config = new()
{
	UseBlackAndWhitePieces = true,
	UseMoveIndicator = true,
	UseWesternPieces = true,
};

IImageGenerationService imageService = new DefaultImageGenerationService();

await imageService.GenerateImage(game.CurrentFen, cancellationToken: default);
```

---

### Summary

The Image Services in Xiangqi-Core provide powerful tools for generating and saving images of the game board. These services support various input formats (FEN strings, move history, or board positions) and allow for extensive customization through the `ImageConfig` class. Whether you need to generate images for visualization or save them for sharing, these services offer a flexible and efficient solution.