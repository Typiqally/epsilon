using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;

namespace Epsilon.Export.Exporters;

public class ExcelModuleExporter : ICanvasModuleExporter
{
    public IEnumerable<string> Formats { get; } = new[] { "xls", "xlsx", "excel" };

    public string FileExtension => "xlsx";

    public async Task<Stream> Export(ExportData data, string format)
    {
        var stream = new MemoryStream();
        using var spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
        var workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Add Sheets to the Workbook.
        spreadsheetDocument.WorkbookPart!.Workbook.AppendChild<Sheets>(new Sheets());

        var cellValueBuilder = new StringBuilder();
        var cellValueOutComeResultsBuilder = new StringBuilder();

        foreach (var module in data.CourseModules)
        {
            var worksheetPart = CreateWorksheet(module, workbookPart);

            InsertCellsInWorksheet(
                worksheetPart,
                1,
                CreateTextCell("KPI", "A", 1),
                CreateTextCell("Assignment", "B", 1),
                CreateTextCell("Grade", "C", 1)
            );

            uint count = 2;
            foreach (var outcome in module.Outcomes)
            {
                foreach (var assignment in outcome.Assignments)
                {
                    cellValueBuilder.AppendLine($"{assignment.Name} {assignment.Url}");
                    cellValueOutComeResultsBuilder.AppendLine(assignment.Score);
                }

                InsertCellsInWorksheet(
                    worksheetPart,
                    count,
                    CreateTextCell($"{outcome.Name} {outcome.Description}", "A", count),
                    CreateTextCell(cellValueBuilder.ToString(), "B", count),
                    CreateTextCell(cellValueOutComeResultsBuilder.ToString(), "C", count)
                );

                cellValueBuilder.Clear();
                cellValueOutComeResultsBuilder.Clear();

                count++;
            }
        }

        return stream;
    }

    // Given a WorkbookPart, inserts a new worksheet.
    private static WorksheetPart CreateWorksheet(CourseModule module, WorkbookPart workbookPart)
    {
        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet(new SheetData());
        worksheetPart.Worksheet.Save();

        var sheets = workbookPart.Workbook.GetFirstChild<Sheets>()!;
        var relationshipId = workbookPart.GetIdOfPart(worksheetPart);

        uint sheetId = 1;
        if (sheets.Elements<Sheet>().Any())
        {
            sheetId = sheets.Elements<Sheet>().Select(static s => s.SheetId!.Value).Max() + 1;
        }

        var sheet = new Sheet { Id = relationshipId, SheetId = sheetId, Name = module.Name };
        sheets.Append(sheet);

        workbookPart.Workbook.Save();

        return worksheetPart;
    }

    private static void InsertCellsInWorksheet(WorksheetPart worksheetPart, uint rowIndex, params Cell[] cells)
    {
        var worksheet = worksheetPart.Worksheet;
        var sheetData = worksheet.GetFirstChild<SheetData>()!;

        var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex! == rowIndex);
        if (row == null)
        {
            row = new Row { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        row.Append(cells);
        worksheet.Save();
    }

    private static Cell CreateTextCell(string value, string columnName, uint rowIndex) => new()
    {
        CellReference = columnName + rowIndex,
        CellValue = new CellValue(value),
        DataType = CellValues.String,
    };
}