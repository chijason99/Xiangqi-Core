using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc.Images.Interfaces;

public interface IImageResourcePathManager
{
	public string GetPieceResourcePath(PieceType pieceType, Side side, ImageConfig imageConfig);

	public string GetPieceResourcePath(PieceType pieceType, Side side)
		=> GetPieceResourcePath(pieceType, side, new ImageConfig());

	public string GetBoardResourcePath(ImageConfig imageConfig);

	public string GetBoardResourcePath() => GetBoardResourcePath(new ImageConfig());

	public string GetMoveIndicatorResourcePath(ImageConfig imageConfig);

	public string GetMoveIndicatorResourcePath() => GetMoveIndicatorResourcePath(new ImageConfig());
}
