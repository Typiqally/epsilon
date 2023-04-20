using DocumentFormat.OpenXml.Packaging;
using Epsilon.Abstractions.Model;
using Epsilon.Export.Exporters;

namespace Epsilon.Tests.Exporters;

public class WordModuleExporterTests
{
    [Fact]
    public async Task GivenExportData_WhenExportToWord_ThenFileShouldContainExportData()
    {
        // Arrange
        var data = new ExportData
        {
            CourseModules = new List<CourseModulePackage>
            {
                new CourseModulePackage
                {
                    Name = "Module 1",
                    Outcomes = new List<CourseOutcome>
                    {
                        new CourseOutcome
                        {
                            Name = "Outcome 1",
                            Description = "Short Description",
                            Assignments = new List<CourseAssignment>
                            {
                                new CourseAssignment
                                {
                                    Name = "Assignment 1",
                                    Url = new Uri("https://assignment1.com/"),
                                    Score = "Good",
                                },
                                new CourseAssignment
                                {
                                    Name = "Assignment 2",
                                    Url = new Uri("https://assignment2.com/"),
                                    Score = "Outstanding",
                                },
                            },
                        },
                    },
                },
            },
        };

        var moduleExporter = new WordModuleExporter();

        // Act
        await using var stream = await moduleExporter.Export(data, "word");
        using var document = WordprocessingDocument.Open(stream, false);

        var content = document.MainDocumentPart?.Document.Body?.InnerText;

        // Assert
        Assert.Contains("Module 1", content, StringComparison.InvariantCulture);
        Assert.Contains("Outcome 1", content, StringComparison.InvariantCulture);
        Assert.Contains("Short Description", content, StringComparison.InvariantCulture);
        Assert.Contains("Assignment 1", content, StringComparison.InvariantCulture);
        Assert.Contains("Assignment 2", content, StringComparison.InvariantCulture);
    }
}