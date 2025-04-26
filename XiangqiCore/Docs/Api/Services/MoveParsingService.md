# Move Parsing Service

The Move Parsing Service in Xiangqi-Core provides tools for parsing move notations and game records. It supports multiple notation types, including Traditional Chinese, Simplified Chinese, English, and UCCI, making it versatile for different use cases.

## Overview

The Move Parsing Service includes:
- **`IMoveParsingService`**: An interface that defines the core methods for parsing individual moves and game records.
- **`DefaultMoveParsingService`**: The default implementation of the `IMoveParsingService` interface, which uses specialized parsers for each notation type.

The service is designed to handle various notation styles and extract meaningful information, such as starting and destination coordinates, piece types, and move directions.

---

## `IMoveParsingService`

The `IMoveParsingService` interface provides a set of APIs for parsing move notations and game records.

### Public Methods

#### `ParseMove(string move, MoveNotationType moveNotationType)`
Parses a single move notation and returns a `ParsedMoveObject` containing detailed information about the move.

**Parameters**:
- `move` (string): The move notation to parse.
- `moveNotationType` (MoveNotationType): The type of move notation (e.g., Traditional Chinese, Simplified Chinese, English, or UCCI).

**Return Value**:
- Returns a `ParsedMoveObject` containing the starting and destination coordinates, piece type, move direction, and other details.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

IMoveParsingService moveParsingService = new DefaultMoveParsingService();
ParsedMoveObject move = moveParsingService.ParseMove("炮二平五", MoveNotationType.TraditionalChinese);

Console.WriteLine($"Piece: {move.PieceType}, Starting Column: {move.StartingColumn}, Move Direction: {move.MoveDirection}");
// Output: 
// Piece: Cannon, Starting Column: 2, Move Direction: Horizontal

```

---

#### `ParseGameRecord(string gameRecord)`
Parses a game record string and returns a list of move notations.

**Parameters**:
- `gameRecord` (string): The game record string to parse. This can include multiple moves, typically separated by newlines or spaces.

**Return Value**:
- Returns a `List<string>` containing individual move notations extracted from the game record.

**Example**:
```c#
IMoveParsingService moveParsingService = new DefaultMoveParsingService();

string gameRecord = @"
1.	炮二平五 馬8進7
2.	馬二進三 車9平8 ";

List<string> moves = moveParsingService.ParseGameRecord(gameRecord);

foreach (string move in moves) 
	Console.WriteLine(move);

// Output: 
// 炮二平五 
// 馬8進7 
// 馬二進三 
// 車9平8

```

---

## Default Implementation: `DefaultMoveParsingService`

The `DefaultMoveParsingService` is the default implementation of the `IMoveParsingService` interface. It uses specialized parsers for each notation type to handle the parsing of individual moves and game records.

### Constructors

#### `DefaultMoveParsingService()`
Initializes a new instance of the `DefaultMoveParsingService` class with default dependencies.

- **Dependency**: The parameterless constructor uses the following default parsers:
  - `TraditionalChineseNotationParser`
  - `SimplifiedChineseNotationParser`
  - `EnglishNotationParser`
  - `UcciNotationParser`

**Example**:
```c#
IMoveParsingService moveParsingService = new DefaultMoveParsingService();
```

---

### How It Works

1. **`ParseMove`**:
   - Delegates the parsing task to the appropriate parser based on the `MoveNotationType`.
   - Returns a `ParsedMoveObject` with detailed information about the move.

2. **`ParseGameRecord`**:
   - Splits the game record string into individual lines.
   - Uses a regular expression to extract move notations from each line.
   - Returns a list of move notations.

### Example
```c#
IMoveParsingService moveParsingService = new DefaultMoveParsingService();

// Parsing a single move 
ParsedMoveObject move = moveParsingService.ParseMove("炮二平五", MoveNotationType.TraditionalChinese); 
Console.WriteLine($"Piece: {move.PieceType}, Starting Column: {move.StartingColumn}, Move Direction: {move.MoveDirection}");

// Output:
// Piece: Cannon, Starting Column: 8, Move Direction: Horizontal

// Parsing a game record 
string gameRecord = @"
1.	炮二平五 馬8進7
2.	馬二進三 車9平8 ";

List<string> moves = moveParsingService.ParseGameRecord(gameRecord); 
foreach (string moveNotation in moves) 
	Console.WriteLine(moveNotation);

// Output: 
// 炮二平五 
// 馬8進7 
// 馬二進三 
// 車9平8

```

---

## `ParsedMoveObject`

The `ParsedMoveObject` class represents the result of parsing a move notation. It contains detailed information about the move, including the piece type, starting and destination coordinates, move direction, and more.

### Properties

- **`PieceType`** (PieceType): The type of the piece being moved (e.g., Cannon, Rook, Knight).
- **`StartingColumn`** (int): The starting column of the piece. This may be unknown for certain notations.
- **`Destination`** (Coordinate?): The destination coordinate of the move.
- **`StartingPosition`** (Coordinate?): The starting coordinate of the move.
- **`MoveDirection`** (MoveDirection): The direction of the move (e.g., Forward, Backward, Horizontal).
- **`PieceOrder`** (PieceOrder): The order of the piece in the column (e.g., First, Last, Unknown).
- **`HasMultiplePieceOfSameTypeOnSameColumn`** (bool): Indicates whether there are multiple pieces of the same type on the same column.
- **`IsFromUcciNotation`** (bool): Indicates whether the move was parsed from UCCI notation.

**Example**:
```c#
ParsedMoveObject move = new ParsedMoveObject { 
	PieceType = PieceType.Cannon, 
	StartingColumn = 2, 
	Destination = new Coordinate(5, 5), 
	MoveDirection = MoveDirection.Horizontal, 
	PieceOrder = PieceOrder.First, 
	HasMultiplePieceOfSameTypeOnSameColumn = false, 
	IsFromUcciNotation = false };

Console.WriteLine($"Piece: {move.PieceType}, Destination: {move.Destination}, Direction: {move.MoveDirection}");

// Output: 
// Piece: Cannon, Destination: (5, 5), Direction: Horizontal
```