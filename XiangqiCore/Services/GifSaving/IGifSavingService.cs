using XiangqiCore.Game;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Services.GifSaving;

public interface IGifSavingService
{
	public void SaveGifToFile(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null);
	public Task SaveGifToFileAsync(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void SaveGifToFile(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null);
	public Task SaveGifToFileAsync(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void SaveGifToFile(string filePath, XiangqiGame game, ImageConfig? imageConfig = null);
	public Task SaveGifToFileAsync(string filePath, XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}
