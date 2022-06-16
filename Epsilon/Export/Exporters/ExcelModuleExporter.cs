﻿using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
using ExcelLibrary.BinaryFileFormat;
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

        foreach (var module in modules.Where(static m => m.HasSubmissions()))
        {
            var worksheet = new Worksheet(module.Name);

            //Because reasons @source https://stackoverflow.com/a/8127642 
            for (var i = 0; i < 100; i++)
            {
                worksheet.Cells[i, 0] = new Cell("");
            }

            var outcomeAssignmentMap = AssociateOutcomes(module.Submissions);

            //Add headers
            worksheet.Cells[0, 0] = new Cell("KPI");
            worksheet.Cells[0, 1] = new Cell("Description");
            worksheet.Cells[0, 2] = new Cell("Assignment(s)");

            //Adding all the outcomes. 

            var index = 1;
            foreach (var (outcome, assignments) in outcomeAssignmentMap)
            {
                worksheet.Cells[index, 0] = new Cell(outcome.Title);
                worksheet.Cells[index, 1] = new Cell(ConvertHtmlToRaw(outcome.Description));
                var cellValueBuilder = new StringBuilder();

                foreach (var assignment in assignments)
                {
                    //Adding assignments to the outcomes 
                    cellValueBuilder.AppendLine($"{assignment.Name} {assignment.Url}");
                }

                worksheet.Cells[index, 2] = new Cell(cellValueBuilder.ToString());
                index++;
            }

            worksheet.Cells.ColumnWidth[0, 0] = 2000;
            worksheet.Cells.ColumnWidth[0, 1] = 8000;
            worksheet.Cells.ColumnWidth[0, 2] = 16000;

            workbook.Worksheets.Add(worksheet);
        }

        // We're forced to xls because of the older format
        workbook.Save($"{_options.FormattedOutputName}.xls");
    }

    private static string ConvertHtmlToRaw(string html)
    {
        return Regex.Replace(html, "<.*?>", " ");
    }

    private static IDictionary<Outcome, ICollection<Assignment>> AssociateOutcomes(IEnumerable<Submission> submissions)
    {
        var map = new Dictionary<Outcome, ICollection<Assignment>>();

        foreach (var (assessment, assignment) in submissions)
        {
            if (assessment == null || assignment == null)
            {
                continue;
            }

            foreach (var rating in assessment.Ratings)
            {
                if (rating.Outcome == null)
                {
                    continue;
                }

                if (!map.TryGetValue(rating.Outcome, out var assignments))
                {
                    assignments = new List<Assignment>();
                    map[rating.Outcome] = assignments;
                }

                if (!assignments.Contains(assignment))
                {
                    assignments.Add(assignment);
                }
            }
        }

        return map;
    }
}