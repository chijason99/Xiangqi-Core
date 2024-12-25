using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiangqiCore.Misc.Images;

public class ImageConfig
{
	public static int DefaultBoardWidth => 450;
	public static int DefaultBoardHeight => 500;
	public static int DefaultSquareSize => 50;

	public bool FlipVertical { get; set; } = false;
	public bool FlipHorizontal { get; set; } = false;

	public bool UseColoredPieces { get; set; } = true;
	public bool UseBlackAndWhitePieces { get; set; } = false;

	public bool UseMoveIndicator { get; set; } = true;

	public bool UseChinesePieces { get; set; } = true;
	public bool UseWesternPieces { get; set; } = false;
}
