using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;
using ExcelLibrary.SpreadSheet;
using Microsoft.Extensions.Options;

namespace Epsilon.Export.Exporters;

public class ExcelModuleExporter : ICanvasModuleExporter
{
    private readonly ExportOptions _options;

    public ExcelModuleExporter(IOptions<ExportOptions> options)
    {
        _options = options.Value;
    }

    public IEnumerable<string> Formats { get; } = new[] { "xls", "xlsx", "excel" };

    public void Export(IEnumerable<Module> modules, string format)
    {
        var workbook = new Workbook();

        foreach (var module in modules)
        {
            if (!module.HasAssignments())
            {
                continue;
            }

            var outcomes = GetAllOutcomesTypes(module);
            var worksheet = new Worksheet(module.Name);

            //Because reasons @source https://stackoverflow.com/a/8127642 
            for (var i = 0; i < 100; i++)
            {
                worksheet.Cells[i, 0] = new Cell("");
            }

            //Adding all the outcomes. 
            for (var index = 0; index < outcomes.Count; index++)
            {
                worksheet.Cells[index, 0] = new Cell(outcomes[index].Title);
            }

            foreach (var assignment in module.Assignments)
            {
                foreach (var outcomeResult in assignment.OutcomeResults)
                {
                    if (outcomeResult.Outcome == null)
                    {
                        continue;
                    }

                    var row = GetOutcomeRow(outcomes, outcomeResult.Outcome);

                    //Adding assignments to the outcomes 
                    var cellValue = worksheet.Cells[row, 1].StringValue;
                    cellValue += (cellValue != "" ? "\n" : "") + assignment.Name + " " + assignment.Url;

                    worksheet.Cells[row, 1] = new Cell(cellValue);
                }
            }

            worksheet.Cells.ColumnWidth[0, 0] = 5000;
            worksheet.Cells.ColumnWidth[0, 1] = 8000;
            workbook.Worksheets.Add(worksheet);
        }

        workbook.Save($"{_options.FormattedOutputName}.xls");
    }

    private static List<Outcome> GetAllOutcomesTypes(Module module)
    {
        var addedOutcomes = new List<Outcome>();
        if (module.HasAssignments())
        {
            foreach (var assignment in module.Assignments)
            {
                foreach (var result in assignment.OutcomeResults)
                {
                    if (result.Outcome != null && !addedOutcomes.Contains(result.Outcome))
                    {
                        addedOutcomes.Add(result.Outcome);
                    }
                }
            }
        }

        return addedOutcomes.OrderByDescending(static o => o.Title.Length).ToList();
    }

    private static int GetOutcomeRow(List<Outcome> outcomes, Outcome outcome)
    {
        var result = outcomes.Find(o => o.Title == outcome.Title);

        return result != null ? outcomes.IndexOf(result) : 0;
    }
}