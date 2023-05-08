using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Epsilon.Component
{
    public class KpiMatrixComponentWordConverter : ComponentConverter<OpenXmlElement, KpiMatrixProfile>
    {
        public override Task<OpenXmlElement> Convert(KpiMatrixProfile component)
        {
            var body = new Body();

            foreach (var module in component.KpiMatrixModules)
            {
                // Create a header with the Name of the module.
                var header = new Paragraph(new Run(new Text(module.Name)));
                body.AppendChild(header);

                // Create a table, with rows for the outcomes and columns for the assignments.
                var table = new Table();

                // Set table properties for formatting.
                table.AppendChild(new TableProperties(
                    new TableWidth() { Width = "100%", Type = TableWidthUnitValues.Pct }));

                // Calculate the header row height based on the longest assignment name.
                int headerRowHeight = 0;
                if (module.KpiMatrix.Assignments.Any())
                {
                    headerRowHeight = module.KpiMatrix.Assignments.Max(a => a.Name.Length) * 50;
                }

                // Create the table header row.
                var headerRow = new TableRow();
                headerRow.AppendChild(new TableRowProperties(new TableRowHeight
                    { Val = (UInt32Value)(uint)headerRowHeight }));

                // Empty top-left cell.
                headerRow.AppendChild(CreateTableCellWithBorders(new Paragraph(new Run(new Text("")))));
                
                foreach (var assignment in module.KpiMatrix.Assignments)
                {
                    var cell = CreateTableCellWithBorders();
                    cell.FirstChild.Append(new TextDirection() { Val = TextDirectionValues.BottomToTopLeftToRight });

                    cell.Append(new Paragraph(new Run(new Text(assignment.Name))));
                    headerRow.AppendChild(cell);
                }

                table.AppendChild(headerRow);

                // Add the outcome rows.
                foreach (var outcome in module.KpiMatrix.Outcomes)
                {
                    var row = new TableRow();

                    // Add the outcome title cell.
                    row.AppendChild(CreateTableCellWithBorders(new Paragraph(new Run(new Text(outcome)))));

                    // Add the assignment cells.
                    foreach (var assignment in module.KpiMatrix.Assignments)
                    {
                        var outcomeAssignment = assignment.Outcomes.FirstOrDefault(o => o.Title == outcome);
                        var cell = CreateTableCellWithBorders();

                        // Set cell color based on GradeStatus.
                        if (outcomeAssignment != null)
                        {
                            var fillColor = outcomeAssignment.GradeStatus switch
                            {
                                GradeStatus.Approved => "44F656",
                                GradeStatus.Insufficient => "FA1818",
                                GradeStatus.NotGraded => "FAFF00",
                                _ => "FFFFFF"
                            };
                            cell.FirstChild.Append(new Shading() { Fill = fillColor });
                        }

                        // Add an empty text element since we're using color instead of text.
                        cell.Append(new Paragraph(new Run(new Text(""))));
                        row.AppendChild(cell);
                    }

                    table.AppendChild(row);
                }

                body.AppendChild(table);
            }

            return Task.FromResult<OpenXmlElement>(body);
        }

        private TableCell CreateTableCellWithBorders(params OpenXmlElement[] elements)
        {
            var cell = new TableCell();
            var cellProperties = new TableCellProperties();
            var borders = new TableCellBorders(
                new LeftBorder() { Val = BorderValues.Single },
                new RightBorder() { Val = BorderValues.Single },
                new TopBorder() { Val = BorderValues.Single },
                new BottomBorder() { Val = BorderValues.Single });

            if (elements != null)
            {
                foreach (var element in elements)
                {
                    cell.Append(element);
                }
            }

            cellProperties.Append(borders);
            cell.PrependChild(cellProperties); // Move the PrependChild here

            return cell;
        }

    }
}