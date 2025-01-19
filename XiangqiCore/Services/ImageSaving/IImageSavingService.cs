using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;

namespace XiangqiCore.Services.ImageSaving;

public interface IImageSavingService
{
	public void SaveImageToFile(string filePath, string fen, ImageConfig? imageConfig = null);
	public Task SaveImageToFileAsync(string filePath, string fen, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void SaveImageToFile(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null);
	public Task SaveImageToFileAsync(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void SaveImageToFile(string filePath, Piece[,] position, ImageConfig? imageConfig = null);
	public Task SaveImageToFileAsync(string filePath, Piece[,] position, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}