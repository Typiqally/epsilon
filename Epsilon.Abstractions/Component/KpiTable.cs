using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public record KpiTable(
    IEnumerable<KpiTableEntry> Entries
) : IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();

        // Create a table to display outcomes, assignments, and grades
        var table = new Table();

        // Define column header texts
        var columnsHeaders = new Dictionary<string, string> { { "KPI", "3000" }, { "Assignments", "5000" }, { "Grades", "1000" }, }; 

        // Create the table header row
        var headerRow = new TableRow();

        // Create the header cells
        foreach (var columnHeader in columnsHeaders)
        {
            headerRow.AppendChild(CreateTableCellWithBorders(columnHeader.Value, new Paragraph(new Run(new Text(columnHeader.Key)))));
        }

        // Add the header row to the table
        table.AppendChild(headerRow);

        // Create the table body rows and cells
        foreach (var entry in Entries)
        {
            var tableRow = new TableRow();
            
            // Outcome (KPI) column
            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(entry.Kpi)))));
            
            // Assignments column
            var assignmentsParagraph = new Paragraph();
            var assignmentsRun = assignmentsParagraph.AppendChild(new Run());
            
            foreach (var assignment in entry.Assignments)
            {
                var rel = mainDocumentPart.AddHyperlinkRelationship(assignment.Link, true);
                var relationshipId = rel.Id;
                
                var runProperties = new RunProperties(
                    new Underline { Val = UnderlineValues.Single, });
                
                assignmentsRun.AppendChild(new Hyperlink(new Run(runProperties, new Text(assignment.Name)))
                {
                    History = OnOffValue.FromBoolean(true),
                    Id = relationshipId,
                });
            
                assignmentsRun.AppendChild(new Break());
            }
            
            tableRow.AppendChild(CreateTableCellWithBorders("5000", assignmentsParagraph));

            // Grades column
            var grades = entry.Assignments.Select(static a => a.Grade);
            var gradesParagraph = new Paragraph();
            var gradesRun = gradesParagraph.AppendChild(new Run());
        
            foreach (var grade in grades)
            {
                gradesRun.AppendChild(new Text(grade));
                gradesRun.AppendChild(new Break());
            }
        
            tableRow.AppendChild(CreateTableCellWithBorders("1000", gradesParagraph));
            
            // Add the row to the table
            table.AppendChild(tableRow);
        }
        
        // Newline to separate the table from the rest of the document
        body.Append(new Paragraph(new Run(new Text(""))));
        
        // Add the table to the document
        body.AppendChild(table);

        mainDocumentPart.Document.AppendChild(body);
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