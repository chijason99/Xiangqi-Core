using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc.Images;

public class ImageResourcePathManager(ImageConfig imageConfig)
{
	private readonly ImageConfig _imageConfig = imageConfig;

	private const string BLACK_AND_WHITE_PATH = ".black_and_white";
	private const string COLOURED_PATH = ".coloured";
	private const string CHINESE_PATH = ".chinese";
	private const string WESTERN_PATH = ".western";

	public string GetPieceResourcePath(PieceType pieceType, Side side)
	{
		if (pieceType == PieceType.None)
			throw new ArgumentException("Cannot get resource path for PieceType.None");

		if (side == Side.None)
			throw new ArgumentException("Cannot get resource path for Side.None");

		string path = "XiangqiCore.Assets.Pieces";

		string stylePath = CHINESE_PATH;

		if (_imageConfig.UseWesternPieces)
			stylePath = WESTERN_PATH;

		path += stylePath;

		string colourPath = COLOURED_PATH;

		if (_imageConfig.UseBlackAndWhitePieces)
			colourPath = BLACK_AND_WHITE_PATH;

		path += colourPath;

		string pieceName = pieceType.ToString().ToLower();
		string sideName = side.ToString().ToLower();

		path += $".{sideName}_{pieceName}.png";

		return path;
	}

	public string GetBoardResourcePath()
	{
		string path = "XiangqiCore.Assets.Boards";

		string colouorPath = COLOURED_PATH;

		if (_imageConfig.UseBlackAndWhiteBoard)
			colouorPath = BLACK_AND_WHITE_PATH;

		path += colouorPath;

		path += $".board.png";

		return path;
	}

	public string GetMoveIndicatorResourcePath()
	{
		string path = "XiangqiCore.Assets.MoveIndicators";

		string colourPath = COLOURED_PATH;

		if (_imageConfig.UseBlackAndWhiteBoard)
			colourPath = BLACK_AND_WHITE_PATH;

		path += colourPath;

		path += ".move_indicator.png";
		return path;
	}
}
