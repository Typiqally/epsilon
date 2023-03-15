using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Epsilon.Abstractions.Export;
using Epsilon.Abstractions.Model;
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

    public void Export(ExportData data, string format)
    {
        using SpreadsheetDocument spreadsheetDocument =
            SpreadsheetDocument.Create($"{_options.FormattedOutputName}.xlsx", SpreadsheetDocumentType.Workbook);
        var workbookpart = spreadsheetDocument.AddWorkbookPart();
        workbookpart.Workbook = new Workbook();

        // Add Sheets to the Workbook.
        spreadsheetDocument.WorkbookPart!.Workbook.AppendChild<Sheets>(new Sheets());

        foreach (var module in data.CourseModules)
        {
            var worksheetPart = InsertWorksheet(module, workbookpart);

            CreateTextCell("KPI", "A", 1, worksheetPart);
            CreateTextCell("Assignment", "B", 1, worksheetPart);
            CreateTextCell("Grade", "C", 1, worksheetPart);

            uint count = 2;
            foreach (var kpi in module.Kpis)
            {
                CreateTextCell($"{kpi.Name} {kpi.Description}", "A", count, worksheetPart);

                var cellValueBuilder = new StringBuilder();
                var cellValueOutComeResultsBuilder = new StringBuilder();
                foreach (var assignment in kpi.Assignments)
                {
                    cellValueBuilder.AppendLine($"{assignment.Name} {assignment.Url}");
                    cellValueOutComeResultsBuilder.AppendLine(assignment.Score);
                }

                CreateTextCell(cellValueBuilder.ToString(), "B", count, worksheetPart);
                CreateTextCell(cellValueOutComeResultsBuilder.ToString(), "C", count,
                    worksheetPart);

                count++;
            }
        }

        workbookpart.Workbook.Save();
        spreadsheetDocument.Close();
    }

    private static Cell CreateTextCell(string content, string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        var cell = InsertCellInWorksheet(columnName, rowIndex, worksheetPart);
        cell.CellValue = new CellValue(content);
        cell.DataType = CellValues.String;
        return cell;
    }

    // Given a WorkbookPart, inserts a new worksheet.
    private static WorksheetPart InsertWorksheet(CourseModule module, WorkbookPart workbookPart)
    {
        var newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        newWorksheetPart.Worksheet = new Worksheet(new SheetData());
        newWorksheetPart.Worksheet.Save();

        var sheets = workbookPart.Workbook.GetFirstChild<Sheets>()!;
        var relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

        uint sheetId = 1;
        if (sheets.Elements<Sheet>().Any())
        {
            sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId!.Value).Max() + 1;
        }

        var sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = module.Name };
        sheets.Append(sheet);
        workbookPart.Workbook.Save();

        return newWorksheetPart;
    }

    private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        var worksheet = worksheetPart.Worksheet;
        var sheetData = worksheet.GetFirstChild<SheetData>()!;
        var cellReference = columnName + rowIndex;

        // If the worksheet does not contain a row with the specified row index, insert one.
        Row row;
        if (sheetData.Elements<Row>().Count(r => r.RowIndex! == rowIndex) != 0)
        {
            row = sheetData.Elements<Row>().First(r => r.RowIndex! == rowIndex);
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        // If there is not a cell with the specified column name, insert one.  
        if (row.Elements<Cell>().Any(c => c.CellReference!.Value == columnName + rowIndex))
        {
            return row.Elements<Cell>().First(c => c.CellReference!.Value == cellReference);
        }
        else
        {
            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            var refCell = new Cell();
            foreach (var cell in row.Elements<Cell>())
            {
                if (cell.CellReference?.Value?.Length == cellReference.Length)
                {
                    if (String.Compare(cell.CellReference.Value, cellReference, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }
            }

            var newCell = new Cell() { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }
    }
}