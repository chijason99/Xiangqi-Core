using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Concurrent;
using System.Reflection;
using XiangqiCore.Game;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc.Images;

public class ImageConfig
{
	private static readonly ConcurrentDictionary<string, Image<Rgba32>> _imageCache = [];
	
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

		string defaultStyle = ".chinese";

		if (UseWesternPieces)
			defaultStyle = ".western";

		path += defaultStyle;

		string defaultColour = ".coloured";

		if (UseBlackAndWhitePieces)
			defaultColour += ".black_and_white";

		path += defaultColour;

		string pieceName = pieceType.ToString().ToLower();
		string sideName = side.ToString().ToLower();

		path += $".{sideName}_{pieceName}.png";

		return path;
	}

	public string GetBoardResourcePath()
	{
		string path = "XiangqiCore.Assets.Boards";

		string defaultColour = ".coloured";

		if (UseBlackAndWhiteBoard)
			defaultColour += ".black_and_white";

		path += defaultColour;

		path += $".board.png";

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
	}
}
