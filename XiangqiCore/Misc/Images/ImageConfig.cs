using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using XiangqiCore.Game;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc.Images;

public class ImageConfig
{
	private static readonly ConcurrentDictionary<string, Image<Rgba32>> _imageCache = [];
	
	private const string BLACK_AND_WHITE_PATH = ".black_and_white";
	private const string COLOURED_PATH = ".coloured";
	private const string CHINESE_PATH = ".chinese";
	private const string WESTERN_PATH = ".western";

	public static int DefaultBoardWidth => 450;
	public static int DefaultBoardHeight => 500;
	public static int DefaultSquareSize => 50;

	public bool FlipVertical { get; set; } = false;
	public bool FlipHorizontal { get; set; } = false;

	public bool UseBlackAndWhitePieces { get; set; } = false;

	public bool UseMoveIndicator { get; set; } = false;

	public bool UseWesternPieces { get; set; } = false;

	public bool UseBlackAndWhiteBoard { get; set; } = false;

	public string GetPieceResourcePath(PieceType pieceType, Side side)
	{
		if (pieceType == PieceType.None)
			throw new ArgumentException("Cannot get resource path for PieceType.None");

		if (side == Side.None)
			throw new ArgumentException("Cannot get resource path for Side.None");

		string path = "XiangqiCore.Assets.Pieces";

		string stylePath = CHINESE_PATH;

		if (UseWesternPieces)
			stylePath = WESTERN_PATH;

		path += stylePath;

		string colourPath = COLOURED_PATH;

		if (UseBlackAndWhitePieces)
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

		if (UseBlackAndWhiteBoard)
			colouorPath = BLACK_AND_WHITE_PATH;

		path += colouorPath;

		path += $".board.png";

		return path;
	}

	public string GetMoveIndicatorResourcePath()
	{
		string path = "XiangqiCore.Assets.MoveIndicators";

		string colourPath = COLOURED_PATH;

		if (UseBlackAndWhiteBoard)
			colourPath = BLACK_AND_WHITE_PATH;

		path += colourPath;

		path += ".move_indicator.png";
		return path;
	}

	public Image<Rgba32> GetImage(string resourceName)
	{
		if (!_imageCache.TryGetValue(resourceName, out var image))
		{
			using Stream stream = Assembly.GetAssembly(typeof(XiangqiBuilder))!.GetManifestResourceStream(resourceName)
				?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");

			image = Image.Load<Rgba32>(stream);
			_imageCache[resourceName] = image;
		}

		return image.Clone();
	}

	public void PreloadImages()
	{
		PieceType[] pieceTypes = Enum.GetValues(typeof(PieceType))
			.Cast<PieceType>()
			.Where(pt => pt != PieceType.None)
			.ToArray();

		Side[] sides = Enum.GetValues(typeof(Side))
			.Cast<Side>()
			.Where(pt => pt != Side.None)
			.ToArray();

		foreach (Side side in sides)
			foreach (PieceType pieceType in pieceTypes)
				GetImage(GetPieceResourcePath(pieceType, side));

		// Preload board image
		GetImage(GetBoardResourcePath());

		// Preload move indicator image
		GetImage(GetMoveIndicatorResourcePath());
	}

	public Image<Rgba32> GetPieceImage(PieceType pieceType, Side side)
		=> GetImage(GetPieceResourcePath(pieceType, side));

	public Image<Rgba32> GetBoardImage()
		=> GetImage(GetBoardResourcePath());

	public Image<Rgba32> GetMoveIndicatorImage()
		=> GetImage(GetMoveIndicatorResourcePath());

	public (int xCoordinate, int yCoordinate) GetCoordinatesAfterRotation(Coordinate originalCoordinate)
	{
		const int NUMBER_OF_ROWS = 10;
		const int NUMBER_OF_COLUMNS = 9;

		int xCoordinate = originalCoordinate.Column - 1;
		int yCoordinate = NUMBER_OF_ROWS - originalCoordinate.Row;

		// If flipping the board horizontally, both the x-coordinate and y-coordinate should be flipped
		// If flipping the board vertically, then the x-coordinate should be flipped
		// If flipping the board vertically and horizontally, then only the y-coordinate should be flipped because the x-coordinate is flipped twice
		if (FlipVertical && FlipHorizontal)
			yCoordinate = originalCoordinate.Row - 1;
		else if (FlipVertical)
			xCoordinate = NUMBER_OF_COLUMNS - originalCoordinate.Column;
		else if (FlipHorizontal)
		{
			yCoordinate = originalCoordinate.Row - 1;
			xCoordinate = NUMBER_OF_COLUMNS - originalCoordinate.Column;
		}

		return (xCoordinate, yCoordinate);
	}
}
