using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;
using Microsoft.Extensions.Logging;

namespace Epsilon.Export;

public class ConsoleCanvasModuleFileExporter : ICanvasModuleFileExporter
{
    private readonly ILogger<ConsoleCanvasModuleFileExporter> _logger;

    public ConsoleCanvasModuleFileExporter(ILogger<ConsoleCanvasModuleFileExporter> logger)
    {
        _logger = logger;
    }

    public bool CanExport(string format) => format is "console" or "";

    public void Export(IEnumerable<Module> data, string path)
    {
        foreach (var module in data)
        {
            _logger.LogInformation("================ {ModuleName} ================", module.Name);

            foreach (var assignment in module.Assignments)
            {
                _logger.LogInformation("{AssignmentName}", assignment.Name);

                foreach (var outcomeResult in assignment.OutcomeResults)
                {
                    _logger.LogInformation("\t- {OutcomeTitle}", outcomeResult.Outcome?.Title);
                }
            }
        }
    }
}