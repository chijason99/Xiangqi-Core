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

game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);
game.MakeMove("�R8�M7", MoveNotationType.Chinese);
game.MakeMove("�R���M��", MoveNotationType.Chinese);
game.MakeMove("܇9ƽ8", MoveNotationType.Chinese);

IPgnGenerationService pgnService = new DefaultPgnGenerationService();
string moveHistory = pgnService.ExportMoveHistory(game);

Console.WriteLine(moveHistory);

// Output:
// 1. �ڶ�ƽ�� �R8�M7 
// 2. �R���M�� ܇9ƽ8
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
	.WithRedPlayer(player => player.Name = "����")
	.WithBlackPlayer(player => player.Name = "������")
	.WithCompetition(competition => {
		competition.Event = "ȫ��������˽�����";
		competition.Date = "1987-11-30";
	})
	.WithMoveRecord(@"
	1. �ڶ�ƽ��  �R���M��    2. �R���M��  ܇��ƽ��
  3. �R���M��  �䣳�M��    4. �ڰ�ƽ��  �R���M��
  5. ܇��ƽ��  ܇��ƽ��    6. ܇һ�Mһ  ���M��
  7. ܇һƽ��  �ڣ��M��    8. ܇���Mһ  ʿ���M��
  9. ܇��ƽ��  �ڣ��M��   10. ܇���M��  �䣷�M��
 11. ܇��ƽ��  �R���ˣ�   12. ܇��ƽ��  ܇���M��
 13. �����Mһ  �ڣ�ƽ��   14. ܇��ƽ��  ܇���M��
 15. �R���M��  ܇��ƽ��   16. ܇���M��  �ڣ��ˣ�
 17. ����ƽ��  ܇��ƽ��   18. �R������  ܇���M��
 19. �����M��  ܇��ƽ��   20. ܇��ƽ��  ܇��ƽ��
 21. ܇������  ܇���M��   22. �ڶ�ƽ��  ܇��ƽ��
 23. �����Mһ  �䣷�M��   24. �����Mһ  ܇���ˣ�
 25. �R���M��  ܇��ƽ��   26. �R���˰�  �䣵�M��
 27. �����M��  ܇���ˣ�   28. ܇���Mһ  ܇��ƽ��
 29. ܇����һ  ܇���M��   30. �����Mһ  �R���ˣ�
 31. �R������  �䣵�M��   32. �R���M��  ���M��
 33. �ھ��M��  �R���M��   34. ܇��ƽһ  �䣵ƽ��
 35. �R���˰�  ܇���M��   36. �R���M��  ܇��ƽ��
 37. �����M��  ���M��   38. ����ƽ��  �R���M��
 39. �ھ�ƽ��  ܇���ˣ�   40. ܇һ�M��  ʿ���ˣ�
 41. �����Mһ")
	.WithGameResult(GameResult.RedWin)
	.Build();

IPgnGenerationService pgnService = new DefaultPgnGenerationService();

string pgnString = pgnService.GeneratePgnString(game);

Console.WriteLine(pgnString);

// Output:
// [Game "Chinese Chess"]
// [Event "ȫ��������˽�����"]
// [Site "Unknown"]
// [Date "1987.11.30"]
// [Red "����"]
// [RedTeam "Unknown"]
// [Black "������"]
// [BlackTeam "Unknown"]
// [Result "1-0"]
// [FEN "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1"]
// 1. �ڶ�ƽ��  �R���M��
// 2. �R���M��  ܇��ƽ��
// 3. �R���M��  �䣳�M��
// 4. �ڰ�ƽ��  �R���M��
// 5. ܇��ƽ��  ܇��ƽ��
// 6. ܇һ�Mһ  ���M��
// 7. ܇һƽ��  �ڣ��M��
// 8. ܇���Mһ  ʿ���M��
// 9. ܇��ƽ��  �ڣ��M��
// 10. ܇���M��  �䣷�M��
// 11. ܇��ƽ��  �R���ˣ�
// 12. ܇��ƽ��  ܇���M��
// 13. �����Mһ  �ڣ�ƽ��
// 14. ܇��ƽ��  ܇���M��
// 15. �R���M��  ܇��ƽ��
// 16. ܇���M��  �ڣ��ˣ�
// 17. ����ƽ��  ܇��ƽ��
// 18. �R������  ܇���M��
// 19. �����M��  ܇��ƽ��
// 20. ܇��ƽ��  ܇��ƽ��
// 21. ܇������  ܇���M��
// 22. �ڶ�ƽ��  ܇��ƽ��
// 23. �����Mһ  �䣷�M��
// 24. �����Mһ  ܇���ˣ�
// 25. �R���M��  ܇��ƽ��
// 26. �R���˰�  �䣵�M��
// 27. �����M��  ܇���ˣ�
// 28. ܇���Mһ  ܇��ƽ��
// 29. ܇����һ  ܇���M��
// 30. �����Mһ  �R���ˣ�
// 31. �R������  �䣵�M��
// 32. �R���M��  ���M��
// 33. �ھ��M��  �R���M��
// 34. ܇��ƽһ  �䣵ƽ��
// 35. �R���˰�  ܇���M��
// 36. �R���M��  ܇��ƽ��
// 37. �����M��  ���M��
// 38. ����ƽ��  �R���M��
// 39. �ھ�ƽ��  ܇���ˣ�
// 40. ܇һ�M��  ʿ���ˣ�
// 41. �����Mһ
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
	.WithRedPlayer(player => player.Name = "����")
	.WithBlackPlayer(player => player.Name = "������")
	.WithCompetition(competition => {
		competition.Event = "ȫ��������˽�����";
		competition.Date = "1987-11-30";
	})
	.Buil();

 game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);
 game.MakeMove("�R8�M7", MoveNotationType.Chinese);
 game.MakeMove("�R���M��", MoveNotationType.Chinese);
 game.MakeMove("܇9ƽ8", MoveNotationType.Chinese);

 IPgnSavingService pgnService = new DefaultPgnSavingService();

 await pgnService.SaveAsync("USERS/DOWNLOAD/test.pgn", game, cancellationToken: default);
```

---

### Summary

The `IPgnGenerationService` and `IPgnSavingService` interfaces provide powerful tools for exporting Xiangqi games as PGN strings or files. These services support multiple move notation types and can be used synchronously or asynchronously, making them flexible for various use cases.