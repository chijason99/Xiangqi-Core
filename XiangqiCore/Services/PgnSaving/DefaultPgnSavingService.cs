using System.Text;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Services.PgnGeneration;

namespace XiangqiCore.Services.PgnSaving;

public class DefaultPgnSavingService : IPgnSavingService
{
	private readonly IPgnGenerationService _pgnGenerationService;

	public DefaultPgnSavingService(IPgnGenerationService pgnGenerationService)
	{
		_pgnGenerationService = pgnGenerationService;
	}

	public DefaultPgnSavingService()
	{
		_pgnGenerationService = new DefaultPgnGenerationService();
	}

	public void Save(
		string filePath,
		XiangqiGame game,
		MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese)
	{
		string sanitizedFilePath = FileHelper.PrepareFilePath(filePath, ".pgn", game.GameName);
		byte[] pgnBytes = _pgnGenerationService.GeneratePgn(game, moveNotationType);

		FileHelper.WriteBytesToFile(sanitizedFilePath, pgnBytes);
	}

	public async Task SaveAsync(
		string filePath,
		XiangqiGame game,
		MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese,
		CancellationToken cancellationToken = default)
	{
		string sanitizedFilePath = FileHelper.PrepareFilePath(filePath, ".pgn", game.GameName);
		byte[] pgnBytes = _pgnGenerationService.GeneratePgn(game, moveNotationType);

		await FileHelper.WriteBytesToFileAsync(sanitizedFilePath, pgnBytes, cancellationToken);
	}
}
