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
		player.Name = "�S�y��";
		plyaer.Team = "�V�|":
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
	.WithBlackPlayer(player => player.Name = "�w���s")
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
	.WithMoveRecord("1. �ڶ�ƽ�� ��8ƽ5")
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
	.WithGameName("�ΚJ���� ��һ�� ")
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
	.WithDpxqGameRecord(@"����: ���ݻ������Ŷ� ����һ �� �Ĵ��ɶ�ܲ�����Ķ� �信ǿ
����: ȫ������׼�����
����: 2023����Ѷ������������ȫ������׼�����
�ִ�: ����
����: E42 �Ա����������
�췽: ���ݻ������Ŷ� ����һ
�ڷ�: �Ĵ��ɶ�ܲ�����Ķ� �信ǿ
���: ����
����: 2023.12.10
�ص�: ����ᶼ
���: ����-�Ĵ�
̨��: ��04̨
����: �й�����Э��
����: ����
��ע: ��3��
��ʱ����: 5�֣�3��
�췽��ʱ: 6��
�ڷ���ʱ: 6����
�������: ȫ��
�������: ������
�췽����: ���ݻ������Ŷ�
�췽����: ����һ
�ڷ�����: �Ĵ��ɶ�ܲ�����Ķ�
�ڷ�����: �信ǿ
��������: ��Ƽ��˾
���׼�ֵ: 0
�������: 3086
��Դ��վ: 1791148
 
������3��
 
������: ���塿
����1.�����߽�һ�����䣷������
����2.����˽��ߡ�����������
����3.�������һ�����󣳽�����
����4.���ڰ�ƽ�š�����������
����5.������ƽ�ˡ�������ƽ����
����6.���ڶ�ƽ�ġ�����������
����7.���ھŽ��ġ�������������
����8.�����˽�����������ƽ����
����9.�����Ľ��塡���ڣ��ˣ���
����10. �ھ�ƽ�ߡ����䣹������
����11. �������塡������������
����12. ��һƽ��������������
����13. �����˶������ڣ�ƽ����
����14. ���˽��塡�����ˣ���
����15. ������һ�����䣷������
����16. �������������������
����17. ����ƽ�š�������ƽ����
����18. �������塡������ƽ����
����19. �ھ�ƽ�ˡ�������ƽ����
����20. �ڰ�ƽ�š�������ƽ����
����21. �ھ�ƽ�ˡ�������ƽ����
����22. �ڰ�ƽ�š����󣵽�����
����23. ���������������ˣ���
����24. ������һ�����ڣ�ƽ����
����25. �����˾š�������ƽ����
����26. �ھ�ƽ�ˡ�������ƽ����
����27. �ڰ�ƽ�š�������ƽ����
����28. �ھ�ƽ�ˡ������ˣ���
����29. �ڰ����ġ�������������
����30. �ڰ�ƽ�ߡ�����������
����31. ���߽��塡������������
����32. ����ƽ�ˡ�������ƽ����
����33. ���߽��š�����������
����34. ����ƽ�����������ˣ���
����35. ��һ�����������ˣ���
����36. ����ƽ�ġ�����������
����37. ���Ľ�һ��������ƽ����
����38. �������ġ����䣵������
����39. �ڰ�ƽ�ߡ�����������
����40. �������ġ����ڣ�������
����41. ���Ľ�������ʿ��������
����42. �������
 
������ http://www.dpxq.com/ ����")
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

#####  `WithPlacementConstraint(PieceType pieceType, Side side, Func<Coordinate, bool> constraint)`
Sets a placement constraint for a specific piece type and side. Mainly used for the `RandomisePosition` method to ensure that certain pieces are placed according to your likings.
**Parameters**:
- `pieceType` (PieceType): The type of piece to apply the constraint to.
- `side` (Side): The side (Red or Black) for which the constraint applies.
- `constraint` (Func<Coordinate, bool>): A function that takes a `Coordinate` and returns a boolean indicating whether the piece can be placed at that column/row.
- **Return Value**:
- Returns the `XiangqiBuilder` instance for method chaining.

###### Additional Note:
This method allows you to pass in more than one constraint for a particular piece type and side, even if there are more than one pieces of that type and side. 
For instance, you want to randomize an endgame that contains 2 red rooks, you would like the first red rook to be on column 3 - 6, and the other red rook to be below row 7. 
All you need to do is first make sure there are 2 red rooks in the initial FEN or the PieceCounts class, and then pass in 2 different constraints in 2 different `WithPlacementConstraint` calls.

```c#

... omitting previous code for brevity ...
    
XiangqiGame game = builder
    .WithBoardConfig(x =>
    {
        x.SetPieceCounts(new PieceCounts(
            RedPieces: new Dictionary<PieceType, int>()
            {
                { PieceType.King, 1},	
                { PieceType.Rook, 2},	
            },
            BlackPieces: new Dictionary<PieceType, int>()
            {
                { PieceType.King, 1},	
                { PieceType.Knight, 1},
                { PieceType.Bishop, 2},
                { PieceType.Rook, 1},
            }));
    })
    .WithPlacementConstraint(PieceType.Rook, Side.Red, c => c.Column >= 3 && c.Column <= 6)
    .WithPlacementConstraint(PieceType.Rook, Side.Red, c => c.Row < 7)
    .RandomisePosition(fromFen: false, allowCheck: false)
    .Build();

```

So basically the first constraint will be applied to the first red rook, and the second constraint will be applied to the second red rook in this example.
Note that the order of the constraints does not matter, as the `RandomisePosition` method will apply all constraints to all pieces of that type and side.
If there is only one red rook, and you pass in two constraints, the first constraint will be applied to the first red rook, and the second constraint will be ignored.

### Summary
The `XiangqiBuilder` class provides a flexible and fluent API for configuring and initializing Xiangqi games. It supports a wide range of customization options, from setting up players and board positions to importing game records. For more advanced usage, refer to the [XiangqiGame API documentation](./XiangqiGame.md).