using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces;

namespace XiangqiCore.Services.ImageGeneration;

public interface IImageGenerationService
{
	public byte[] GenerateImage(string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null);
	public Task<byte[]> GenerateImageAsync(string fen, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public byte[] GenerateImage(MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null);
	public Task<byte[]> GenerateImageAsync(MoveHistoryObject moveHistoryObject, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);

	public byte[] GenerateImage(Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null);
	public Task<byte[]> GenerateImageAsync(Piece[,] position, Coordinate? previousLocation = null, Coordinate? currentLocation = null, ImageConfig? imageConfig = null, CancellationToken cancellationToken = default);
}