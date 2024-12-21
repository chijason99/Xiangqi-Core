# Xiangqi-Core Nuget Package

## Description
Xiangqi-Core is a comprehensive library designed to facilitate the development of applications related to Xiangqi (Chinese Chess). It provides a robust set of functionalities including move generation, move validation, game state management, etc. Built with flexibility and performance in mind, XiangqiCore aims to be the go-to solution for developers looking to integrate Xiangqi mechanics into their software.

## Features

- **Fluent API**: Provides a fluent API for easy configuration and initialization of game instances.
- **Game State Management**: Easily manage game states, including piece positions, turn tracking, and game outcome detection.
- **Parsing of Move Notations**: Supports parsing of move notations in UCCI, Chinese, and English, allowing for versatile game command inputs.
- **Move Validation**: Validate player moves, ensuring moves adhere to the rules of Xiangqi.
- **Utility Functions**: A collection of utility functions for piece and board management, including piece movement simulation and position checking.


## Installation

Xiangqi-Core is available as a NuGet package. You can install it using the NuGet Package Manager or the dotnet CLI.

```bash
dotnet add package Xiangqi-Core
```

## Getting Started

To get started with Xiangqi-Core, first import the package into your project:

Here's a simple example of setting up a game board and making a move:

```c#
using XiangqiCore.Game;

// Create a new game instance with the help of the XiangqiBuilder
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
	
// Make a move
game.MakeMove("炮二平五", MoveNotationType.Chinese);
```

Refer to the tests or documentation below (In progress) for more detailed examples and usage instructions.

## Documentation

### XiangqiBuilder

The `XiangqiBuilder` class is responsible for creating instances of the Xiangqi game with different configurations. It provides a fluent API for easy configuration and initialization of game instances.

#### Public Methods

##### `WithDefaultConfiguration()`
Sets the Xiangqi game configuration to the default starting position.

Default Configuration:
- Starting Fen: rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1
- Side to move: Red
- Red player: Unknown
- Black player: Unknown
- Game result: Unknown
- Competition: 
	- GameDate : null
	- Round : Unknown
	- Name : Unknown

If the above fields are not set when calling the ``Build`` method, these default values will be used by default.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = builder.WithDefaultConfiguration().Build();
```


##### `WithStartingFen(string customFen)`
Sets the starting position according to the provided FEN (Forsyth-Edwards Notation).

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game =  builder
	.WithStartingFen("1r2kabr1/4a4/1c2b1n2/p3p1C1p/2pn2P2/9/P1P1P2cP/N3C1N2/2R6/2BAKABR1 b - - 2 10")
	.Build();
```

##### `WithEmptyBoard()`
Sets the starting position to an empty board by setting the initial FEN to "9/9/9/9/9/9/9/9/9/9 w - - 0 1". 
This is useful for setting up custom board configurations with the ``WithBoardConfig`` method below, where you want to set up the board manually without using a FEN.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game = builder
	.WithEmptyBoard()
	.Build();
```

##### `Build()`
Builds an instance of the Xiangqi game.

##### `WithRedPlayer(Action<Player> action)`
Sets the configuration for the red player. If the Name or Team is not set for the player, the default value will be "Unknown".

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithRedPlayer(player => { 
		player.Name = "許銀川";
		plyaer.Team = "廣東":
	})
	.Build();
```

##### `WithBlackPlayer(Action<Player> action)`
Sets the configuration for the black player. If the Name or Team is not set for the player, the default value will be "Unknown".

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithBlackPlayer(player => player.Name = "趙國榮")
	.Build();
```

##### `WithGameResult(GameResult gameResult)`
Sets the game result for the Xiangqi game. The default value is GameResult.Unknown.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithGameResult(GameResult.RedWin)
	.Build();
```

##### `WithCompetition(Action<CompetitionBuilder> action)`
Sets the configuration for the competition. The competition configuration includes the event name, site, and date. 
If these fields are not set, the default value of event name and site will be "Unknown", and the default date would be today's date.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithCompetition(competition => {
		competition.Event = "World Xiangqi Championship";
		competition.Site = "Beijing, China";
		competition.Date = "2022-10-01";
	})
	.Build();
