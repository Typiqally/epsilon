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

    public IEnumerable<string> Formats { get; } = new[] { "console", "logs", "txt" };

    public string FileExtension => "txt";


    public async Task<Stream> Export(ExportData data, string format)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        foreach (var module in data.CourseModules)
        {
            await WriteLineAndLog(writer, "--------------------------------");
            await WriteLineAndLog(writer, $"Module: {module.Name}");

            foreach (var outcome in module.Outcomes)
            {
                await WriteLineAndLog(writer, "");
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

    private async Task WriteLineAndLog(TextWriter writer, string line)
    {
        await writer.WriteLineAsync(line);
        _logger.LogInformation(line);
    }
}