using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Model;
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

    public async Task Export(IAsyncEnumerable<ModuleOutcomeResultCollection> data, string format)
    {
        string fileName = $"{_options.FormattedOutputName}.xlsx";
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
        {
            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            await foreach (var item in data.Where(static m => m.Collection.OutcomeResults.Any()))
            {
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                
                // Append a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = item.Module.Name
                };
                var links = item.Collection.Links;
            
                var alignments = links.AlignmentsDictionary;
                var outcomes = links.OutcomesDictionary;
                
                sheetData.Append(AddHeader());

                int count = 2;
                foreach (var (outcomeId, outcome) in outcomes)
                {
                    var assignmentIds = item.Collection.OutcomeResults
                        .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                        .Select(static o => o.Link.Assignment)
                        .ToArray();
                    
                    if (assignmentIds.Any())
                    {
                        Row row = new Row();
                        row.Append(CreateCell($"{outcome.Title} {outcome.ShortDescription()}", $"A{count}"));
                
                        var cellValueBuilder = new StringBuilder();
                
                        foreach (var (_, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
                        {
                            cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                        }
                
                        row.Append(CreateCell(cellValueBuilder.ToString(), $"B{count}"));
                
                        var cellValueOutComeResultsBuilder = new StringBuilder();
                        foreach (var outcomeResult in item.Collection.OutcomeResults.Where(result =>
                                     result.Link.Outcome == outcomeId))
                        {
                            cellValueOutComeResultsBuilder.AppendLine(outcomeResult.Grade());
                        }
                
                        row.Append(CreateCell(cellValueOutComeResultsBuilder.ToString(), $"C{count}"));
                        sheetData.AppendChild(row);
                        count++;
                    }
                }
                sheets.Append(sheet);
                // sheets.Append(sheetData);
            }
            workbookpart.Workbook.Save();
            spreadsheetDocument.Close();
        }
    }

    public static Row AddHeader()
    {
        var tr = new Row();
        tr.Append(CreateCell("KPI's", "A1"));
        tr.Append(CreateCell("Assignments", "B1"));
        tr.Append(CreateCell("Score", "C1"));
        return tr;
    }

    public static Cell CreateCell(string text, string cellReference)
    {
        var tc = new Cell();
        tc.CellValue = new CellValue(text);
        tc.CellReference = cellReference;
        tc.DataType = CellValues.String;
        return tc;
    }
}