```

##### `WithBoardConfig(BoardConfig config)`
Sets the board configuration for the Xiangqi game using a `BoardConfig` class instance.
The SetPiece method is used to set the piece type and color at a specific coordinate on the board. 
It will overwrite any existing piece at that coordinate, so it is recommended to call the ``WithEmptyBoard`` method before calling ``WithBoardConfig``.

```c#
using XiangqiCore.Game;
using XiangqiCore.Boards;

XiangqiBuilder builder = new ();

BoardConfig config = new ();

config.AddPiece(new Coordinate(column: 5, row: 5), PieceType.Rook, PieceColor.Red);
config.AddPiece(new Coordinate(column: 5, row: 1), PieceType.King, PieceColor.Red);
config.AddPiece(new Coordinate(column: 4, row: 10), PieceType.King, PieceColor.Black);

XiangqiGame game =  builder
	.WithEmptyBoard()
	.WithBoardConfig(config)
	.Build();
```

##### `WithBoardConfig(Action<BoardConfig> action)`
Sets the board configuration for the Xiangqi game using the APIs directly from the `BoardConfig` class.

```c#
using XiangqiCore.Game;
using XiangqiCore.Boards;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithEmptyBoard()
	.WithBoardConfig(config => {
		config.AddPiece(new Coordinate(column: 5, row: 5), PieceType.Rook, PieceColor.Red);
		config.AddPiece(new Coordinate(column: 5, row: 1), PieceType.King, PieceColor.Red);
		config.AddPiece(new Coordinate(column: 4, row: 10), PieceType.King, PieceColor.Black);
	})
	.Build();
```

##### `WithMoveRecord(string moveRecord)`
Sets the move record for the Xiangqi game.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithMoveRecord("1. 炮二平五 炮8平5")
	.Build();
```

##### `WithGameName(string gameName)`
Sets the game name for the Xiangqi game.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithGameName("呂欽專集 第一局 ")
	.Build();
```

##### `WithDpxqGameRecord(string dpxqGameRecord)`
Sets the Xiangqi game configuration using a Dpxq game record. 
The method is used to import a game from dpxq.com. The method is still in beta, and there may be issues with parsing the game record, due to the incosistency in the game record format.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDpxqGameRecord(@"标题: 杭州环境集团队 王天一 和 四川成都懿锦金弈队 武俊强
分类: 全国象棋甲级联赛
赛事: 2023年腾讯棋牌天天象棋全国象棋甲级联赛
轮次: 决赛
布局: E42 对兵互进右马局
红方: 杭州环境集团队 王天一
黑方: 四川成都懿锦金弈队 武俊强
结果: 和棋
日期: 2023.12.10
地点: 重庆丰都
组别: 杭州-四川
台次: 第04台
评论: 中国象棋协会
作者: 张磊
备注: 第3局
记时规则: 5分＋3秒
红方用时: 6分
黑方用时: 6分钟
棋局类型: 全局
棋局性质: 超快棋
红方团体: 杭州环境集团队
红方姓名: 王天一
黑方团体: 四川成都懿锦金弈队
黑方姓名: 武俊强
棋谱主人: 东萍公司
棋谱价值: 0
浏览次数: 3086
来源网站: 1791148
 
　　第3局
 
【主变: 和棋】
　　1.　兵七进一　　卒７进１　
　　2.　马八进七　　马８进７　
　　3.　马二进一　　象３进５　
　　4.　炮八平九　　马２进３　
　　5.　车九平八　　车１平２　
　　6.　炮二平四　　马７进８　
　　7.　炮九进四　　车９进１　
　　8.　车八进六　　车９平６　
　　9.　仕四进五　　炮２退１　
　　10. 炮九平七　　卒９进１　
　　11. 相三进五　　车６进３　
　　12. 车一平三　　马８进９　
　　13. 车八退二　　炮２平８　
　　14. 车八进五　　马３退２　
　　15. 兵三进一　　卒７进１　
　　16. 相五进三　　马２进１　
　　17. 炮七平九　　车６平１　
　　18. 相三退五　　后炮平３　
　　19. 炮九平八　　车１平２　
　　20. 炮八平九　　车２平１　
　　21. 炮九平八　　车１平２　
　　22. 炮八平九　　象５进７　
　　23. 车三进三　　马９退８　
　　24. 车三进一　　炮８平３　
　　25. 马七退九　　车２平１　
　　26. 炮九平八　　车１平２　
　　27. 炮八平九　　车２平１　
　　28. 炮九平八　　象７退５　
　　29. 炮八退四　　车１进２　
　　30. 炮八平七　　马１进２　
　　31. 炮七进五　　车１进２　
　　32. 炮七平八　　车１平４　
　　33. 相七进九　　马２进３　
　　34. 车三平二　　车４退４　
　　35. 马一进三　　马８退６　
　　36. 车二平四　　马６进８　
　　37. 车四进一　　车４平６　
　　38. 马三进四　　卒５进１　
　　39. 炮八平七　　马８进６　
　　40. 炮七退四　　炮３进５　
　　41. 马四进二　　士４进５　
　　42. 相九退七
 
棋谱由 http://www.dpxq.com/ 生成")
	.Build();
```

