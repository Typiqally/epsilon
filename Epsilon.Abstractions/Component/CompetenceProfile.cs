using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

public record CompetenceProfile(
    IHboIDomain HboIDomain,
    IEnumerable<ProfessionalTaskResult> ProfessionalTaskOutcomes,
    IEnumerable<ProfessionalSkillResult> ProfessionalSkillOutcomes
) : IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();
        // Create a table, with rows for the outcomes and columns for the assignments.
        var table = new Table();

        var tasks = ProfessionalTaskOutcomes
                    .GroupBy(static task => task.ArchitectureLayer)
                    .Select(static group => new
                    {
                        architectureId = group.Key,
                        activities = group
                                     .GroupBy(static act => act.Activity)
                                     .Select(static group => new
                                     {
                                         activityId = group.Key,
                                         count = group.Count(),
                                         masteryLevel = group.Max(static m => m.MasteryLevel),
                                     }).OrderBy(static i => i.activityId).ToList(),
                    }).OrderBy(static i => i.architectureId).ToList();

        var hboI = HboIDomain;
        
        // Set table properties for formatting.
        table.AppendChild(new TableProperties(
            new TableWidth {
                Width = "6", Type = TableWidthUnitValues.Auto, }));

        // Calculate the header row height based on the longest assignment name.
        var headerRowHeight = hboI.Activities.Max(static a => a.Name.Length) * 111; 
        
        // Create the table header row.
        var headerRow = new TableRow();
        headerRow.AppendChild(new TableRowProperties(new TableRowHeight {
            Val = (UInt32Value)(uint)headerRowHeight, }));

        // Empty top-left cell.
        headerRow.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text("")))));

        foreach (var activity in hboI.Activities)
        {
            var cell = CreateTableCellWithBorders("100");
            cell.FirstChild.Append(new TextDirection 
                { Val = TextDirectionValues.LeftToRightTopToBottom2010,});

            cell.Append(new Paragraph(new Run(new Text(activity.Name))));
            headerRow.AppendChild(cell);
        }

        table.AppendChild(headerRow);

        // Add the outcome rows.
        foreach (var architecture in tasks)
        {
            var row = new TableRow();
            Console.WriteLine($"Architecture: {architecture.architectureId}");
            // Add the outcome title cell.
            row.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new 
                Text(hboI.ArchitectureLayers.First(x => x.Id == architecture.architectureId).Name)))));

            // Add the assignment cells.
            foreach (var activityValue in hboI.Activities)
            {
                var fillColor= "#fffff";
                var kpiCount = 0;
                var cell = CreateTableCellWithBorders("100");
                if (architecture.activities.Exists(x => x.activityId == activityValue.Id))
                {
                    var activity =
                        architecture
                            .activities
                            .First(x => x.activityId == activityValue.Id);
                    // Set cell color based on GradeStatus.
                    if (activity.masteryLevel > 0)
                    {
                        fillColor = hboI.MasteryLevels.FirstOrDefault(x => x.Id == activity.masteryLevel).Color;
                    }
                    Console.WriteLine($" MasteryLevel: {activity.masteryLevel} ");
                    Console.Write($" Color:{fillColor} ");
                    Console.WriteLine($"Masterycolors: {hboI.MasteryLevels}");

                    kpiCount = activity.count;
                }

                cell.FirstChild?.Append(new Shading
                { Fill = fillColor, });
                cell.Append(new Paragraph(new Run(new Text($"{kpiCount}"))));
                row.AppendChild(cell);
            }

            table.AppendChild(row);
        }

        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(table);
        body.Append(new Paragraph(new Run(new Text(""))));

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
                Type = TableWidthUnitValues.Dxa, Width = width,
            });
        }

        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);

        return cell;
    }
}