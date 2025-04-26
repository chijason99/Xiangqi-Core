## Change Log

### Version 2.1.0
- Fix the issue where the `AddXiangqiCore` extension method throws an error
- Improve the memory performance when validating the moves

---

### Version 2.0.1
Bug Fixes:
- Fix the issue where the `DefaultImageGenerationService` does not make use of the `ImageConfig` from the user when generating the image

---

### Version 2.0.0
- Features:
	- Upgraded to .NET 9.0
	- Dependency Injection support for the services by adding the `AddXiangqiCore` extension method
	- Separated the external logics, like the Image Generation and PGN generation, from the `XiangqiGame` class.
	- Added the `IImageGenerationService` interface to generate images of the game, with the `DefaultImageGenerationService` implementation
	- Added the `IImageSavingService` interface to export the game as an image file, with the `DefaultImageSavingService` implementation
	- Added the `IGifGenerationService` interface to generate GIFs of the game, with the `DefaultGifGenerationService` implementation
	- Added the `IGifSavingService` interface to export the game as a GIF file, with the `DefaultGifSavingService` implementation
	- Added the `IPgnGenerationService` interface to export the game as a PGN string, with the `DefaultPgnGenerationService` implementation
	- Added the `IPgnSavingService` interface to export the game as a PGN file, with the `DefaultPgnSavingService` implementation
	- Added the `IMoveParsingService` interface to parse the move from the move notation string, and game record string, with the `DefaultMoveParsingService` implementation
	- Added the `IMoveTranslationService` interface to translate the move notation between different languages, with the `DefaultMoveTranslationService` implementation
	- Added the `IMoveCommand` interface to execute/undo the move in the game
	- Add the UndoMove method to undo moves in the game

---

### Version 1.6.0
- Features:
	- Separate the MoveNotationType from Chinese to SimplifiedChinese and TradtionalChinese
	- Now the MoveHistoryObject supports the translation into UCCI, English, Simplified Chinese and Traditional Chinese notations
- Bug Fixes:
	- Fix the issue where the UCCI notation is in uppercase but not in lowercase

---

### Version 1.5.0
- Features:
	- Add the ImageConfig class to set different options for the image/GIF generation
	- Add Western pieces (black and white + coloured), Chinese pieces (coloured), and board (coloured) for the image generation
	- Add some unit tests for the file generation (PGN, GIF, JPG)
- Bug Fixes:
	- Fix the potential concurrency issue when accessing the ImageCache

---

### Version 1.4.1
- Features:
	- Rename the ``ExportGameAsPgnFile`` to ``GeneratePgnFile`` to follow naming convention
	- Add ``GenerateImageAsync``, ``GenerateGifAsync``, and ``GeneratePgnFileAsync`` for asynchronous generation of image, GIF, and PGN file
- Bug Fixes:
	- Fix the position validation for pawns not accounting for the pawns that have not yet crossed the river

---

### Version 1.4.0
- Features:
	- Functionality to randomise the board position using the `RandomisePiecePosition` method in the `XiangqiBuilder` class.
	- Add the overriding behaviour of the `WithBoardConfig` method in the `XiangqiBuilder` class. Now it will overwrite the existing FEN/Board Config if it is called multiple times.
- Bug Fixes:
	- Fix the issue where the `WithBoardConfig` method in the `XiangqiBuilder` class does not append the game info to the FEN string when initializing the game.
	- New extension methods `GetPiecesOfType` to reduce the use of reflection in the process of getting the pieces of a specific type on the board.

---

### Version 1.3.0
- Features:
	- Functionality to create a GIF from the game.
	- Functionality to create an image from a game position.

---

### Version 1.2.1
- Features:
	- Performance enhancement when building a Xiangqi game from DPXQ or game record
	- Default the game date to null if not provided
- Bug Fixes:
	- Handle the exception when the GameDate format in DPXQ record cannot be parsed

---

### Version 1.2.0
- Features:
	- Rename the APIs in `XiangqiBuilder` class to prefix with `With`.
	- Improve the performance of the process for checking for checkmates.
	- Add the `WithDpxqGameRecord` method to import a game from dpxq.com. (Beta)
	- Add the `ExportGameAsPgnFile` method to export the game as a PGN file.
	- `ExportMoveHistory` can now accept the MoveNotationType parameter to export the move history in the specified notation. Currently it only supports the transaltion of Chinese/English to UCCI notation. The other translation would be added in the future.
	- The `MakeMove` method now can also accepts Simplified Chinese notation.
- Bug Fixes:
	- Fix the issue where the MakeMove method does not handle the edge case that, in some notations, there are two pieces of the same type on the same column and the notation
does not mark which piece is moving, as only one of them would be able to perform the move validly.

---
