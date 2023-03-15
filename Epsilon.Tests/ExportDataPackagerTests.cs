using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Export;
using Moq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Tests
{
    public class ExportDataPackagerTests
    {
        [Fact]
        public async Task GivenListOfModuleOutcomeResultCollection_WhenRequiringOpenLearningOutcomeStructure_ThenOutcomeStructureShouldBeTransformed()
        {
            // Arrange
            var module = new Module(1, "Module 1", 3, new List<ModuleItem>
            {
                new ModuleItem(1, "Module 1 Item 1", ModuleItemType.Page, 1),
                new ModuleItem(2, "Module 1 Item 2", ModuleItemType.Assignment, 2),
                new ModuleItem(3, "Module 1 Item 3", ModuleItemType.Quiz, 3)
            });

            var outcomes = new List<Outcome>
            {
                new Outcome(1, "Outcome 1", "Outcome 1 EN Short Description NL Long Description"),
                new Outcome(2, "Outcome 2", "Outcome 2 EN Short Description NL Long Description"),
            };

            var alignments = new List<Alignment>
            {
                new Alignment("1", "Alignment 1", new Uri("https://alignment1.com")),
                new Alignment("2", "Alignment 2", new Uri("https://alignment2.com")),
            };

            var outcomeResults = new List<OutcomeResult>
            {
                new OutcomeResult(false, 3, new OutcomeResultLink("user1", "1", "1", "2")),
                new OutcomeResult(true, 4.5, new OutcomeResultLink("user2", "2", "2", "3")),
                new OutcomeResult(false, null, new OutcomeResultLink("user1", "1", "1", "3")),
            };

            var links = new OutcomeResultCollectionLink(outcomes, alignments);

            var collection = new OutcomeResultCollection(outcomeResults, links);

            var moduleOutcomeResultCollection = new ModuleOutcomeResultCollection(module, collection);

            var data = new List<ModuleOutcomeResultCollection>
            {
                moduleOutcomeResultCollection
            }.ToAsyncEnumerable();

            var expectedData = new ExportData
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
                                Name = "Outcome 1 Short Description",
                                Assignments = new List<CourseAssignment>
                                {
                                    new CourseAssignment
                                    {
                                        Name = "Alignment 2 | https://alignment2.com/",
                                        Score = "Satisfactory"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var exportDataPackager = new ExportDataPackager();

            // Act
            var result = await exportDataPackager.GetExportData(data);

            // Assert
            Assert.Equal(JsonSerializer.Serialize(expectedData), JsonSerializer.Serialize(result));
        }
    }
}