##### `RandomisePosition(bool fromFen = true, bool allowCheck = false)`
Randomises the board position. 
If fromFen is set to true, the board will be randomised based on the current FEN. 
If fromFen is set to false, the board will be randomised based on the board config. 
If allowCheck is set to false, the randomised position will be checked to ensure that the king is not in check.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.RandomisePosition(fromFen: true, allowCheck: false)
	.Build();
```

##### `RandomisePosition(PieceCounts pieceCounts, bool allowCheck = false)`
Randomises the board position based on the `PieceCounts` class. 
If allowCheck is set to false, the randomised position will be checked to ensure that the king is not in check.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

PieceCounts pieceCounts =  new(
	RedPieces: new Dictionary<PieceType, int>()
	{
		{ PieceType.King, 1 },
		{ PieceType.Advisor, 1 },
		{ PieceType.Cannon, 1 },
	},
	BlackPieces: new Dictionary<PieceType, int>()
	{
		{ PieceType.King, 1 },
		{ PieceType.Advisor, 2 },
	}
);

XiangqiGame game = builder
	.RandomisePosition(pieceCounts, allowCheck: false)
	.Build();
```

### `BoardConfig`

The `BoardConfig` class is used to set up the board configuration for the Xiangqi game in ``XiangqiBuilder`` without the use of FEN. 
It provides a set of APIs for setting up the board configuration.

#### Public Methods

##### `AddPiece(Coordinate coordinate, PieceType pieceType, PieceColor pieceColor)`
Sets the piece type and color at a specific coordinate on the board.

```c#
using XiangqiCore.Boards;

BoardConfig config = new ();

config.AddPiece(new Coordinate(column: 5, row: 5), PieceType.Rook, PieceColor.Red);
config.AddPiece(new Coordinate(column: 5, row: 1), PieceType.King, PieceColor.Red);
config.AddPiece(new Coordinate(column: 4, row: 10), PieceType.King, PieceColor.Black);
```

##### `AddRandomPiece(Coordinate coordinate)`

Sets a random piece at a specific coordinate on the board.

```c#
using XiangqiCore.Boards;

BoardConfig config = new ();

config.AddRandomPiece(new Coordinate(column: 5, row: 5));
```

##### `SetPieceCounts(PieceCounts pieceCounts)`
Sets the piece counts for the board configuration. This is for the ``RandomisePosition`` method in the ``XiangqiBuilder``.
```c#
using XiangqiCore.Boards;

BoardConfig config = new ();

PieceCounts pieceCounts =  new(
	RedPieces: new Dictionary<PieceType, int>()
	{
		{ PieceType.King, 1 },
		{ PieceType.Advisor, 1 },
		{ PieceType.Cannon, 1 },
	},
	BlackPieces: new Dictionary<PieceType, int>()
	{
		{ PieceType.King, 1 },
		{ PieceType.Advisor, 2 },
	}
);

config.SetPieceCounts(pieceCounts);
```

