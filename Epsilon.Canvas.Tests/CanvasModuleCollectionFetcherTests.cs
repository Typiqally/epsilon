using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Epsilon.Canvas.Abstractions;

namespace Epsilon.Canvas.Tests
{
    public class CanvasModuleCollectionFetcherTests
    {
        [Fact]
        public async Task
            GivenAllowedModulesAndCourseId_WhenModuleAndOutcomeServiceAreMocked_ThenReturnsExpectedModuleOutcomeResultCollection()
        {
            // Arrange
            var courseId = 123;
            var allowedModules = new[] {"Module 1", "Module 3"};
            var outcomeServiceMock = new Mock<IOutcomeHttpService>();
            var moduleServiceMock = new Mock<IModuleHttpService>();

            var expectedResults = new List<ModuleOutcomeResultCollection>
            {
                new ModuleOutcomeResultCollection(
                    new Module(1, "Module 1", 3, new List<ModuleItem>
                    {
                        new ModuleItem(1, "Module 1 Item 1", ModuleItemType.Page, 1),
                        new ModuleItem(2, "Module 1 Item 2", ModuleItemType.Assignment, 2),
                        new ModuleItem(3, "Module 1 Item 3", ModuleItemType.Quiz, 3)
                    }),
                    new OutcomeResultCollection(
                        new List<OutcomeResult> { },
                        new OutcomeResultCollectionLink(
                            new List<Outcome>
                            {
                                new Outcome(1, "Outcome 1", "Outcome 1 EN Short Description NL Long Description"),
                                new Outcome(2, "Outcome 2", "Outcome 2 EN Short Description NL Long Description")
                            },
                            new List<Alignment> { }
                        )
                    )
                ),
                new ModuleOutcomeResultCollection(
                    new Module(3, "Module 3", 2, new List<ModuleItem>
                    {
                        new ModuleItem(4, "Module 3 Item 1", ModuleItemType.Assignment, 4),
                        new ModuleItem(5, "Module 3 Item 2", ModuleItemType.Assignment, 5)
                    }),
                    new OutcomeResultCollection(
                        new List<OutcomeResult> { },
                        new OutcomeResultCollectionLink(
                            new List<Outcome>
                            {
                                new Outcome(1, "Outcome 1", "Outcome 1 EN Short Description NL Long Description"),
                                new Outcome(2, "Outcome 2", "Outcome 2 EN Short Description NL Long Description")
                            },
                            new List<Alignment> { }
                        )
                    )
                )
            };

            outcomeServiceMock
                .Setup(s => s.GetResults(It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(new OutcomeResultCollection(
                    new List<OutcomeResult>
                    {
                        new OutcomeResult(false, 3, new OutcomeResultLink("user1", "1", "1", "2")),
                        new OutcomeResult(true, 4.5, new OutcomeResultLink("user2", "2", "2", "3")),
                        new OutcomeResult(false, null, new OutcomeResultLink("user1", "1", "1", "3")),
                    },
                    new OutcomeResultCollectionLink(
                        new List<Outcome>
                        {
                            new Outcome(1, "Outcome 1", "Outcome 1 EN Short Description NL Long Description"),
                            new Outcome(2, "Outcome 2", "Outcome 2 EN Short Description NL Long Description"),
                        },
                        new List<Alignment>
                        {
                            new Alignment("1", "Alignment 1", new Uri("https://alignment1.com")),
                            new Alignment("2", "Alignment 2", new Uri("https://alignment2.com")),
                            new Alignment("3", "Alignment 3", new Uri("https://alignment3.com")),
                        }
                    )
                ));

            moduleServiceMock
                .Setup(s => s.GetAll(It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<Module>
                {
                    new Module(1, "Module 1", 3, new List<ModuleItem>
                    {
                        new ModuleItem(1, "Module 1 Item 1", ModuleItemType.Page, 1),
                        new ModuleItem(2, "Module 1 Item 2", ModuleItemType.Assignment, 2),
                        new ModuleItem(3, "Module 1 Item 3", ModuleItemType.Quiz, 3)
                    }),
                    new Module(2, "Module 2", 0, new List<ModuleItem>()),
                    new Module(3, "Module 3", 2, new List<ModuleItem>
                    {
                        new ModuleItem(4, "Module 3 Item 1", ModuleItemType.Assignment, 4),
                        new ModuleItem(5, "Module 3 Item 2", ModuleItemType.Assignment, 5)
                    }),
                });

            var canvasModuleCollectionFetcher = new CanvasModuleCollectionFetcher(
                moduleServiceMock.Object, outcomeServiceMock.Object
            );

            // Act
            var result = new List<ModuleOutcomeResultCollection>();
            await foreach (var item in canvasModuleCollectionFetcher.GetAll(courseId, allowedModules))
            {
                result.Add(item);
            }

            // Assert
            Assert.Equal(JsonSerializer.Serialize(expectedResults), JsonSerializer.Serialize(result));
        }
    }
}