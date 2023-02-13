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
        using (SpreadsheetDocument spreadsheetDocument =
               SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
        {
            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            await foreach (var item in data.Where(static m => m.Collection.OutcomeResults.Any()))
            {
                WorksheetPart worksheetPart = InsertWorksheet(item.Module, workbookpart);

                var links = item.Collection.Links;

                var alignments = links.AlignmentsDictionary;
                var outcomes = links.OutcomesDictionary;

                // sheetData.Append(AddHeader());

                uint count = 2;
                foreach (var (outcomeId, outcome) in outcomes)
                {
                    var assignmentIds = item.Collection.OutcomeResults
                        .Where(o => o.Link.Outcome == outcomeId && o.Grade() != null)
                        .Select(static o => o.Link.Assignment)
                        .ToArray();

                    if (assignmentIds.Any())
                    {
                        CreateTextCell($"{outcome.Title} {outcome.ShortDescription()}", "A", count, worksheetPart);

                        var cellValueBuilder = new StringBuilder();
                        foreach (var (_, alignment) in alignments.Where(a => assignmentIds.Contains(a.Key)))
                        {
                            cellValueBuilder.AppendLine($"{alignment.Name} {alignment.Url}");
                        }

                        CreateTextCell(cellValueBuilder.ToString(), "B", count, worksheetPart);

                        var cellValueOutComeResultsBuilder = new StringBuilder();
                        foreach (var outcomeResult in item.Collection.OutcomeResults.Where(result =>
                                     result.Link.Outcome == outcomeId))
                        {
                            cellValueOutComeResultsBuilder.AppendLine(outcomeResult.Grade());
                        }

                        Cell cell = CreateTextCell(cellValueOutComeResultsBuilder.ToString(), "C", count,
                            worksheetPart);
                        count++;
                    }
                }
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

    public static Cell CreateTextCell(string content, string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        Cell cell = InsertCellInWorksheet(columnName, rowIndex, worksheetPart);
        cell.CellValue = new CellValue(content);
        cell.DataType = CellValues.String;
        return cell;
    }

    public static Cell CreateCell(string text, string cellReference)
    {
        var tc = new Cell();
        tc.CellValue = new CellValue(text);
        tc.CellReference = cellReference;
        tc.DataType = CellValues.String;
        return tc;
    }

    // Given a WorkbookPart, inserts a new worksheet.
    private static WorksheetPart InsertWorksheet(Module module, WorkbookPart workbookPart)
    {
        // Add a new worksheet part to the workbook.
        WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        newWorksheetPart.Worksheet = new Worksheet(new SheetData());
        newWorksheetPart.Worksheet.Save();

        Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
        string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

        // Get a unique ID for the new sheet.
        uint sheetId = 1;
        if (sheets.Elements<Sheet>().Count() > 0)
        {
            sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
        }

        // Append the new worksheet and associate it with the workbook.
        Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = module.Name };
        sheets.Append(sheet);
        workbookPart.Workbook.Save();

        return newWorksheetPart;
    }

    // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
    // If the cell already exists, returns it. 
    private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        Worksheet worksheet = worksheetPart.Worksheet;
        SheetData sheetData = worksheet.GetFirstChild<SheetData>();
        string cellReference = columnName + rowIndex;

        // If the worksheet does not contain a row with the specified row index, insert one.
        Row row;
        if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        // If there is not a cell with the specified column name, insert one.  
        if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
        {
            return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
        }
        else
        {
            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (cell.CellReference.Value.Length == cellReference.Length)
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }
            }

            Cell newCell = new Cell() { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }
    }
}