### XiangqiGame

The `XiangqiGame` class is the main class that represents a game of Xiangqi. 
It provides a set of APIs for managing game states, making moves, and retrieving game information.

#### Public Methods

##### `MakeMove(string move, MoveNotationType notationType)`
Makes a move in the game based on the provided move notation and notation type. Returns a boolean representing if the move is made successfully or not.
Currently, the library supports UCCI, Chinese and English move notations.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);
 game.MakeMove("h8+7", MoveNotationType.English);
```

##### `MakeMove(Coodrinate startingPosition, Coordinate destination)`
Makes a move in the game based on the starting and destination coordinates. Returns a boolean representing if the move is made successfully or not

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove(new Coordinate(column: 5, row: 2), new Coordinate(column: 5, row: 3));
```

#### `ExportMoveHistory()`
Exports the move history of the game as a string.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);
 game.MakeMove("馬8進7", MoveNotationType.Chinese);
 game.MakeMove("馬二進三", MoveNotationType.Chinese);
 game.MakeMove("車9平8", MoveNotationType.Chinese);

string moveHistory = game.ExportMoveHistory();

Console.WriteLine(moveHistory);

// Output:
// 1. 炮二平五 馬8進7 
// 2. 馬二進三 車9平8
```

#### `ExportGameAsPgnString()`
Exports the game as a PGN string.

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

string pgnString = game.ExportGameAsPgnString();

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

#### `GeneratePgnFile(string filePath)`
#### `GeneratePgnFileAsync(string filePath, CancellationToken cancellationToken = default)`

Exports the game as a PGN file to the specified filePath.
If the file name is provided in the file path, please make sure you are using the PGN extension. If not provided, the PGN file would be default to use the GameName in the XiangqiGame class

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

 game.ExportGameAsPgnFile("game.pgn");
```

#### GenerateImage(string filePath, int moveCount = 0, bool flipHorizontal = false, bool flipVertical = false)
#### GenerateImageAsync(string filePath, int moveCount = 0, bool flipHorizontal = false, bool flipVertical = false, CancellationToken cancellationToken = default)
Generates an image of the a board position for a specified move count and saves it to the specified file path.
If the image name is provided in the file path, please make sure you are using the JPG extension. If not provided, the image would be default to use the GameName in the XiangqiGame class

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();
XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);
game.MakeMove("馬8進7", MoveNotationType.Chinese);
game.MakeMove("馬二進三", MoveNotationType.Chinese);

game.GenerateImage("C:\Users\User\Downloads", moveCount: 3);
```

####  GenerateGif(string filePath, bool flipHorizontal = false, bool flipVertical = false, decimal frameDelayInSecond = 1)
####  GenerateGifAsync(string filePath, bool flipHorizontal = false, bool flipVertical = false, decimal frameDelayInSecond = 1, CancellationToken cancellationToken = default)
Generates a GIF of the game and saves it to the specified file path.
If the image name is provided in the file path, please make sure you are using the GIF extension. If not provided, the image would be default to use the GameName in the XiangqiGame class

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();
XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);
game.MakeMove("馬8進7", MoveNotationType.Chinese);
game.MakeMove("馬二進三", MoveNotationType.Chinese);

game.GenerateGif("C:\Users\User\Downloads", frameDelayInSecond: 2);
```

### Public Properties

#### `CurrentFen`
Gets the current FEN of the game.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);
 game.MakeMove("馬8進7", MoveNotationType.Chinese);
 game.MakeMove("馬二進三", MoveNotationType.Chinese);
 game.MakeMove("車9平8", MoveNotationType.Chinese);

string currentFen = game.CurrentFen;

Console.WriteLine(currentFen);

// Output:
// rnbakabr1/9/1c4nc1/p1p1p1p1p/9/9/P1P1P1P1P/1C2C1N2/9/RNBAKAB1R w - - 4 2
```

#### `BoardPosition`
Gets the current board position of the game. It returns a deep copy of the 2D array of `Piece` objects representing the board state.

```c#
using XiangqiCore.Game;
using XiangqiCore.Extension;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);

