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
    
    public IEnumerable<string> Formats { get; } = new[] { "console", "logs" };

    public async Task Export(ExportData data, string format)
    {
        foreach (var module in data.CourseModules)
        {
            _logger.LogInformation("--------------------------------");
            _logger.LogInformation("Module: {Module}", module.Name);

            foreach (var kpi in module.Kpis)
            {
                _logger.LogInformation("");
                _logger.LogInformation("KPI: {Kpi}", kpi.Name);

                foreach (var assignment in kpi.Assignments)
                {
                    _logger.LogInformation("- Assignment: {Assignment}", assignment.Name);
                    _logger.LogInformation("  Score: {Score}", assignment.Score);
                }
            }
        }
    }
}