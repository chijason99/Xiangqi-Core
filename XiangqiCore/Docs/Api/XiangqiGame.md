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
  - [GetMoveHistory](#getmovehistorybool-includerootnode--false-variationpath-variationpath--null)
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

 game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);
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

game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);
game.MakeMove("�R8�M7", MoveNotationType.Chinese);

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

 game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);
 game.MakeMove("�R8�M7", MoveNotationType.Chinese);
 game.MakeMove("�R���M��", MoveNotationType.Chinese);
 game.MakeMove("܇9ƽ8", MoveNotationType.Chinese);

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

 game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);

Piece[,] boardPosition = game.BoardPosition;

Console.WriteLine(boardPosition.GetPieceAtPosition(new Coordinate(column: 5, row: 3))); 

// Output: 
// XiangqiCore.Pieces.Cannon
```

---

### `GetMoveHistory`(bool includeRootNode = false, VariationPath? variationPath = null)
Gets the move history of the game as a list of `MoveHistoryObject`.

**Parameters**:
- `includeRootNode` (bool): If `true`, includes the root node in the move history, usually used when you need the starting position, for example, in the case of generating a GIF of the game. Defaults to `false`.
- `variationPath` (VariationPath): An optional parameter to specify a variation path to retrieve the move history for that specific variation.


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

 game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);

 game.MakeMove("�ڶ�ƽ��", MoveNotationType.Chinese);
 game.MakeMove("�R8�M7", MoveNotationType.Chinese);
 game.MakeMove("�R���M��", MoveNotationType.Chinese);
 game.MakeMove("܇9ƽ8", MoveNotationType.Chinese);


IReadOnlyList<MoveHistoryObject> moveHistory = game.MoveHistory;

foreach (MoveHistoryObject move in moveHistory)
	Console.WriteLine(move.MoveNotation);

// Output:
// �ڶ�ƽ��
// �R8�M7
// �R���M��
// ܇9ƽ8

```

---

### `GetMoveLine(bool includeRootNode = false, VariationPath? variationsPath = null)`
Gets the move line of the game as a list of `MoveHistoryObject` for the specified variation path.
Returns a ReadOnlyCollection of `MoveNode` representing the move line.

**Parameters**:
- `includeRootNode` (bool): If `true`, includes the root node in the move line, usually used when you need the starting position.
- `variationsPath` (VariationPath): An optional parameter to specify a variation path to retrieve the move line for that specific variation.

The `MoveNode` class has the following properties:
- **Parent**: The parent node of the current move node. The parent node is the move that was made before the current move. Note that the root node does not have a parent.
- **Variations**: A list of variations for the current move node. Variations are alternative moves that can be made from the current position.
- **MoveNumber**: The move number of the current move node. The move number is the number of moves made in the game so far.

The `VariationPath` is just a wrapper of a Dictionary<int, int> where the key is the move number and the value is the index of the variation in the `Variations` list of the `MoveNode`. 
This allows you to specify a path through the variations of the game.

For example, consider the game below that starts at the default position:
1. C2=5 c8=5
2. H2+3 h8+7
3. R1=2 r9+1

```C#
var builder = new XiangqiBuilder();
var game = builder.WithDefaultConfiguration().Build();

game.MakeMove("C2=5", MoveNotationType.English);
game.MakeMove("c8=5", MoveNotationType.English);
game.MakeMove("H2+3", MoveNotationType.English);
game.MakeMove("h8+7", MoveNotationType.English);
game.MakeMove("R1=2", MoveNotationType.English);
game.MakeMove("r9+1", MoveNotationType.English);

```

The root node would be the default position which has the move number 0. The first MoveNode would be the move C2=5, with a move number of 1,
and c8=5 would have a move number of 2, and so on.

You recently learned that other than r9+1, another possible move for black on round 3 is p7+1, which is a variation of the move r9+1.

To record this, you simply need to navigate to move number 5 (the point where the move R1=2 is made), and then make another move:

```c#
game.NavigateToMove(5);

game.MakeMove("p7+1", MoveNotationType.English);
```

Now that you have added the variation, the move line would look like this (just a visual representation, not actual code):
(using 6a and 6b because the variation is at move number 6.)
1. C2=5 c8=5
2. H2+3 h8+7
3. R1=2 (6a) r9+1
        (6b) p7+1

Let's say now you want to get the move line/move history of the game for the variation 4a, which is r9+1.
You would create a `VariationPath` with the following dictionary:
```c#
VariationPath variationPath = new VariationPath(new Dictionary<int, int> 
{
    // For move number 6, we want the first variation (starting the count from 0)
    // the move r9+1 is the first variation in this case because it is recorded first
    { 6, 0 }
});
```