Piece[,] boardPosition = game.BoardPosition;

Console.WriteLine(boardPosition.GetPieceAtPosition(new Coordinate(column: 5, row: 3))); 

// Output: 
// XiangqiCore.Pieces.Cannon
```

#### `MoveHistory`
Gets the move history of the game as a list of `MoveHistoryObject`.

```c#
using XiangqiCore.Game;
using XiangqiCore.Move.MoveObjects;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);

 game.MakeMove("炮二平五", MoveNotationType.Chinese);
 game.MakeMove("馬8進7", MoveNotationType.Chinese);
 game.MakeMove("馬二進三", MoveNotationType.Chinese);
 game.MakeMove("車9平8", MoveNotationType.Chinese);


IReadOnlyList<MoveHistoryObject> moveHistory = game.MoveHistory;

foreach (MoveHistoryObject move in moveHistory)
	Console.WriteLine(move.MoveNotation);

// Output:
// 炮二平五
// 馬8進7
// 馬二進三
// 車9平8

```

#### `GameName`
Gets the name of the game. This property can be set in the `XiangqiBuilder` using the `WithGameName` method.
The default value is {RedPlayerName}{GameResult}{BlackPlayerName}.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game1 =  builder
	.WithRedPlayer(player => player.Name = "吕钦")
	.WithBlackPlayer(player => player.Name = "王嘉良")
	.WithGameResult(GameResult.RedWin)
	.Build();

XiangqiGame game2 =  builder
	.WithRedPlayer(player => player.Name = "王天一")
	.WithBlackPlayer(player => player.Name = "鄭惟恫")
	.WithGameResult(GameResult.Draw)
	.Build();

XiangqiGame game3 =  builder
	.WithRedPlayer(player => player.Name = "胡榮華")
	.WithBlackPlayer(player => player.Name = "楊官璘")
	.WithGameResult(GameResult.BlackWin)
	.Build();

XiangqiGame game4 =  builder
	.WithRedPlayer(player => player.Name = "胡榮華")
	.WithBlackPlayer(player => player.Name = "楊官璘")
	.WithGameResult(GameResult.BlackWin)
	.WithGameName("1980全國個人賽 胡榮華先负楊官璘")
	.Build();

string gameName1 = game1.GameName;
string gameName2 = game2.GameName;
string gameName3 = game3.GameName;
string gameName4 = game3.GameName;

Console.WriteLine(gameName1);
Console.WriteLine(gameName2);
Console.WriteLine(gameName3);
Console.WriteLine(gameName4);

// Output: 
// 吕钦先勝王嘉良
// 王天一先和鄭惟恫
// 胡榮華先負楊官璘
// 1980全國個人賽 胡榮華先负楊官璘
```

#### `GameResult`
Gets the result of the game. This property can be set in the `XiangqiBuilder` using the `WithGameResult` method.
The default value is GameResult.Unknown.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithGameResult(GameResult.RedWin)
	.Build();

GameResult gameResult = game.GameResult;

Console.WriteLine(gameResult);

// Output:
// RedWin
```

#### `Competition`
Gets the competition information of the game. This property can be set in the `XiangqiBuilder` using the `WithCompetition` method.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithCompetition(competition => {
		competition.WithName("全国象棋个人锦标赛");
		competition.WithGameDate(DateTime.Parse"1987-11-30");
	})
	.Build();

Competition competition = game.Competition;

Console.WriteLine(competition.Name);
Console.WriteLine(competition.GameDate);

// Output:
// 全国象棋个人锦标赛
// 30/11/1987 00:00:00
```

#### `RedPlayer`
Gets the red player information of the game. This property can be set in the `XiangqiBuilder` using the `WithRedPlayer` method.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithRedPlayer(player => {
		player.Name = "吕钦";
		player.Team = "广东";
	})
	.Build();

Player redPlayer = game.RedPlayer;

Console.WriteLine(redPlayer.Name);
Console.WriteLine(redPlayer.Team);

