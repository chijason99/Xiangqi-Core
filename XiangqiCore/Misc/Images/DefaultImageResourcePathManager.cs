using XiangqiCore.Misc.Images.Interfaces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc.Images;

public class DefaultImageResourcePathManager : IImageResourcePathManager
{
	private const string BLACK_AND_WHITE_PATH = ".black_and_white";
	private const string COLOURED_PATH = ".coloured";
	private const string CHINESE_PATH = ".chinese";
	private const string WESTERN_PATH = ".western";

	public string GetPieceResourcePath(PieceType pieceType, Side side, ImageConfig imageConfig)
	{
		ArgumentNullException.ThrowIfNull(imageConfig, nameof(imageConfig));

		if (pieceType == PieceType.None)
			throw new ArgumentException("Cannot get resource path for PieceType.None");

		if (side == Side.None)
			throw new ArgumentException("Cannot get resource path for Side.None");

		string path = "XiangqiCore.Assets.Pieces";

		string stylePath = CHINESE_PATH;

		if (imageConfig.UseWesternPieces)
			stylePath = WESTERN_PATH;

		path += stylePath;

		string colourPath = COLOURED_PATH;

		if (imageConfig.UseBlackAndWhitePieces)
			colourPath = BLACK_AND_WHITE_PATH;

		path += colourPath;

		string pieceName = pieceType.ToString().ToLower();
		string sideName = side.ToString().ToLower();

		path += $".{sideName}_{pieceName}.png";

		return path;
	}

	public string GetBoardResourcePath(ImageConfig imageConfig)
	{
		ArgumentNullException.ThrowIfNull(imageConfig, nameof(imageConfig));

		string path = "XiangqiCore.Assets.Boards";

		string colouorPath = COLOURED_PATH;

		if (imageConfig.UseBlackAndWhiteBoard)
			colouorPath = BLACK_AND_WHITE_PATH;

		path += colouorPath;

		path += $".board.png";

		return path;
	}

	public string GetMoveIndicatorResourcePath(ImageConfig imageConfig)
	{
		ArgumentNullException.ThrowIfNull(imageConfig, nameof(imageConfig));

		string path = "XiangqiCore.Assets.MoveIndicators";

		string colourPath = COLOURED_PATH;

		if (imageConfig.UseBlackAndWhiteBoard)
			colourPath = BLACK_AND_WHITE_PATH;

		path += colourPath;

		path += ".move_indicator.png";
		return path;
	}
}
