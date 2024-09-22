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
XiangqiGame game = await builder.WithDefaultConfiguration().BuildAsync();
	
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
- Competition: Unknown

If the above fields are not set when calling the ``BuildAsync`` method, these default values will be used by default.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder.WithDefaultConfiguration().BuildAsync();
```

##### `WithStartingFen(string customFen)`
Sets the starting position according to the provided FEN (Forsyth-Edwards Notation).

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game = await builder
	.WithStartingFen("1r2kabr1/4a4/1c2b1n2/p3p1C1p/2pn2P2/9/P1P1P2cP/N3C1N2/2R6/2BAKABR1 b - - 2 10")
	.BuildAsync();
```

##### `WithEmptyBoard()`
Sets the starting position to an empty board by setting the initial FEN to "9/9/9/9/9/9/9/9/9/9 w - - 0 1". 
This is useful for setting up custom board configurations with the ``WithBoardConfig`` method below, where you want to set up the board manually without using a FEN.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game = await builder
	.WithEmptyBoard()
	.BuildAsync();
```

##### `BuildAsync()`
Builds an instance of the Xiangqi game asynchronously.

##### `WithRedPlayer(Action<Player> action)`
Sets the configuration for the red player. If the Name or Team is not set for the player, the default value will be "Unknown".

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game = await builder
	.WithDefaultConfiguration()
	.WithRedPlayer(player => { 
		player.Name = "許銀川";
		plyaer.Team = "廣東":
	})
	.BuildAsync();
```

##### `WithBlackPlayer(Action<Player> action)`
Sets the configuration for the black player. If the Name or Team is not set for the player, the default value will be "Unknown".

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game = await builder
	.WithDefaultConfiguration()
	.WithBlackPlayer(player => player.Name = "趙國榮")
	.BuildAsync();
```

##### `WithGameResult(GameResult gameResult)`
Sets the game result for the Xiangqi game. The default value is GameResult.Unknown.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder
	.WithDefaultConfiguration()
	.WithGameResult(GameResult.RedWin)
	.BuildAsync();
```

##### `WithCompetition(Action<CompetitionBuilder> action)`
Sets the configuration for the competition. The competition configuration includes the event name, site, and date. 
If these fields are not set, the default value of event name and site will be "Unknown", and the default date would be today's date.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder
	.WithDefaultConfiguration()
	.WithCompetition(competition => {
		competition.Event = "World Xiangqi Championship";
		competition.Site = "Beijing, China";
		competition.Date = "2022-10-01";
	})
	.BuildAsync();
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

XiangqiGame game = await builder
	.WithEmptyBoard()
	.WithBoardConfig(config)
	.BuildAsync();
```

##### `WithBoardConfig(Action<BoardConfig> action)`
Sets the board configuration for the Xiangqi game using the APIs directly from the `BoardConfig` class.

```c#
using XiangqiCore.Game;
using XiangqiCore.Boards;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder
	.WithEmptyBoard()
	.WithBoardConfig(config => {
		config.AddPiece(new Coordinate(column: 5, row: 5), PieceType.Rook, PieceColor.Red);
		config.AddPiece(new Coordinate(column: 5, row: 1), PieceType.King, PieceColor.Red);
		config.AddPiece(new Coordinate(column: 4, row: 10), PieceType.King, PieceColor.Black);
	})
	.BuildAsync();
```

##### `WithMoveRecord(string moveRecord)`
Sets the move record for the Xiangqi game.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder
	.WithDefaultConfiguration()
	.WithMoveRecord("1. 炮二平五 炮8平5")
	.BuildAsync();
```

##### `WithGameName(string gameName)`
Sets the game name for the Xiangqi game.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder
	.WithDefaultConfiguration()
	.WithGameName("呂欽專集 第一局 ")
	.BuildAsync();
```

##### `WithDpxqGameRecord(string dpxqGameRecord)`
Sets the Xiangqi game configuration using a Dpxq game record. 
The method is used to import a game from dpxq.com. The method is still in beta, and there may be issues with parsing the game record, due to the incosistency in the game record format.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = await builder
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
	.BuildAsync();
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


Example:
## Contributing

Contributions to Xiangqi-Core are welcome! If you have suggestions for improvements or bug fixes, please feel free to fork the repository and submit a pull request.

## License

Xiangqi-Core is licensed under the MIT License. See the LICENSE file for more details.

## Contact

For questions or support, please contact chijason99@gmail.com
