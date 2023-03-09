using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace Epsilon.Export.Exporters;

public class ConsoleModuleExporter : ICanvasModuleExporter
{
    private readonly ILogger<ConsoleModuleExporter> _logger;
    private readonly ExportOptions _options;

    public ConsoleModuleExporter(ILogger<ConsoleModuleExporter> logger, IOptions<ExportOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }
    
    public IEnumerable<string> Formats { get; } = new[] { "console", "logs" };

    public async Task<Stream> Export(IEnumerable<Module> data, string format)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        
        foreach (var module in data)
        {
            await writer.WriteLineAsync("--------------------------------");
            await writer.WriteLineAsync($"Module: {module.Name}");
            _logger.LogInformation("--------------------------------");
            _logger.LogInformation("Module: {ModuleName}", module.Name);

            foreach (var kpi in module.Kpis)
            {
                await writer.WriteLineAsync("");
                await writer.WriteLineAsync($"KPI: {kpi.Name}");
                _logger.LogInformation("");
                _logger.LogInformation("KPI: {KpiName}", kpi.Name);

                foreach (var assignment in kpi.Assignments)
                {
                    await writer.WriteLineAsync($"- Assignment: {assignment.Name}");
                    await writer.WriteLineAsync($"  Score: {assignment.Score}");
                    _logger.LogInformation("- Assignment: {AssignmentName}", assignment.Name);
                    _logger.LogInformation("  Score: {AssignmentScore}", assignment.Score);
                }
            }
        }

        writer.Flush();
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }
}