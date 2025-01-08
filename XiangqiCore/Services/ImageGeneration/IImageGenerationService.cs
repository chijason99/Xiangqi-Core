using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Services.ImageGeneration;

public interface IImageGenerationService
{
	public void GenerateImage(string filePath, string fen, ImageConfig? imageConfig = null);

	public Task GenerateImageAsync(string filePath,	string fen, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void GenerateGif(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null);

	public Task GenerateGifAsync(string filePath, IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public void GenerateGif(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null);

	public Task GenerateGifAsync(string filePath, List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}
