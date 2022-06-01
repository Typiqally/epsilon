using Epsilon.Abstractions.Export;
using Epsilon.Canvas;
using Epsilon.Export;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Epsilon.Extensions;

public static class CoreServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddCanvas(config.GetSection("Canvas"));
        services.AddExport(config.GetSection("Export"));

        return services;
    }

    private static IServiceCollection AddExport(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ExportSettings>(config);
        services.AddScoped<ICanvasModuleFileExporter, ConsoleCanvasModuleFileExporter>();
        services.AddScoped<ICanvasModuleFileExporter, CsvCanvasModuleFileExporter>();

        return services;
    }
}