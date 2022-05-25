using Epsilon.Abstractions.Format;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly CanvasSettings _settings;
    private readonly IModuleService _moduleService;
    private readonly IOutcomeService _outcomeService;
    private readonly IAssignmentService _assignmentService;
    private readonly ICsvFormat _csvFormat;

    public Startup(
        ILogger<Startup> logger,
        IHostApplicationLifetime lifetime,
        IOptions<CanvasSettings> options,
        IModuleService moduleService,
        IOutcomeService outcomeService,
        IAssignmentService assignmentService,
        ICsvFormat csvFormat)
    {
        _logger = logger;
        _lifetime = lifetime;
        _settings = options.Value;
        _moduleService = moduleService;
        _outcomeService = outcomeService;
        _assignmentService = assignmentService;
        _csvFormat = csvFormat;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Epsilon, targeting course: {CourseId}", _settings.CourseId);

        _lifetime.ApplicationStarted.Register(() => Task.Run(ExecuteAsync, cancellationToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync()
    {
        var outcomeResults = await _outcomeService.AllResults(_settings.CourseId) ?? throw new InvalidOperationException();
        var masteredOutcomeResults = outcomeResults.Where(static result => result.Mastery.HasValue && result.Mastery.Value);

        var assignments = new Dictionary<int, Assignment>();
        var outcomes = new Dictionary<int, Outcome>();

        foreach (var outcomeResult in masteredOutcomeResults)
        {
            var outcomeId = int.Parse(outcomeResult.Links["learning_outcome"]);
            if (!outcomes.TryGetValue(outcomeId, out var outcome))
            {
                outcome = await _outcomeService.Find(outcomeId) ?? throw new InvalidOperationException();
                outcomes.Add(outcomeId, outcome);
            }

            outcomeResult.Outcome = outcome;

            var assignmentId = int.Parse(outcomeResult.Links["assignment"]["assignment_".Length..]);
            if (!assignments.TryGetValue(assignmentId, out var assignment))
            {
                assignment = await _assignmentService.Find(_settings.CourseId, assignmentId) ?? throw new InvalidOperationException();
                assignments.Add(assignmentId, assignment);
            }

            assignment.OutcomeResults.Add(outcomeResult);
        }

        var modules = (await _moduleService.All(_settings.CourseId) ?? throw new InvalidOperationException()).ToList();

        foreach (var module in modules)
        {
            var items = await _moduleService.AllItems(_settings.CourseId, module.Id);
            if (items != null)
            {
                
                var assignmentItems = items.Where(static item => item.Type == ModuleItemType.Assignment);

                foreach (var item in assignmentItems)
                {
                    if (item.ContentId != null && assignments.TryGetValue(item.ContentId.Value, out var assignment))
                    {
                        module.Assignments.Add(assignment);
                    }
                }
            }

        }

        LogResults(modules);
        
        ReadArguments(modules);

        _lifetime.StopApplication();
    }

    private void LogResults(IEnumerable<Module> modules)
    {
        foreach (var module in modules)
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

    private void ReadArguments(IEnumerable<Module> modules)
    {
        string[] arguments = Environment.GetCommandLineArgs();

        foreach (string argument in arguments)
        {
            if (argument.Contains("="))
            {
                if (argument.ToLower().StartsWith("format="))
                {
                    string format = argument.ToLower().Substring("format=".Length);
                    string filename = "Epsilon-Export-" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                    switch (format)
                    {
                        default:
                            Console.WriteLine("No format given");
                            break;
                        case "pdf":
                            Console.WriteLine("PDF format");
                            break;
                        case "csv":
                            Console.WriteLine("csv format");
                            _csvFormat.FormatFile(modules).CreateDocument(filename);
                            break;
                    }
                }
            }
        }
    }
}