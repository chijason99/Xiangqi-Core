### PGN Services Overview

The PGN (Portable Game Notation) services in Xiangqi-Core provide tools for exporting Xiangqi games as PGN strings or files. 
These services are designed to help developers generate, save, and manage PGN representations of games, which are widely used for recording and sharing game data.

The PGN services include:
- **`IPgnGenerationService`**: For generating PGN strings or exporting move history in various notation types.
- **`IPgnSavingService`**: For saving PGN data to files, either synchronously or asynchronously.

These services support multiple move notation types, including Traditional Chinese, Simplified Chinese, English, and UCCI, making them flexible for different use cases.

---

### `IPgnGenerationService`
The `IPgnGenerationService` interface provides a set of APIs for exporting the game as a PGN string or file.

## Public Methods

### `ExportMoveHistory(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)`
Returns the move history of the game as a string.

**Parameters**:
- `game` (XiangqiGame): The game instance containing the move history to export.
- `moveNotationType` (MoveNotationType): The type of move notation to use in the result. Defaults to `MoveNotationType.TraditionalChinese`.

**Return Value**:
- Returns a `string` containing the move history in the specified notation type.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);
game.MakeMove("馬8進7", MoveNotationType.Chinese);
game.MakeMove("馬二進三", MoveNotationType.Chinese);
game.MakeMove("車9平8", MoveNotationType.Chinese);

IPgnGenerationService pgnService = new DefaultPgnGenerationService();
string moveHistory = pgnService.ExportMoveHistory(game);

Console.WriteLine(moveHistory);

// Output:
// 1. 炮二平五 馬8進7 
// 2. 馬二進三 車9平8
```

---

### `GeneratePgnString(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)`
Returns a PGN string.

**Parameters**:
- `game` (XiangqiGame): The game instance to export as a PGN string.
- `moveNotationType` (MoveNotationType): The type of move notation to use in the PGN string. Defaults to `MoveNotationType.TraditionalChinese`.

**Return Value**:
- Returns a `string` containing the PGN representation of the game.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithRedPlayer(player => player.Name = "吕钦")
	.WithBlackPlayer(player => player.Name = "王嘉良")
	.WithCompetition(competition => {
		competition.Event = "全国象棋个人锦标赛";
		competition.Date = "1987-11-30";
	})
	.WithMoveRecord(@"
	1. 炮二平五  馬８進７    2. 馬二進三  車９平８
  3. 馬八進七  卒３進１    4. 炮八平九  馬２進３
  5. 車九平八  車１平２    6. 車一進一  象７進５
  7. 車一平四  炮２進４    8. 車八進一  士６進５
  9. 車八平六  炮２進１   10. 車四進五  卒７進１
 11. 車四平三  馬７退６   12. 車三平二  車８進１
 13. 兵五進一  炮８平７   14. 車二平三  車８進４
 15. 馬三進五  車８平６   16. 車六進五  炮２退１
 17. 炮五平二  車６平８   18. 馬五退四  車２進５
 19. 仕六進五  車２平５   20. 車六平八  車５平６
 21. 車八退三  車６進３   22. 炮二平四  車６平７
 23. 兵七進一  卒７進１   24. 兵七進一  車７退２
 25. 馬七進六  車７平２   26. 馬六退八  卒５進１
 27. 炮四進六  車８退４   28. 車三進一  車８平６
 29. 車三退一  車６進４   30. 兵七進一  馬３退２
 31. 馬八退六  卒５進１   32. 馬六進七  象５進３
 33. 炮九進四  馬６進５   34. 車三平一  卒５平４
 35. 馬七退八  車６進１   36. 馬八進九  車６平２
 37. 相三進五  象３進１   38. 兵七平六  馬２進３
 39. 炮九平八  車２退１   40. 車一進三  士５退６
 41. 兵六進一")
	.WithGameResult(GameResult.RedWin)
	.Build();

IPgnGenerationService pgnService = new DefaultPgnGenerationService();

string pgnString = pgnService.GeneratePgnString(game);

Console.WriteLine(pgnString);

// Output:
// [Game "Chinese Chess"]
// [Event "全国象棋个人锦标赛"]
// [Site "Unknown"]
// [Date "1987.11.30"]
// [Red "吕钦"]
// [RedTeam "Unknown"]
// [Black "王嘉良"]
// [BlackTeam "Unknown"]
// [Result "1-0"]
// [FEN "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1"]
// 1. 炮二平五  馬８進７
// 2. 馬二進三  車９平８
// 3. 馬八進七  卒３進１
// 4. 炮八平九  馬２進３
// 5. 車九平八  車１平２
// 6. 車一進一  象７進５
// 7. 車一平四  炮２進４
// 8. 車八進一  士６進５
// 9. 車八平六  炮２進１
// 10. 車四進五  卒７進１
// 11. 車四平三  馬７退６
// 12. 車三平二  車８進１
// 13. 兵五進一  炮８平７
// 14. 車二平三  車８進４
// 15. 馬三進五  車８平６
// 16. 車六進五  炮２退１
// 17. 炮五平二  車６平８
// 18. 馬五退四  車２進５
// 19. 仕六進五  車２平５
// 20. 車六平八  車５平６
// 21. 車八退三  車６進３
// 22. 炮二平四  車６平７
// 23. 兵七進一  卒７進１
// 24. 兵七進一  車７退２
// 25. 馬七進六  車７平２
// 26. 馬六退八  卒５進１
// 27. 炮四進六  車８退４
// 28. 車三進一  車８平６
// 29. 車三退一  車６進４
// 30. 兵七進一  馬３退２
// 31. 馬八退六  卒５進１
// 32. 馬六進七  象５進３
// 33. 炮九進四  馬６進５
// 34. 車三平一  卒５平４
// 35. 馬七退八  車６進１
// 36. 馬八進九  車６平２
// 37. 相三進五  象３進１
// 38. 兵七平六  馬２進３
// 39. 炮九平八  車２退１
// 40. 車一進三  士５退６
// 41. 兵六進一
```

