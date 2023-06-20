using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

public record KpiTable(
    IEnumerable<KpiTableEntry> Entries,
    IDictionary<OutcomeGradeLevel, KpiTableEntryAssignmentGradeStatus> GradeStatus
) : IWordCompetenceComponent
{
    public static readonly IDictionary<OutcomeGradeLevel, KpiTableEntryAssignmentGradeStatus> DefaultGradeStatus = new Dictionary<OutcomeGradeLevel, KpiTableEntryAssignmentGradeStatus>
    {
        {
            OutcomeGradeLevel.One, new KpiTableEntryAssignmentGradeStatus("One", "CBF5DD")
        },
        {
            OutcomeGradeLevel.Two, new KpiTableEntryAssignmentGradeStatus("Two", "64E3A1")
        },
        {
            OutcomeGradeLevel.Three, new KpiTableEntryAssignmentGradeStatus("Three", "27A567")
        },
        {
            OutcomeGradeLevel.Four, new KpiTableEntryAssignmentGradeStatus("Four", "198450")
        },
    };
    
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();

        // Create a table, with columns for the outcomes and corresponding assignments and grades
        var table = new Table();

        // Columns header texts
        var columnsHeaders = new List<string> { "KPI", "Assignments", "Grades", };

        // Create the table header row
        var headerRow = new TableRow();

        // Create the header cells
        foreach (var columnHeader in columnsHeaders)
        {
            headerRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(columnHeader)))));
        }

        // Add the header row to the table
        table.AppendChild(headerRow);

        // Create the table body rows and cells in which the first cell is the outcome and the rest are the assignments and grades
        foreach (var entry in Entries)
        {
            var tableRow = new TableRow();

            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(entry.Kpi)))));

            var assignmentParagraphs = entry.Assignments
                .Select(static a =>
                    new Paragraph(new Run(new Hyperlink(new Run(new Text(a.Name)))
                    {
                        Anchor = a.Link.AbsoluteUri,
                    })));

            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(assignmentParagraphs)));
            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(string.Join("\n", entry.Assignments.Select(static a => a.Grade.ToString(CultureInfo.InvariantCulture)))))));

            table.AppendChild(tableRow);
        }

        body.AppendChild(GetLegend());
        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(table);

        mainDocumentPart.Document.AppendChild(body);
    }
    
    private OpenXmlElement GetLegend()
    {
        var table = new Table();
        
        foreach (var status in GradeStatus)
        {
            var row = new TableRow();
            var cellName = CreateTableCellWithBorders("200");
            cellName.Append(new Paragraph(new Run(new Text(status.Value.Level))));

            var cellValue = CreateTableCellWithBorders("200");
            cellValue.Append(new Paragraph(new Run(new Text(""))));
            cellValue.FirstChild?.Append(new Shading
            {
                Fill = status.Value.Color,
            });
            row.AppendChild(cellName);
            row.AppendChild(cellValue);
            table.AppendChild(row);
        }

        return table;
    }

    private static TableCell CreateTableCellWithBorders(string? width, params OpenXmlElement[] elements)
    {
        var cell = new TableCell();
        var cellProperties = new TableCellProperties();
        var borders = new TableCellBorders(
            new LeftBorder
            {
                Val = BorderValues.Single,
            },
            new RightBorder
            {
                Val = BorderValues.Single,
            },
            new TopBorder
            {
                Val = BorderValues.Single,
            },
            new BottomBorder
            {
                Val = BorderValues.Single,
            });

        foreach (var element in elements)
        {
            cell.Append(element);
        }

        if (width != null)
        {
            cellProperties.Append(new TableCellWidth
            {
                Type = TableWidthUnitValues.Dxa,
                Width = width,
            });
        }

        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);

        return cell;
    }
}