using XiangqiCore.Game;
using XiangqiCore.Move;

namespace XiangqiCore.Services.PgnGeneration;

public interface IPgnGenerationService
{
	public void GeneratePgn(string filePath, XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese);

	public Task GeneratePgnAsync(string filePath, XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese, CancellationToken cancellationToken = default);

	public string GeneratePgnString(XiangqiGame game, MoveNotationType moveNotationType = MoveNotationType.TraditionalChinese);
}
