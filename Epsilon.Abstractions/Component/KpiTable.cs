using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

public record KpiTable(
    IEnumerable<KpiTableEntry> Entries,
    IDictionary<OutcomeGradeLevel, KpiTableEntryLevel> GradeStatus  
) : IWordCompetenceComponent
{
    public static readonly IDictionary<OutcomeGradeLevel, KpiTableEntryLevel> DefaultGradeStatus = new Dictionary<OutcomeGradeLevel, KpiTableEntryLevel>
    {
        {
            OutcomeGradeLevel.One, new KpiTableEntryLevel("One", "CBF5DD")
        },
        {
            OutcomeGradeLevel.Two, new KpiTableEntryLevel("Two", "64E3A1")
        },
        {
            OutcomeGradeLevel.Three, new KpiTableEntryLevel("Three", "27A567")
        },
        {
            OutcomeGradeLevel.Four, new KpiTableEntryLevel("Four", "198450")
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
            
            // KPI column
            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(entry.Kpi)))));
            
            // Assignments column
            var aParagraph = new Paragraph();
            var aRun = aParagraph.AppendChild(new Run());
            
            foreach (var assignment in entry.Assignments)
            {
                var runProperties = new RunProperties();
                var underline = new Underline { Val = UnderlineValues.Single, };
                
                runProperties.Append(underline);
                
                var rel = mainDocumentPart.AddHyperlinkRelationship(assignment.Link, true);
                var relationshipId = rel.Id;
                
                aRun.AppendChild(new Hyperlink(new Run(runProperties, new Text(assignment.Name)))
                {
                    History = OnOffValue.FromBoolean(true), Id = relationshipId,
                });
                
                aRun.AppendChild(new Break());
            }
            
            tableRow.AppendChild(CreateTableCellWithBorders("3000", aParagraph));
            
            // Grades column
            var grades = entry.Assignments.Select(static a => a.Grade);
            var gParagraph = new Paragraph();
            var gRun = gParagraph.AppendChild(new Run());
            
            foreach (var grade in grades)
            {
                gRun.AppendChild(new Text(grade));
                gRun.AppendChild(new Break());
            }
            
            tableRow.AppendChild(CreateTableCellWithBorders("3000", gParagraph));
            
            // Add the row to the table
            table.AppendChild(tableRow);
        }

        // body.AppendChild(GetLegend());
        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(table);

        mainDocumentPart.Document.AppendChild(body);
    }
    
    // private OpenXmlElement GetLegend()
    // {
    //     var table = new Table();
    //     
    //     foreach (var status in GradeStatus)
    //     {
    //         var row = new TableRow();
    //         var cellName = CreateTableCellWithBorders("200");
    //         cellName.Append(new Paragraph(new Run(new Text(status.Value.Level))));
    //
    //         var cellValue = CreateTableCellWithBorders("200");
    //         cellValue.Append(new Paragraph(new Run(new Text(""))));
    //         cellValue.FirstChild?.Append(new Shading
    //         {
    //             Fill = status.Value.Color,
    //         });
    //         row.AppendChild(cellName);
    //         row.AppendChild(cellValue);
    //         table.AppendChild(row);
    //     }
    //
    //     return table;
    // }

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