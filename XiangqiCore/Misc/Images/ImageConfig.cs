using System.Collections.Generic;
using XiangqiCore.Move;

namespace XiangqiCore.Misc.Images;

public record ImageConfig
{
	public static int DefaultBoardWidth => 450;
	public static int DefaultBoardHeight => 500;
	public static int DefaultSquareSize => 50;

	public int BoardWidth { get; set; }
	public int BoardHeight { get; set; }

	public float SquareOriginX { get; set; } = 0f;
	public float SquareOriginY { get; set; } = 0f;
	public float SquareStepX { get; set; } = DefaultSquareSize;
	public float SquareStepY { get; set; } = DefaultSquareSize;

	public int PieceWidth { get; set; } = DefaultSquareSize;
	public int PieceHeight { get; set; } = DefaultSquareSize;

	public int MoveIndicatorWidth { get; set; } = DefaultSquareSize;
	public int MoveIndicatorHeight { get; set; } = DefaultSquareSize;

	public string? CustomBoardImagePath { get; set; }
	public string? CustomMoveIndicatorImagePath { get; set; }
	public Dictionary<string, string>? CustomPieceImagePaths { get; set; }

	public bool FlipVertical { get; set; } = false;
	public bool FlipHorizontal { get; set; } = false;

	public bool UseBlackAndWhitePieces { get; set; } = false;

	public bool UseMoveIndicator { get; set; } = false;

	public bool UseWesternPieces { get; set; } = false;

	public bool UseBlackAndWhiteBoard { get; set; } = false;

	// The delay between each frame in the GIF in seconds
	public int FrameDelayInSecond { get; set; } = 1;

	// The move number to generate the image for
	public int MoveNumber { get; set; } = 0;
}
