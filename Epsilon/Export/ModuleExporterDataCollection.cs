using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
using Module = Epsilon.Abstractions.Model.Module;
using Assignment = Epsilon.Abstractions.Model.Assignment;
using System.Diagnostics;
using Epsilon.Abstractions.Model;

namespace Epsilon.Export;

public class ModuleExporterDataCollection : IModuleExporterDataCollection
{
    public ModuleExporterDataCollection() { }

    public async Task<IEnumerable<Module>> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data)
    {
        var output = new List<Module>();

        await foreach (var item in data.Where(m => m.Collection.OutcomeResults.Any()))
        {
            var module = new Module { Name = item.Module.Name };
            var links = item.Collection.Links;

            Debug.Assert(links != null, nameof(links) + " != null");

            var alignments = links.AlignmentsDictionary;
            var outcomes = links.OutcomesDictionary;

            var moduleKpis = new List<Kpi>();

            foreach (var (outcomeId, outcome) in outcomes)
            {
                var assignmentIds = item.Collection.OutcomeResults
                    .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                    .Select(o => o.Link.Assignment);

                if (assignmentIds.Any())
                {
                    var assignments = assignmentIds
                        .Select(assignmentId => new Assignment
                        {
                            Name = alignments[assignmentId].Name + " | " + alignments[assignmentId].Url,
                            Score = item.Collection.OutcomeResults
                                .First(o => o.Link.Outcome == outcomeId && o.Link.Assignment == assignmentId)
                                .Grade() ?? "N/A"
                        })
                        .ToList();
                    
                    var outcomeResults = assignments
                        .Select(a => a.Score)
                        .ToList();

                    moduleKpis.Add(new Kpi
                    {
                        Name = outcome.Title + " " + outcome.ShortDescription(),
                        Assignments = assignments,
                    });
                }
            }

            module.Kpis = moduleKpis;
            
            output.Add(module);
        }

        return output;
    }
}