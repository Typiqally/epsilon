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
        
        // Set table properties for formatting.
        table.AppendChild(new TableProperties(
            new TableWidth {
                Width = "8", Type = TableWidthUnitValues.Auto, }));

        var headerRow = new TableRow();

        // Empty top-left cell.
        headerRow.AppendChild(CreateTableCellWithBorders("2500", new Paragraph(new Run(new Text("")))));

        foreach (var activity in HboIDomain.Activities)
        {
            var cell = CreateTableCellWithBorders("500");
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
                Text(HboIDomain.ArchitectureLayers.First(x => x.Id == architecture.architectureId).Name)))));

            // Add the assignment cells.
            foreach (var activityValue in HboIDomain.Activities)
            {
                var fillColor= "#fffff";
                var kpiCount = 0;
                var cell = CreateTableCellWithBorders("500");
                if (architecture.activities.Exists(x => x.activityId == activityValue.Id))
                {
                    var activity =
                        architecture
                            .activities
                            .First(x => x.activityId == activityValue.Id);
                    // Set cell color based on GradeStatus.
                    fillColor = HboIDomain.MasteryLevels.FirstOrDefault(x => x.Id == activity.masteryLevel).Color;
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
        body.AppendChild(GetLegend());
        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(table);
        body.Append(new Paragraph(new Run(new Text(""))));
        body.AppendChild(GetSkills());
        body.Append(new Paragraph(new Run(new Text(""))));

        mainDocumentPart.Document.AppendChild(body);
    }

    private OpenXmlElement GetSkills()
    {
        var table = new Table();

        var skills = ProfessionalSkillOutcomes
                     .GroupBy(static x => x.Skill)
                     .Select(group => new
                     {
                         skillId = group.Key,
                         count = group.Count(),
                         maxMastery = HboIDomain
                                      .MasteryLevels
                                      .FirstOrDefault(
                                          x => x.Id == group.Max(
                                              static m => m.MasteryLevel)),
                     });

        // Set table properties for formatting.
        table.AppendChild(new TableProperties(
            new TableWidth {
                Width = "4", Type = TableWidthUnitValues.Auto, }));

        // Create the table header row.
        var headerRow = new TableRow();

        
        foreach (var skill in HboIDomain.ProfessionalSkills)
        {
            var cell = CreateTableCellWithBorders("500");
            cell.FirstChild.Append(new TextDirection 
                { Val = TextDirectionValues.LeftToRightTopToBottom2010,});
            

            cell.Append(new Paragraph(new Run(new Text(skill.Name))));
            headerRow.AppendChild(cell);
        }

        table.AppendChild(headerRow);
        var valueRow = new TableRow();
        
        foreach (var skill in HboIDomain.ProfessionalSkills)
        {
            var value = skills.First(x => x.skillId == skill.Id);
            var cell = CreateTableCellWithBorders("500");
            cell.FirstChild?.Append(new Shading
                { Fill = value.maxMastery.Color, });
            cell.Append(new Paragraph(new Run(new Text($"{value.count}"))));
            valueRow.AppendChild(cell);
        }
        table.AppendChild(valueRow);

        return table;
    }

    private OpenXmlElement GetLegend()
    {
        var table = new Table();

        foreach (var level in HboIDomain.MasteryLevels)
        {
            var row = new TableRow();
            var cellName = CreateTableCellWithBorders("200");
            cellName.Append(new Paragraph(new Run(new Text($"Level: {level.Level}"))));

            var cellValue = CreateTableCellWithBorders("600");
            cellValue.Append(new Paragraph(new Run(new Text(""))));
            cellValue.FirstChild?.Append(new Shading
            {
                Fill = level.Color,
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
                Type = TableWidthUnitValues.Dxa, Width = width,
            });
        }

        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);

        return cell;
    }
}