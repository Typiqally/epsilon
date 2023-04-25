using System.Diagnostics.CodeAnalysis;
using System.Text;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Microsoft.Extensions.Logging;

namespace Epsilon.Export.Exporters;

public class ConsoleModuleExporter : ICanvasModuleExporter
{
    private readonly ILogger<ConsoleModuleExporter> _logger;

    public ConsoleModuleExporter(ILogger<ConsoleModuleExporter> logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> Formats { get; } = new[]
    {
        "CONSOLE",
        "LOGS",
        "TXT",
    };

    public string FileExtension => "txt";

    public async Task<Stream> Export(ExportData data, string format)
    {
        var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true);

        foreach (var module in data.CourseModules)
        {
            await WriteLineAndLog(writer, "--------------------------------");
            await WriteLineAndLog(writer, $"Module: {module.Name}");

            foreach (var outcome in module.Outcomes)
            {
                await WriteLineAndLog(writer, string.Empty);
                await WriteLineAndLog(writer, $"KPI: {outcome.Name}");

                foreach (var assignment in outcome.Assignments)
                {
                    await WriteLineAndLog(writer, $"- Assignment: {assignment.Name}");
                    await WriteLineAndLog(writer, $"  Score: {assignment.Score}");
                }
            }
        }

        await writer.FlushAsync();

        return stream;
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Cleanest way right now")]
    [SuppressMessage("Performance", "CA1848: Use the LoggerMessage delegates", Justification = "https://github.com/dotnet/roslyn-analyzers/issues/6281")]
    private async Task WriteLineAndLog(TextWriter writer, string line)
    {
        await writer.WriteLineAsync(line);
        _logger.LogInformation(line);
    }
}