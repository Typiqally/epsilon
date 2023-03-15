using Epsilon.Abstractions.Model;
using Epsilon.Export;
using Epsilon.Export.Exporters;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Epsilon.Tests.Exporters
{
    public class ExcelModuleExporterTests
    {
        [Fact]
        public async Task GivenExportData_WhenExportToExcel_ThenFileShouldContainExportData()
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
                                Assignments = new List<CourseAssignment>
                                {
                                    new CourseAssignment { Name = "Assignment 1", Score = "Good" },
                                    new CourseAssignment { Name = "Assignment 2", Score = "Outstanding" },
                                }
                            }
                        }
                    }
                }
            };

            var options = new ExportOptions() { OutputName = "file_name" };
            var mockOptions = Options.Create(options);

            var moduleExporter = new ExcelModuleExporter(mockOptions);

            // Act
            using var stream = await moduleExporter.Export(data, "xls");
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();

            // Assert
            Assert.Contains("Module 1", content);
            Assert.Contains("Outcome 1", content);
            Assert.Contains("Assignment 1", content);
            Assert.Contains("Assignment 2", content);
        }
    }
}