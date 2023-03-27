using Epsilon.Abstractions.Model;
using Epsilon.Export;
using Epsilon.Export.Exporters;
using Microsoft.Extensions.Options;
using DocumentFormat.OpenXml.Packaging;

namespace Epsilon.Tests.Exporters
{
    public class WordModuleExporterTests
    {
        [Fact]
        public async Task GivenExportData_WhenExportToWord_ThenFileShouldContainExportData()
        {
            // Arrange
            var data = new ExportData
            {
                CourseModules = new List<CourseModule>
                {
                    new CourseModule
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
                                    new CourseAssignment { Name = "Assignment 1", Url = "https://assignment1.com/", Score = "Good" },
                                    new CourseAssignment { Name = "Assignment 2", Url = "https://assignment2.com/", Score = "Outstanding" },
                                }
                            }
                        }
                    }
                }
            };

            var moduleExporter = new WordModuleExporter();

            // Act
            using var stream = await moduleExporter.Export(data, "word");
            using var document = WordprocessingDocument.Open(stream, false);

            var content = document?.MainDocumentPart?.Document?.Body?.InnerText;

            // Assert
            Assert.Contains("Module 1", content);
            Assert.Contains("Outcome 1", content);
            Assert.Contains("Short Description", content);
            Assert.Contains("Assignment 1", content);
            Assert.Contains("Assignment 2", content);
        }
    }
}