---

### `GeneratePgn(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)`
Returns the PGN string as a byte array.

**Parameters**:
- `game` (XiangqiGame): The game instance to export as a PGN byte array.
- `moveNotationType` (MoveNotationType): The type of move notation to use in the PGN string. Defaults to `MoveNotationType.TraditionalChinese`.

**Return Value**:
- Returns a `byte[]` containing the PGN representation of the game encoded in GB2312 (or UTF-8 if GB2312 is unavailable).

---

### `IPgnSavingService`

The `IPgnSavingService` interface provides a set of APIs for exporting the game as a PGN file.

---

## Public Methods

### `Save(string filePath, XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)`
Saves the game as a PGN file to the specified file path.

**Parameters**:
- `filePath` (string): The file path where the PGN file will be saved. If no file name is provided, the file will default to the `GameName` property of the `XiangqiGame` class.
- `game` (XiangqiGame): The game instance to export as a PGN file.
- `moveNotationType` (MoveNotationType): The type of move notation to use in the PGN file. Defaults to `MoveNotationType.TraditionalChinese`.

**Return Value**:
- This method does not return a value.

---

### `SaveAsync(string filePath, XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default)`
Asynchronously saves the game as a PGN file to the specified file path.

**Parameters**:
- `filePath` (string): The file path where the PGN file will be saved. If no file name is provided, the file will default to the `GameName` property of the `XiangqiGame` class.
- `game` (XiangqiGame): The game instance to export as a PGN file.
- `moveNotationType` (MoveNotationType): The type of move notation to use in the PGN file. Defaults to `MoveNotationType.TraditionalChinese`.
- `cancellationToken` (CancellationToken): A token to monitor for cancellation requests.

**Return Value**:
- This method does not return a value.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithRedPlayer(player => player.Name = "吕钦")
	.WithBlackPlayer(player => player.Name = "王嘉良")
	.WithCompetition(competition => {
		competition.Event = "全国象棋个人锦标赛";
		competition.Date = "1987-11-30";
	})
	.Buil();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);
 game.MakeMove("馬8進7", MoveNotationType.Chinese);
 game.MakeMove("馬二進三", MoveNotationType.Chinese);
 game.MakeMove("車9平8", MoveNotationType.Chinese);

 IPgnSavingService pgnService = new DefaultPgnSavingService();

 await pgnService.SaveAsync("USERS/DOWNLOAD/test.pgn", game, cancellationToken: default);
```

---

### Summary

The `IPgnGenerationService` and `IPgnSavingService` interfaces provide powerful tools for exporting Xiangqi games as PGN strings or files. These services support multiple move notation types and can be used synchronously or asynchronously, making them flexible for various use cases.