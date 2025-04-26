### XiangqiBuilder

The `XiangqiBuilder` class is responsible for creating instances of the Xiangqi game with different configurations. It provides a fluent API for easy configuration and initialization of game instances.

## Overview

The `XiangqiBuilder` is the primary way to configure and initialize a Xiangqi game. It allows you to:
- Set up a game with default or custom configurations.
- Configure players, board positions, and game metadata.
- Import game records or randomize board positions.

This class is designed to be flexible and supports method chaining for a fluent API experience.

---

## Table of Contents
- [Public Methods](#public-methods)
  - [WithDefaultConfiguration](#withdefaultconfiguration)
  - [WithStartingFen](#withstartingfenstring-customfen)
  - [WithEmptyBoard](#withemptyboard)
  - [Build](#build)
  - [WithRedPlayer](#withredplayeractionplayer-action)
  - [WithBlackPlayer](#withblackplayeractionplayer-action)
  - [WithGameResult](#withgameresultgameresult-gameresult)
  - [WithCompetition](#withcompetitionactioncompetitionbuilder-action)
  - [WithBoardConfig](#withboardconfigboardconfig-config)
  - [WithMoveRecord](#withmoverecordstring-moverecord-movenotationtype-movenotationtype--movenotationtypetraditionalchinese)
  - [WithGameName](#withgamenamestring-gamename)
  - [WithDpxqGameRecord](#withdpxqgamerecordstring-dpxqgamerecord-movenotationtype-movenotationtype-simplifiedchinese)  
  - [RandomisePosition](#randomisepositionbool-fromfen-true-bool-allowcheck-false)

---

## Public Methods

### `WithDefaultConfiguration()`
Sets the Xiangqi game configuration to the default starting position.

**Default Configuration**:
- Starting Fen: rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1
- Side to move: Red
- Red player: Unknown
- Black player: Unknown
- Game result: Unknown
- Competition: 
	- GameDate : null
	- Round : Unknown
	- Name : Unknown

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game = builder.WithDefaultConfiguration().Build();
```

---

### `WithStartingFen(string customFen)`
Sets the starting position according to the provided FEN (Forsyth-Edwards Notation).

**Parameters**:
- `customFen` (string): The FEN string representing the starting position. Must be a valid FEN string; otherwise, an exception will be thrown.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Exceptions**:
- `InvalidFenException`: Thrown if the provided FEN string is invalid.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game =  builder
	.WithStartingFen("1r2kabr1/4a4/1c2b1n2/p3p1C1p/2pn2P2/9/P1P1P2cP/N3C1N2/2R6/2BAKABR1 b - - 2 10")
	.Build();
```

---

### `WithEmptyBoard()`
Sets the starting position to an empty board by setting the initial FEN to "9/9/9/9/9/9/9/9/9/9 w - - 0 1". 

**Use Case**:
This is useful for setting up custom board configurations with the `WithBoardConfig` method, where you want to set up the board manually without using a FEN.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game = builder
	.WithEmptyBoard()
	.Build();
```

---

### `Build()`
Builds an instance of the Xiangqi game.

**Return Value**:
- Returns a fully configured `XiangqiGame` instance.

**Example**:
```C#
	using XiangqiCore.Game;
	XiangqiBuilder builder = new ();
	XiangqiGame game = builder.WithDefaultConfiguration().Build();
```

---

##### `WithRedPlayer(Action<Player> action)`
Sets the configuration for the red player.

**Parameters**:
- `action` (Action<Player>): A delegate to configure the red player's properties (e.g., name, team).

**Default Values**:
- If the `Name` or `Team` is not set, the default value will be "Unknown".

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
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

---

### `WithBlackPlayer(Action<Player> action)`
Sets the configuration for the black player.

**Parameters**:
- `action` (Action<Player>): A delegate to configure the black player's properties (e.g., name, team).

**Default Values**:
- If the `Name` or `Team` is not set, the default value will be "Unknown".

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new (); 

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithBlackPlayer(player => player.Name = "趙國榮")
	.Build();
```

---

### `WithGameResult(GameResult gameResult)`
Sets the game result for the Xiangqi game. The default value is GameResult.Unknown.

**Parameters**:
- `gameResult` (GameResult): The result of the game (e.g., `GameResult.RedWin`, `GameResult.BlackWin`, `GameResult.Draw`).

**Default Value**:
- The default value is `GameResult.Unknown`.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:

```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithGameResult(GameResult.RedWin)
	.Build();
```

---

### `WithCompetition(Action<CompetitionBuilder> action)`
Sets the configuration for the competition.

**Parameters**:
- `action` (Action<CompetitionBuilder>): A delegate to configure the competition details (e.g., event name, site, date).

**Default Values**:
- If these fields are not set:
  - Event name and site will default to "Unknown".
  - Date will default to today's date.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
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

---

### `WithBoardConfig(BoardConfig config)`
Sets the board configuration for the Xiangqi game using a `BoardConfig` class instance.

**Parameters**:
- `config` (BoardConfig): The board configuration object.

**Behavior**:
- The `SetPiece` method is used to set the piece type and color at a specific coordinate on the board.
- It will overwrite any existing piece at that coordinate, so it is recommended to call the `WithEmptyBoard` method before calling `WithBoardConfig`.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
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

**Parameters**:
- `action` (Action<BoardConfig>): A delegate to configure the board using the `BoardConfig` class.

**Behavior**:
- The `action` parameter allows you to directly manipulate the `BoardConfig` object to set up the board.
- It is recommended to call `WithEmptyBoard` before using this method to ensure a clean board setup.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
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

---

### `WithMoveRecord(string moveRecord, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)`
Sets the move record for the Xiangqi game.

**Parameters**:
- `moveRecord` (string): The move record in the specified notation type.
- `moveNotationType` (MoveNotationType): The type of move notation used in the move record. Defaults to `MoveNotationType.TraditionalChinese`.

**Behavior**:
- Parses the provided move record and applies the moves to the game.
- The move record must be in the specified notation type.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithMoveRecord("1. 炮二平五 炮8平5")
	.Build();
```

---

### `WithGameName(string gameName)`
Sets the game name for the Xiangqi game.

**Parameters**:
- `gameName` (string): The name of the game.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.WithGameName("呂欽專集 第一局 ")
	.Build();
```

---

### `WithDpxqGameRecord(string dpxqGameRecord, MoveNotationType moveNotationType = MoveNotationType.SimplifiedChinese)`
Sets the Xiangqi game configuration using a Dpxq game record.

**Parameters**:
- `dpxqGameRecord` (string): The Dpxq game record string.
- `moveNotationType` (MoveNotationType): The type of move notation used in the game record. Defaults to `MoveNotationType.SimplifiedChinese`.

**Behavior**:
- Parses the Dpxq game record and applies the moves to the game.
- This method is in beta and may have issues with parsing due to inconsistencies in the Dpxq game record format.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
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

---

### `RandomisePosition(bool fromFen = true, bool allowCheck = false)`
Randomizes the board position.

**Parameters**:
- `fromFen` (bool): If `true`, the board will be randomized based on the current FEN. Defaults to `true`.
- `allowCheck` (bool): If `false`, the randomization will ensure that the king is not in check. Defaults to `false`.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder
	.WithDefaultConfiguration()
	.RandomisePosition(fromFen: true, allowCheck: false)
	.Build();
```

##### `RandomisePosition(PieceCounts pieceCounts, bool allowCheck = false)`
Randomizes the board position based on the `PieceCounts` class.

**Parameters**:
- `pieceCounts` (PieceCounts): Specifies the number of each piece type for both sides.
- `allowCheck` (bool): If `false`, the randomization will ensure that the king is not in check. Defaults to `false`.

**Behavior**:
- Randomizes the board position based on the specified piece counts.
- Ensures the board is valid and adheres to the rules of Xiangqi.

**Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

**Example**:
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

--- 

### Summary
The `XiangqiBuilder` class provides a flexible and fluent API for configuring and initializing Xiangqi games. It supports a wide range of customization options, from setting up players and board positions to importing game records. For more advanced usage, refer to the [XiangqiGame API documentation](./XiangqiGame.md).