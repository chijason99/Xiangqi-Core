using Microsoft.Extensions.DependencyInjection;
using XiangqiCore.Services.ImageGeneration;
using XiangqiCore.Services.MoveParsing;
using XiangqiCore.Services.MoveTransalation;
using XiangqiCore.Services.PgnGeneration;

namespace XiangqiCore.Extension;

public static class DependencyInjection
{
	public static IServiceCollection AddXiangqiCore(this IServiceCollection services)
	{
		services.AddScoped<IImageGenerationService, DefaultImageGenerationService>();
		services.AddScoped<IPgnGenerationService, DefaultPgnGenerationService>();
		services.AddScoped<IMoveParsingService, DefaultMoveParsingService>();
		services.AddScoped<IMoveTranslationService, DefaultMoveTranslationService>();

		return services;
	}

	public static IServiceCollection AddXiangqiServices(this IServiceCollection services, Action<XiangqiServiceOptions> configureOptions)
	{
		var options = new XiangqiServiceOptions();
		configureOptions(options);

		services.AddScoped<IMoveTranslationService>(provider => options.MoveTranslationService ?? new DefaultMoveTranslationService());
		services.AddScoped<IPgnGenerationService>(provider => options.PgnGenerationService ?? new DefaultPgnGenerationService());
		services.AddScoped<IImageGenerationService>(provider => options.ImageGenerationService ?? new DefaultImageGenerationService());
		services.AddScoped<IMoveParsingService>(provider => options.MoveParsingService ?? new DefaultMoveParsingService());

		return services;
	}
}

public class XiangqiServiceOptions
{
	public IMoveTranslationService? MoveTranslationService { get; set; }
	public IPgnGenerationService? PgnGenerationService { get; set; }
	public IImageGenerationService? ImageGenerationService { get; set; }
	public IMoveParsingService? MoveParsingService { get; set; }
}