// Output:
// 吕钦
// 广东
```

#### `BlackPlayer`
Gets the black player information of the game. This property can be set in the `XiangqiBuilder` using the `WithBlackPlayer` method.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithBlackPlayer(player => {
		player.Name = "王嘉良";
		player.Team = "黑龍江";
	})
	.Build();

Player blackPlayer = game.BlackPlayer;

Console.WriteLine(blackPlayer.Name);
Console.WriteLine(blackPlayer.Team);

// Output:
// 王嘉良
// 黑龍江
```

#### `SideToMove`
Gets the side to move in the game. The default value is Side.Red.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

Side sideToMove = game.SideToMove;

Console.WriteLine(sideToMove);

// Output:
// Red
```

### `Coordinate`
A struct representing a coordinate on the Xiangqi board. It has two properties, `Column` and `Row`, representing the column and row of the coordinate.
Below is a simple diagram of how the coodrinate in XiangqiCore is represented.
The maximum column and row are 9 and 10, respectively.

``` Bash
	Black
   1 2 3 4 5 6 7 8 9
10 ┌─┬─┬─┬─┬─┬─┬─┬─┐
9  │ │ │ │ │ │ │ │─┤
8  ├─┼─┼─┼─┼─┼─┼─┼─┤
7  │ │ │ │ │ │ │ │─┤
6  ├─┼─┼─┼─┼─┼─┼─┼─┤
5  │ │ │ │ │ │ │ │─┤
4  ├─┼─┼─┼─┼─┼─┼─┼─┤
3  │ │ │ │ │ │ │ │─┤
2  ├─┼─┼─┼─┼─┼─┼─┼─┤
1  │ │ │ │ │ │ │ │─┤
   1 2 3 4 5 6 7 8 9
	Red
```

## Release Notes

Version 1.4.1
Features:
- Rename the ``ExportGameAsPgnFile`` to ``GeneratePgnFile`` to follow naming convention
- Add ``GenerateImageAsync``, ``GenerateGifAsync``, and ``GeneratePgnFileAsync`` for asynchronous generation of image, GIF, and PGN file

Bug Fixes:
- Fix the position validation for pawns not accounting for the pawns that have not yet crossed the river


Version 1.4.0
Features:
- Functionality to randomise the board position using the `RandomisePiecePosition` method in the `XiangqiBuilder` class.
- Add the overriding behaviour of the `WithBoardConfig` method in the `XiangqiBuilder` class. Now it will overwrite the existing FEN/Board Config if it is called multiple times.


- Bug Fixes:
- Fix the issue where the `WithBoardConfig` method in the `XiangqiBuilder` class does not append the game info to the FEN string when initializing the game.
- New extension methods `GetPiecesOfType` to reduce the use of reflection in the process of getting the pieces of a specific type on the board.


Version 1.3.0

Features:
- Functionality to create a GIF from the game.
- Functionality to create an image from a game position.



Version 1.2.1

Features:
- Performance enhancement when building a Xiangqi game from DPXQ or game record
- Default the game date to null if not provided

Bug Fixes:
- Handle the exception when the GameDate format in DPXQ record cannot be parsed



Version 1.2.0

Features:
- Rename the APIs in `XiangqiBuilder` class to prefix with `With`.
- Improve the performance of the process for checking for checkmates.
- Add the `WithDpxqGameRecord` method to import a game from dpxq.com. (Beta)
- Add the `ExportGameAsPgnFile` method to export the game as a PGN file.
- `ExportMoveHistory` can now accept the MoveNotationType parameter to export the move history in the specified notation. Currently it only supports the transaltion of Chinese/English to UCCI notation. The other translation would be added in the future.
- The `MakeMove` method now can also accepts Simplified Chinese notation.

Bug Fixes:
- Fix the issue where the MakeMove method does not handle the edge case that, in some notations, there are two pieces of the same type on the same column and the notation
does not mark which piece is moving, as only one of them would be able to perform the move validly.

## Contributing

Contributions to Xiangqi-Core are welcome! If you have suggestions for improvements or bug fixes, please feel free to fork the repository and submit a pull request.

## License

Xiangqi-Core is licensed under the MIT License. See the LICENSE file for more details.

## Contact

For questions or support, please contact chijason99@gmail.com
