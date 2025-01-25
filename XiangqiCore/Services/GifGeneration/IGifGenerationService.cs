using XiangqiCore.Game;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Services.GifGeneration;

public interface IGifGenerationService
{
	public byte[] GenerateGif(IEnumerable<string> fens, ImageConfig? imageConfig = null);
	public Task<byte[]> GenerateGifAsync(IEnumerable<string> fens, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public byte[] GenerateGif(List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null);
	public Task<byte[]> GenerateGifAsync(List<MoveHistoryObject> moveHistory, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public byte[] GenerateGif(XiangqiGame game, ImageConfig? imageConfig = null);
	public Task<byte[]> GenerateGifAsync(XiangqiGame game, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}
