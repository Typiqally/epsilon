using System.Diagnostics;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Export;

public class ModuleDataPackager : IModuleDataPackager
{ 
    public async Task<ModuleData> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data)
    {
        var output = new List<CourseModule>();

        await foreach (var item in data.Where(m => m.Collection.OutcomeResults.Any()))
        {
            var module = new CourseModule { Name = item.Module.Name };
            var links = item.Collection.Links;

            Debug.Assert(links != null, nameof(links) + " != null");

            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            var moduleKpis = new List<CourseOutcome>();

            foreach (var (outcomeId, outcome) in outcomes)
            {
                var assignmentIds = item.Collection.OutcomeResults
                    .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                    .Select(o => o.Link.Assignment);

                if (assignmentIds.Any())
                {
                    var assignments = assignmentIds
                        .Select(assignmentId => new CourseAssignment
                        {
                            Name = alignments[assignmentId].Name + " | " + alignments[assignmentId].Url,
                            Score = item.Collection.OutcomeResults
                                .First(o => o.Link.Outcome == outcomeId && o.Link.Assignment == assignmentId)
                                .Grade() ?? "N/A"
                        })
                        .ToList();

                    moduleKpis.Add(new CourseOutcome
                    {
                        Name = outcome.Title + " " + outcome.ShortDescription(),
                        Assignments = assignments,
                    });
                }
            }

            module.Kpis = moduleKpis;
            
            output.Add(module);
        }

        return new ModuleData { CourseModules = output };
    }
}