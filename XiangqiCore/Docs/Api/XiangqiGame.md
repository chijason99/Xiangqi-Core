### XiangqiGame

The `XiangqiGame` class is the main class that represents a game of Xiangqi. 
It provides a set of APIs for managing game states, making moves, and retrieving game information.

---

## Overview

The `XiangqiGame` class allows you to:
- Make moves using various input formats (e.g., coordinates, notations, or custom commands).
- Undo moves and navigate through the move history.
- Retrieve game information such as the current board state, move history, and game metadata.

This class is the core of the library and is designed to handle all aspects of a Xiangqi game.

---

## Table of Contents
- [Public Methods](#public-methods)
  - [MakeMove(string move, MoveNotationType notationType)](#makemovestring-move-movenotationtype-notationtype)
  - [MakeMove(Coordinate startingPosition, Coordinate destination)](#makemovecoordinate-startingposition-coordinate-destination)
  - [MakeMove(IMoveCommand moveCommand)](#makemoveimovecommand-movecommand)
  - [UndoMove(int numberOfMovesToUndo = 1)](#undomoveint-numberofmovestoundo--1)
- [Public Properties](#public-properties)
  - [CurrentFen](#currentfen)
  - [BoardPosition](#boardposition)
  - [MoveHistory](#movehistory)
  - [GameName](#gamename)
  - [GameResult](#gameresult)
  - [Competition](#competition)
  - [RedPlayer](#redplayer)
  - [BlackPlayer](#blackplayer)
  - [SideToMove](#sidetomove)
- [Coordinate](#coordinate)

---

## Public Methods

### `MakeMove(string move, MoveNotationType notationType)`
Makes a move in the game based on the provided move notation and notation type.

**Parameters**:
- `move` (string): The move in the specified notation type.
- `notationType` (MoveNotationType): The type of move notation (e.g., `MoveNotationType.TraditionalChinese`, `MoveNotationType.SimplifiedChinese`, `MoveNotationType.English`, `MoveNotationType.UCCI`).

**Behavior**:
- Validates the move against the rules of Xiangqi.
- Updates the board state and move history if the move is valid.

**Return Value**:
- Returns `true` if the move is valid and successfully made; otherwise, `false`.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

 game.MakeMove("炮二平五", MoveNotationType.Chinese);
 game.MakeMove("h8+7", MoveNotationType.English);
```

---

### `MakeMove(Coordinate startingPosition, Coordinate destination)`
Makes a move in the game based on the starting and destination coordinates.

**Parameters**:
- `startingPosition` (Coordinate): The starting position of the piece.
- `destination` (Coordinate): The destination position of the piece.

**Behavior**:
- Validates the move against the rules of Xiangqi.
- Updates the board state and move history if the move is valid.

**Return Value**:
- Returns `true` if the move is valid and successfully made; otherwise, `false`.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

game.MakeMove(new Coordinate(column: 5, row: 2), new Coordinate(column: 5, row: 3));
```

---

### `MakeMove(IMoveCommand moveCommand)`
Makes a move in the game based on a custom implementation of the `IMoveCommand` interface.

**Parameters**:
- `moveCommand` (IMoveCommand): A custom move command implementing the `IMoveCommand` interface.

**Behavior**:
- Executes the custom move command.
- Updates the board state and move history if the move is valid.

**Return Value**:
- Returns `true` if the move is valid and successfully made; otherwise, `false`.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

IMoveCommand moveCommand = new NotationMoveCommand(
		moveParsingService: yourCustomImplementationOfMoveParsingService,
		board: game.Board,
		moveNotation: "E3+5",
		sideToMove: Side.Red,
		moveNotationType: MoveNotationType.English);

game.MakeMove(moveCommand);
```

---

### `UndoMove(int numberOfMovesToUndo = 1)`
Undoes the specified number of moves in the game.

**Parameters**:
- `numberOfMovesToUndo` (int): The number of moves to undo. Defaults to `1`.

**Behavior**:
- Reverts the board state and game information to the state before the specified number of moves.

**Return Value**:
- Returns `true` if the moves are successfully undone; otherwise, `false`.

**Example**:
```c#

using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game =  builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.Chinese);
game.MakeMove("馬8進7", MoveNotationType.Chinese);

game.UndoMove();
```

---

## Public Properties

### `CurrentFen`
Gets the current FEN string representing the board position.

**Example**:
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

---

### `BoardPosition`
Gets the current board position as a 2D array of `Piece` objects.

**Example**:
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

---

### `MoveHistory`
Gets the move history of the game as a list of `MoveHistoryObject`.

The `MoveHistoryObject` class has the following properties:

- **FenAfterMove**
- **FenBeforeMove**
- **IsCapture**: Indicates if the move resulted in a capture.
- **IsCheck**: Indicates if the move resulted in a check.
- **IsCheckmate**: Indicates if the move resulted in a checkmate.
- **HasMultiplePieceOfSameTypeOnSameColumn**: Indicates if there are multiple pieces of the same type on the same column.
- **PieceMoved**: The piece type of the piece that was moved.
- **PieceCaptured**: The piece type of the piece that was captured, if any.
- **MovingSide**: The side (Red or Black) that made the move.
- **StartingPosition**: The starting coordinate of the move.
- **Destination**: The destination coordinate of the move.
- **PieceOrder**: The order of the piece in the column, as there might be more than one pieces of the same type.

**Example**:
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

---

### `GameName`
Gets the name of the game.

**Example**:
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

---

### `GameResult`
Gets the result of the game.

**Example**:
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

---

### `Competition`
Gets the competition information of the game.

**Example**:
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

### `RedPlayer`
Gets the red player information of the game.

**Example**:
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

### `BlackPlayer`
Gets the black player information of the game.

**Example**:
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

### `SideToMove`
Gets the side to move in the game.

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