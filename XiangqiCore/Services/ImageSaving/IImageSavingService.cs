using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;

namespace XiangqiCore.Services.ImageSaving;

public interface IImageSavingService
{
	public void Save(string filePath, string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null);
	public Task SaveAsync(string filePath, string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void Save(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null);
	public Task SaveAsync(string filePath, MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void Save(string filePath, Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null);
	public Task SaveAsync(string filePath, Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}