using Microsoft.Extensions.DependencyInjection;
using XiangqiCore.Move.Commands;
using XiangqiCore.Services.GifGeneration;
using XiangqiCore.Services.GifSaving;
using XiangqiCore.Services.ImageGeneration;
using XiangqiCore.Services.ImageSaving;
using XiangqiCore.Services.MoveParsing;
using XiangqiCore.Services.MoveTransalation;
using XiangqiCore.Services.PgnGeneration;
using XiangqiCore.Services.PgnSaving;

namespace XiangqiCore.Extension;

public static class DependencyInjection
{
	public static IServiceCollection AddXiangqiCore(this IServiceCollection services)
	{
		services.AddScoped<IImageGenerationService, DefaultImageGenerationService>();
		services.AddScoped<IPgnGenerationService, DefaultPgnGenerationService>();
		services.AddScoped<IMoveParsingService, DefaultMoveParsingService>();
		services.AddScoped<IMoveTranslationService, DefaultMoveTranslationService>();
		services.AddScoped<IImageSavingService, DefaultImageSavingService>();
		services.AddScoped<IGifSavingService, DefaultGifSavingService>();
		services.AddScoped<IGifGenerationService, DefaultGifGenerationService>();
		services.AddScoped<IPgnSavingService, DefaultPgnSavingService>();

		services.AddScoped<IMoveCommand, NotationMoveCommand>();
		services.AddScoped<IMoveCommand, CoordinateMoveCommand>();

		return services;
	}
}