And then you would call the `GetMoveLine` method with this variation path:

```c#
var moveLine = game.GetMoveLine(variationPath: variationPath);
```

You should then see the move line for the variation (4a) r9+1, and the game history would go on from there.
The same principles apply to any variation in the game, allowing you to navigate through the move history and variations easily.

--- 

### `NavigateToMove(int moveNumber, VariationPath? variationsPath = null)`
Navigates to a specific move in the game based on the move number and optional variation path.

**Parameters**:
- `moveNumber` (int): The move number to navigate to.
- `variationsPath` (VariationPath): An optional parameter to specify a variation path to navigate to that specific variation.

**Behavior**:
- Updates the current game state to reflect the position after the specified move.
- It would by default start from the root node. If the `variationsPath` is provided, it will navigate to the specified variation. Else it will always pick the first variation (main line) if there are any on the road to the target move number.

---

### `NavigateToStart()`
Navigates to the starting position of the game.

---

### `NavigateToEnd(VariationPath? variationsPath = null)`
Navigates to the end of the game, which is the last move made. If a variation path is provided, it will navigate to the end of those specific variations. Else it will always pick the first variation (main line) if there are any on the road to the end of the game.

--- 

### `NavigateToNextMove(int variationNumber = 0)`
Navigates to the next move in the game. If there are multiple variations, you can specify which variation to navigate to. By default, it picks the first variation (main line).

---

### `NavigateToPreviousMove()`
Navigates to the previous move in the game.

---

### `GameName`
Gets the name of the game.

**Example**:
```c#
using XiangqiCore.Game;

XiangqiBuilder builder = new ();

XiangqiGame game1 =  builder
	.WithRedPlayer(player => player.Name = "����")
	.WithBlackPlayer(player => player.Name = "������")
	.WithGameResult(GameResult.RedWin)
	.Build();

XiangqiGame game2 =  builder
	.WithRedPlayer(player => player.Name = "����һ")
	.WithBlackPlayer(player => player.Name = "��Ω��")
	.WithGameResult(GameResult.Draw)
	.Build();

XiangqiGame game3 =  builder
	.WithRedPlayer(player => player.Name = "���s�A")
	.WithBlackPlayer(player => player.Name = "��٭U")
	.WithGameResult(GameResult.BlackWin)
	.Build();

XiangqiGame game4 =  builder
	.WithRedPlayer(player => player.Name = "���s�A")
	.WithBlackPlayer(player => player.Name = "��٭U")
	.WithGameResult(GameResult.BlackWin)
	.WithGameName("1980ȫ������ِ ���s�A�ȸ���٭U")
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
// �����Ȅ�������
// ����һ�Ⱥ���Ω��
// ���s�A��ؓ��٭U
// 1980ȫ������ِ ���s�A�ȸ���٭U
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
		competition.WithName("ȫ��������˽�����");
		competition.WithGameDate(DateTime.Parse"1987-11-30");
	})
	.Build();

Competition competition = game.Competition;

Console.WriteLine(competition.Name);
Console.WriteLine(competition.GameDate);

// Output:
// ȫ��������˽�����
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
		player.Name = "����";
		player.Team = "�㶫";
	})
	.Build();

Player redPlayer = game.RedPlayer;

Console.WriteLine(redPlayer.Name);
Console.WriteLine(redPlayer.Team);

// Output:
// ����
// �㶫
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
		player.Name = "������";
		player.Team = "������";
	})
	.Build();

Player blackPlayer = game.BlackPlayer;

Console.WriteLine(blackPlayer.Name);
Console.WriteLine(blackPlayer.Team);

// Output:
// ������
// ������
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
A record struct representing a coordinate on the Xiangqi board. It has two properties, `Column` and `Row`, representing the column and row of the coordinate.

``` Bash
	Black
   1 2 3 4 5 6 7 8 9
10 �����Щ��Щ��Щ��Щ��Щ��Щ��Щ���
9  �� �� �� �� �� �� �� ������
8  �����੤�੤�੤�੤�੤�੤�੤��
7  �� �� �� �� �� �� �� ������
6  �����੤�੤�੤�੤�੤�੤�੤��
5  �� �� �� �� �� �� �� ������
4  �����੤�੤�੤�੤�੤�੤�੤��
3  �� �� �� �� �� �� �� ������
2  �����੤�੤�੤�੤�੤�੤�੤��
1  �� �� �� �� �� �� �� ������
   1 2 3 4 5 6 7 8 9
	Red
```

