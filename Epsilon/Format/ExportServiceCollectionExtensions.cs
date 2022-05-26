using Epsilon.Abstractions.Format;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Epsilon.Format;

public static class ExportServiceCollectionExtensions
{
    public static IServiceCollection AddFormat(this IServiceCollection collection, IConfiguration config)
    {
        collection.Configure<ExportSettings>(config);
        collection.AddScoped<ICsvFormat, CsvFormat>();

        return collection;
    }
}