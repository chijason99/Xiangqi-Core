### `BoardConfig`

The `BoardConfig` class is used to set up the board configuration for the Xiangqi game in `XiangqiBuilder` without the use of FEN. 
It provides a set of APIs for setting up the board configuration, allowing you to customize the initial board state.

---

## Overview

The `BoardConfig` class allows you to:
- Add specific pieces to the board at designated coordinates.
- Randomize pieces on the board.
- Set piece counts for randomization purposes.

This class is particularly useful when you want to create custom board setups or randomize the board state for unique game scenarios.

---

## Public Methods

### `AddPiece(Coordinate coordinate, PieceType pieceType, PieceColor pieceColor)`
Sets the piece type and color at a specific coordinate on the board.

**Parameters**:
- `coordinate` (Coordinate): The coordinate where the piece will be placed.
- `pieceType` (PieceType): The type of the piece (e.g., `PieceType.Rook`, `PieceType.King`).
- `pieceColor` (PieceColor): The color of the piece (e.g., `PieceColor.Red`, `PieceColor.Black`).

**Behavior**:
- If a piece already exists at the specified coordinate, it will be replaced with the new piece.
- Coordinates must be valid within the Xiangqi board dimensions (1 ¡Ü row ¡Ü 10, 1 ¡Ü column ¡Ü 9).

**Return Value**:
- This method does not return a value.

**Example**:
```c#
using XiangqiCore.Boards;

BoardConfig config = new ();

config.AddPiece(new Coordinate(column: 5, row: 5), PieceType.Rook, PieceColor.Red);
config.AddPiece(new Coordinate(column: 5, row: 1), PieceType.King, PieceColor.Red);
config.AddPiece(new Coordinate(column: 4, row: 10), PieceType.King, PieceColor.Black);
```

---

### `AddRandomPiece(Coordinate coordinate)`
Sets a random piece at a specific coordinate on the board.

**Parameters**:
- `coordinate` (Coordinate): The coordinate where the random piece will be placed.

**Behavior**:
- A random piece of either color will be placed at the specified coordinate.
- If a piece already exists at the coordinate, it will be replaced with the random piece.

**Return Value**:
- This method does not return a value.

**Example**:

```c#
using XiangqiCore.Boards;

BoardConfig config = new ();

config.AddRandomPiece(new Coordinate(column: 5, row: 5));
```

--- 

### `SetPieceCounts(PieceCounts pieceCounts)`
Sets the piece counts for the board configuration. This is used for the `RandomisePosition` method in the `XiangqiBuilder`.

**Parameters**:
- `pieceCounts` (PieceCounts): A `PieceCounts` object specifying the number of each piece type for both Red and Black sides.

**Behavior**:
- The `pieceCounts` parameter allows you to define how many of each piece type should be present on the board for both sides.
- This method is typically used in conjunction with the `RandomisePosition` method to randomize the board state based on the specified piece counts.

**Return Value**:
- This method does not return a value.

**Example**:
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

---

## Notes
- The `BoardConfig` class is designed to work seamlessly with the `XiangqiBuilder` class.
- When using `BoardConfig`, ensure that the board setup adheres to the rules of Xiangqi (e.g., only one King per side, valid piece placements).

---

### Summary
The `BoardConfig` class provides a flexible way to configure the initial state of the Xiangqi board. Whether you're setting up a standard game, creating a custom scenario, or randomizing the board, `BoardConfig` offers the tools you need to achieve your desired setup.