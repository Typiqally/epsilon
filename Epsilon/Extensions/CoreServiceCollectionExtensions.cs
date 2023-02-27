using Epsilon.Abstractions.Export;
using Epsilon.Canvas;
using Epsilon.Export;
using Epsilon.Export.Exporters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Epsilon.Extensions;

public static class CoreServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddCanvas(config.GetSection("Canvas"));
        services.AddExport(config);

        return services;
    }

    private static IServiceCollection AddExport(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ExportOptions>(config);

        services.AddScoped<ICanvasModuleExporter, ConsoleModuleExporter>();
        services.AddScoped<ICanvasModuleExporter, CsvModuleExporter>();
        services.AddScoped<ICanvasModuleExporter, ExcelModuleExporter>();
        services.AddScoped<ICanvasModuleExporter, WordModuleExporter>();

        services.AddScoped<IModuleExporterCollection, ModuleExporterCollection>();
        services.AddScoped<IModuleDataPackager, ModuleDataPackager>();

        return services;
    }
}