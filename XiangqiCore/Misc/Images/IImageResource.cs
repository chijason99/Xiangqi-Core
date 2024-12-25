namespace XiangqiCore.Misc.Images;

public interface IImageResource
{
	public static virtual string GetResourcePath(string resourceName) => $"XiangqiCore.Assets.Board.Pieces.{resourceName}";
}
