namespace XiangqiCore.Misc.Images;

public class ImageConfig
{
	public static int DefaultBoardWidth => 450;
	public static int DefaultBoardHeight => 500;
	public static int DefaultSquareSize => 50;

	public bool FlipVertical { get; set; } = false;
	public bool FlipHorizontal { get; set; } = false;

	public bool UseBlackAndWhitePieces { get; set; } = false;

	public bool UseMoveIndicator { get; set; } = false;

	public bool UseWesternPieces { get; set; } = false;

	public bool UseBlackAndWhiteBoard { get; set; } = false;

	// The delay between each frame in the GIF in seconds
	public int FrameDelayInSecond { get; set; } = 1;
}
