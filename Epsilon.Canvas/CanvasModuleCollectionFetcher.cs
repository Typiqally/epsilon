using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Epsilon.Canvas;

public class CanvasModuleCollectionFetcher : ICanvasModuleCollectionFetcher
{
    private readonly ILogger<CanvasModuleCollectionFetcher> _logger;
    private readonly IModuleService _moduleService;
    private readonly ISubmissionService _submissionService;
    private readonly IOutcomeService _outcomeService;

    public CanvasModuleCollectionFetcher(
        ILogger<CanvasModuleCollectionFetcher> logger,
        IModuleService moduleService,
        ISubmissionService submissionService,
        IOutcomeService outcomeService
    )
    {
        _logger = logger;
        _moduleService = moduleService;
        _submissionService = submissionService;
        _outcomeService = outcomeService;
    }

    //TODO: Deal with KPIs with lower score
    public async Task<IEnumerable<Module>> Fetch(int courseId)
    {
        var submissions = await FetchSubmissions(courseId);
        var submissionsArray = submissions.ToArray();

        await Saturate(submissionsArray);

        var submissionAssignmentMap = new Dictionary<int, Submission>();

        foreach (var submission in submissionsArray)
        {
            if (submission.RubricAssessment == null || submission.Assignment == null)
            {
                continue;
            }

            var assignmentId = submission.Assignment.Id;

            if (submissionAssignmentMap.ContainsKey(assignmentId))
            {
                throw new InvalidOperationException();
            }

            submissionAssignmentMap[assignmentId] = submission;
        }

        var modules = await FetchModules(courseId);

        foreach (var module in modules)
        {
            var items = await _moduleService.AllItems(courseId, module.Id);

            foreach (var item in items.Where(static i => i.Type == ModuleItemType.Assignment && i.ContentId.HasValue))
            {
                if (submissionAssignmentMap.TryGetValue(item.ContentId!.Value, out var submission))
                {
                    module.Submissions.Add(submission);
                }
            }
        }

        return modules;
    }

    private async Task<IEnumerable<Module>> FetchModules(int courseId)
    {
        _logger.LogInformation("Downloading modules...");
        var modules = await _moduleService.All(courseId);
        if (modules == null)
        {
            throw new InvalidOperationException();
        }

        var moduleArray = modules.ToArray();

        foreach (var (id, name, count) in moduleArray)
        {
            _logger.LogInformation("[{Id}] {Name} with {Count} items", id, name, count);
        }

        return moduleArray;
    }

    private async Task<IEnumerable<Submission>> FetchSubmissions(int courseId)
    {
        _logger.LogInformation("Downloading submissions...");

        var submissions = await _submissionService.GetAllFromStudent(courseId, new[] { "assignment", "full_rubric_assessment" });
        if (submissions == null)
        {
            throw new InvalidOperationException();
        }

        var submissionsArray = submissions.ToArray();

        foreach (var (assessment, assignment) in submissionsArray)
        {
            _logger.LogInformation("[{Id}] {Assignment}", assignment.Id, assignment.Name);
        }

        return submissionsArray;
    }

    //TODO: Move to saturation class
    private async Task Saturate(IEnumerable<Submission> submissions)
    {
        _logger.LogInformation("Saturating submissions...");

        var cache = new Dictionary<int, Outcome>();

        foreach (var submission in submissions.Where(static s => s.RubricAssessment != null))
        {
            foreach (var rating in submission.RubricAssessment!.Ratings.Where(static r => r.OutcomeId.HasValue))
            {
                var outcomeId = rating.OutcomeId!.Value;

                if (!cache.TryGetValue(outcomeId, out var outcome))
                {
                    outcome = await _outcomeService.Find(outcomeId);
                    cache[outcomeId] = outcome;

                    _logger.LogInformation("[{Id}] {Outcome}", outcome.Id, outcome.Title);
                }

                rating.Outcome = outcome;
            }
        }
    }
}