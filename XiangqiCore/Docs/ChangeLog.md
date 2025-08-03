## Change Log

### Version 3.0.0
#### Bug Fixes
- Fixed an issue where the `DefaultUciEngineService` did not correctly identify the score when it is negative.
- Fixed an issue some classes that makes use of Cache/Singleton are not thread-safe, which could lead to unexpected behavior in multithreaded environments.

#### Features
- Xiangqi-Core now supports a tree-like structure for move history, allowing users to explore different branches of the game. This is particularly useful for analyzing alternative moves and strategies.
- Introduced different APIs for move navigation in the `XiangqiGame` class, like NavigateToMove, NavigateToStart, NavigateToEnd, NavigateToNextMove, and NavigateToPreviousMove, which allow users to traverse the move history tree.
- Optimized the `RandmoizePiecePosition` method for quicker performance
- Added the `WithPiecePlacementConstraint` method to the `XiangqiBuilder` class, allowing users to specify constraints for piece placement when randomizing positions. This feature enables more controlled and strategic game setups.
- Rename the `UndoMove` method to `DeleteSubsequentMoves` to better reflect its functionality, which now deletes all moves after the current move in the history tree.

### Version 2.2.0
#### Bug Fixes
- Fixed an issue where the `UndoMove` method failed to restore the board state to its previous position after a move was undone.

#### Documentation
- Reorganized documentation by moving detailed API references and usage guides from `readme.md` to separate markdown files in the `docs` folder.

#### Features
- Introduced `IXiangqiEngineService` and `DefaultUCIEngineService` to provide a flexible API for interacting with local Xiangqi engines. These services allow users to:
  - Analyze positions using the `AnalyzePositionAsync` method.
  - Get the best move from the engine with the `SuggestMoveAsync` method.
  - Configure engine options programmatically.

---

### Version 2.1.0
#### Bug Fixes
- Fixed an issue where the `AddXiangqiCore` extension method threw an error.

#### Performance Improvements
- Improved memory performance when validating moves.

---

### Version 2.0.1
#### Bug Fixes
- Fixed an issue where the `DefaultImageGenerationService` did not use the `ImageConfig` provided by the user when generating images.

---

### Version 2.0.0
#### Features
- Upgraded to .NET 9.0.
- Added dependency injection support for services via the `AddXiangqiCore` extension method.
- Refactored the `XiangqiGame` class to separate external logic, such as image generation and PGN generation.
- Introduced the following interfaces and their default implementations:
  - `IImageGenerationService` and `DefaultImageGenerationService` for generating game images.
  - `IImageSavingService` and `DefaultImageSavingService` for exporting game images as files.
  - `IGifGenerationService` and `DefaultGifGenerationService` for generating GIFs of the game.
  - `IGifSavingService` and `DefaultGifSavingService` for exporting GIFs as files.
  - `IPgnGenerationService` and `DefaultPgnGenerationService` for exporting the game as a PGN string.
  - `IPgnSavingService` and `DefaultPgnSavingService` for exporting the game as a PGN file.
  - `IMoveParsingService` and `DefaultMoveParsingService` for parsing moves from notation strings or game records.
  - `IMoveTranslationService` and `DefaultMoveTranslationService` for translating move notations between different languages.
  - `IMoveCommand` for executing and undoing moves in the game.
- Added the `UndoMove` method to undo moves in the game.

---

### Version 1.6.0
#### Features
- Separated `MoveNotationType` into `SimplifiedChinese` and `TraditionalChinese`.
- Enhanced `MoveHistoryObject` to support translation into UCCI, English, Simplified Chinese, and Traditional Chinese notations.

#### Bug Fixes
- Fixed an issue where UCCI notation was incorrectly output in uppercase instead of lowercase.

---

### Version 1.5.0
#### Features
- Added the `ImageConfig` class to configure options for image and GIF generation.
- Introduced Western (black and white + colored) and Chinese (colored) pieces, as well as a colored board, for image generation.
- Added unit tests for file generation (PGN, GIF, JPG).

#### Bug Fixes
- Fixed a potential concurrency issue when accessing the `ImageCache`.

---

### Version 1.4.1
#### Features
- Renamed `ExportGameAsPgnFile` to `GeneratePgnFile` to follow naming conventions.
- Added asynchronous methods for file generation:
  - `GenerateImageAsync`
  - `GenerateGifAsync`
  - `GeneratePgnFileAsync`

#### Bug Fixes
- Fixed an issue where pawn position validation did not account for pawns that had not yet crossed the river.

---

### Version 1.4.0
#### Features
- Added the `RandomisePiecePosition` method in the `XiangqiBuilder` class to randomize board positions.
- Enhanced the `WithBoardConfig` method in the `XiangqiBuilder` class to overwrite existing FEN/Board configurations when called multiple times.

#### Bug Fixes
- Fixed an issue where the `WithBoardConfig` method in the `XiangqiBuilder` class did not append game info to the FEN string during initialization.
- Added new extension methods `GetPiecesOfType` to reduce the use of reflection when retrieving specific pieces on the board.

---

### Version 1.3.0
#### Features
- Added functionality to create a GIF from the game.
- Added functionality to create an image from a game position.

---

### Version 1.2.1
#### Features
- Enhanced performance when building a Xiangqi game from DPXQ or game records.
- Defaulted the game date to `null` if not provided.

#### Bug Fixes
- Handled exceptions when the `GameDate` format in DPXQ records could not be parsed.

---

### Version 1.2.0
#### Features
- Renamed APIs in the `XiangqiBuilder` class to use the `With` prefix.
- Improved performance when checking for checkmates.
- Added the `WithDpxqGameRecord` method to import games from dpxq.com (Beta).
- Added the `ExportGameAsPgnFile` method to export games as PGN files.
- Enhanced `ExportMoveHistory` to accept the `MoveNotationType` parameter for exporting move history in the specified notation. Currently supports translation of Chinese/English to UCCI notation, with additional translations planned for future releases.
- Updated the `MakeMove` method to accept Simplified Chinese notation.

#### Bug Fixes
- Fixed an issue where the `MakeMove` method did not handle edge cases where two pieces of the same type on the same column were ambiguously referenced in the notation.

---