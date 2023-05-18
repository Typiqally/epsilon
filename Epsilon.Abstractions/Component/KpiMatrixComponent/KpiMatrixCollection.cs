using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Canvas.Abstractions.Model;
using TextDirectionValues = DocumentFormat.OpenXml.Wordprocessing.TextDirectionValues;

// using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component.KpiMatrixComponent;

public record KpiMatrixCollection(IEnumerable<KpiMatrixAssignment> KpiMatrixAssignments) : ICompetenceWordComponent
{
    public OpenXmlElement ToWord()
    {
        var body = new Body();
        // Create a table, with rows for the outcomes and columns for the assignments.
            var table = new Table();

            var assignments = KpiMatrixAssignments.OrderBy(static ass => ass.Name).ToList();
        
            // Set table properties for formatting.
            table.AppendChild(new TableProperties(
                new TableWidth
                {
                    Width = "0", Type = TableWidthUnitValues.Auto,
                }));
        
            // Calculate the header row height based on the longest assignment name.
            var headerRowHeight = 0;
            if (KpiMatrixAssignments.Any())
            {
                headerRowHeight = assignments.Max(static a => a.Name.Length) * 111;
            }
        
            // Create the table header row.
            var headerRow = new TableRow();
            headerRow.AppendChild(new TableRowProperties(new TableRowHeight
            {
                Val = (UInt32Value)(uint)headerRowHeight,
            }));
        
            // Empty top-left cell.
            headerRow.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text("")))));
        
            foreach (var assignment in assignments)
            {
                var cell = CreateTableCellWithBorders("100");
                cell.FirstChild.Append(new TextDirection
                {
                    Val = TextDirectionValues.TopToBottomLeftToRightRotated,
                });
        
                cell.Append(new Paragraph(new Run(new Text(assignment.Name))));
                cell.FirstChild.Append(new Shading
                {
                    Fill = assignments.IndexOf(assignment) % 2 == 0
                        ? "FFFFFF"
                        : "d3d3d3",
                });
                headerRow.AppendChild(cell);
            }
        
            table.AppendChild(headerRow);

            var listOfOutcomes = new Dictionary<int, KpiMatrixOutcome>();
            foreach (var assignment in KpiMatrixAssignments)
            {
                foreach (var outcome in assignment.Outcomes)
                {
                    listOfOutcomes.TryAdd(outcome.Id, outcome);
                }
            }
            
           // Add the outcome rows.
            foreach (var outcome in listOfOutcomes.OrderByDescending(static o => o.Value.Title))
            {
                var row = new TableRow();
            
                // Add the outcome title cell.
                row.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text(outcome.Value.Title)))));
            
                // Add the assignment cells.
                foreach (var assignment in assignments)
                {
                    var outcomeAssignment = assignment.Outcomes.FirstOrDefault(o => o.Id == outcome.Key);
                    var cell = CreateTableCellWithBorders("100");

                    // Set cell color based on GradeStatus.
                    var fillColor = outcomeAssignment != null
                        ? outcomeAssignment.GradeStatus switch
                        {
                            GradeStatus.Approved => "44F656",
                            GradeStatus.Insufficient => "FA1818",
                            GradeStatus.NotGraded => "FAFF00",
                            _ => null,
                        }
                        : assignments.IndexOf(assignment) % 2 == 0
                            ? "FFFFFF"
                            : "d3d3d3";
                cell.FirstChild.Append(new Shading
                    {
                        Fill = fillColor,
                    });
                
                    // Add an empty text element since we're using color instead of text.
                    cell.Append(new Paragraph(new Run(new Text(""))));
                    row.AppendChild(cell);
                }
            
                table.AppendChild(row);
            }
            
            body.AppendChild(table);

            return body;
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
                Type = TableWidthUnitValues.Dxa, Width = width,
            });
        }
    
        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);
    
        return cell;
    }
}