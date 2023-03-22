﻿using System.Text;
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
        using var spreadsheetDocument =
            SpreadsheetDocument.Create($"{_options.FormattedOutputName}.xlsx", SpreadsheetDocumentType.Workbook);
        var workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Add Sheets to the Workbook.
        spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());

        var cellValueBuilder = new StringBuilder();
        var cellValueOutComeResultsBuilder = new StringBuilder();

        foreach (var module in data.CourseModules)
        {
            var worksheetPart = CreateWorksheet(module, workbookPart);
            worksheetPart.Worksheet.Append(
                new Columns(
                    new Column { Min = 1, Max = 1, Width = 30, CustomWidth = true },
                    new Column { Min = 2, Max = 2, Width = 60, CustomWidth = true },
                    new Column { Min = 3, Max = 3, Width = 10, CustomWidth = true }
                )
            );

            InsertCellsInWorksheet(
                worksheetPart,
                1,
                CreateTextCell("KPI", "A", 1),
                CreateTextCell("Assignment", "B", 1),
                CreateTextCell("Grade", "C", 1)
            );

            uint count = 2;
            foreach (var kpi in module.Kpis)
            {
                foreach (var assignment in kpi.Assignments)
                {
                    cellValueBuilder.AppendLine($"{assignment.Name} {assignment.Url}");
                    cellValueOutComeResultsBuilder.AppendLine(assignment.Score);
                }

                InsertCellsInWorksheet(
                    worksheetPart,
                    count,
                    CreateTextCell($"{kpi.Name} {kpi.Description}", "A", count),
                    CreateTextCell(cellValueBuilder.ToString(), "B", count),
                    CreateTextCell(cellValueOutComeResultsBuilder.ToString(), "C", count)
                );

                cellValueBuilder.Clear();
                cellValueOutComeResultsBuilder.Clear();

                count++;
            }
        }

        workbookPart.Workbook.Save();
        spreadsheetDocument.Close();
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