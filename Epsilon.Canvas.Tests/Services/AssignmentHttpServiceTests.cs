using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Service;
using Moq;
using Xunit;

namespace Epsilon.Canvas.Tests.Services;

public class AssignmentHttpServiceTests
{
    [Fact]
    public async Task GivenCourseIdAndIncludeSubmissionAndRubricAssessment_WhenCanvasAssignmentsAreRetrieved_ThenAssignmentsAreReturned()
    {
        // Arrange
        const int courseId = 123;
        var include = new[]
        {
            "submission",
            "rubric_assessment",
        };
        var assignments = new[]
        {
            new Assignment(1, "Assignment 1", new Uri("https://example.com/1"), null),
            new Assignment(2, "Assignment 2", new Uri("https://example.com/2"), new Submission(null, null, null, null)),
            new Assignment(3, "Assignment 3", new Uri("https://example.com/3"), new Submission(null, null, new RubricAssessment(8.5, 1, Enumerable.Empty<RubricRating>()), null)
            ),
        };

        var paginatorHttpServiceMock = new Mock<IPaginatorHttpService>();
        paginatorHttpServiceMock.Setup(static x => x.GetAllPages<IEnumerable<Assignment>>(
                HttpMethod.Get,
                new Uri("v1/courses/{courseId}/assignments?include[]=submission&include[]=rubric_assessment")))
            .ReturnsAsync(new[]
            {
                assignments,
            });

        using var httpClientMock = new HttpClient();
        var assignmentHttpService = new AssignmentHttpService(httpClientMock, paginatorHttpServiceMock.Object);

        // Act
        var result = await assignmentHttpService.GetAll(courseId, include);

        // Assert
        Assert.Equal(assignments, result);
    }
}