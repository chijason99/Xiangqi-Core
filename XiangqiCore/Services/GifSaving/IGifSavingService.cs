using XiangqiCore.Game;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Services.GifSaving;

public interface IGifSavingService
{
	public void Save(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null);
	public Task SaveAsync(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void Save(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null);
	public Task SaveAsync(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void Save(string filePath, XiangqiGame game, ImageConfig? imageConfig = null);
	public Task SaveAsync(string filePath, XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}
