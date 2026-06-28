using Microsoft.Extensions.DependencyInjection;
using XiangqiCore.Game;
using XiangqiCore.Move.Commands;
using XiangqiCore.Services.GifGeneration;
using XiangqiCore.Services.GifSaving;
using XiangqiCore.Services.ImageGeneration;
using XiangqiCore.Services.ImageSaving;
using XiangqiCore.Services.JsonGeneration;
using XiangqiCore.Services.MoveParsing;
using XiangqiCore.Services.MoveTransalation;
using XiangqiCore.Services.CblLoading;
using XiangqiCore.Services.CbrLoading;
using XiangqiCore.Services.PgnGeneration;
using XiangqiCore.Services.PgnLoading;
using XiangqiCore.Services.PgnSaving;
using XiangqiCore.Services.XqfLoading;

namespace XiangqiCore.Extension;

public static class DependencyInjection
{
	public static IServiceCollection AddXiangqiCore(this IServiceCollection services)
	{
		services.AddScoped<IImageGenerationService, DefaultImageGenerationService>();
		services.AddScoped<IPgnGenerationService, DefaultPgnGenerationService>();
		services.AddScoped<IPgnLoadingService, DefaultPgnLoadingService>();
		services.AddScoped<IJsonGenerationService, DefaultJsonGenerationService>();
		services.AddScoped<IMoveParsingService, DefaultMoveParsingService>();
		services.AddScoped<IMoveTranslationService, DefaultMoveTranslationService>();
		services.AddScoped<IImageSavingService, DefaultImageSavingService>();
		services.AddScoped<IGifSavingService, DefaultGifSavingService>();
		services.AddScoped<IGifGenerationService, DefaultGifGenerationService>();
		services.AddScoped<IPgnSavingService, DefaultPgnSavingService>();
		services.AddScoped<ICblLoadingService, DefaultCblLoadingService>();
		services.AddScoped<ICbrLoadingService, DefaultCbrLoadingService>();
		services.AddScoped<IXqfLoadingService, DefaultXqfLoadingService>();

		services.AddScoped<IXiangqiBuilder, XiangqiBuilder>();

		return services;
	}
}
