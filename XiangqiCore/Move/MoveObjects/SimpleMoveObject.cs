using XiangqiCore.Misc;
using XiangqiCore.Pieces;

namespace XiangqiCore.Move.MoveObjects;

/// <summary>
/// Represents a simple move object, used for making/undoing moves in place, instead of deep cloning the piece array.
/// </summary>
public record class SimpleMoveObject(
	Coordinate StartingPosition, 
	Coordinate Destination, 
	Piece PieceMoved, 
	Piece PieceCaptured);
