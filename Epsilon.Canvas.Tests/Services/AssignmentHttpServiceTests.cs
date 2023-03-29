using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Epsilon.Abstractions.Http;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Moq;
using Moq.Protected;
using Xunit;

namespace Epsilon.Canvas.Service.Tests
{
    public class AssignmentHttpServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<IPaginatorHttpService> _paginatorHttpServiceMock;
        private readonly AssignmentHttpService _assignmentHttpService;

        public AssignmentHttpServiceTests()
        {
            _httpClient = new HttpClient();
            _paginatorHttpServiceMock = new Mock<IPaginatorHttpService>();
            _assignmentHttpService = new AssignmentHttpService(_httpClient, _paginatorHttpServiceMock.Object);
        }

        [Fact]
        public async Task
            GivenCourseIdAndIncludeSubmissionAndRubricAssessment_WhenCanvasAssignmentsAreRetrieved_ThenAssignmentsAreReturned()
        {
            // Arrange
            var courseId = 123;
            var include = new[] {"submission", "rubric_assessment"};
            var assignments = new[]
            {
                new Assignment(1, "Assignment 1", new("https://example.com/1"), null),
                new Assignment(2, "Assignment 2", new("https://example.com/2"), new Submission(null, null, null, null)),
                new Assignment(3, "Assignment 3", new("https://example.com/3"),
                    new Submission(null, null, new RubricAssessment(8.5, 1, Enumerable.Empty<RubricRating>()), null)),
            };
            _paginatorHttpServiceMock.Setup(x => x.GetAllPages<IEnumerable<Assignment>>(HttpMethod.Get,
                    $"v1/courses/{courseId}/assignments?include[]=submission&include[]=rubric_assessment"))
                .ReturnsAsync(new[] {assignments});

            // Act
            var result = await _assignmentHttpService.GetAll(courseId, include);

            // Assert
            Assert.Equal(assignments, result);
        }
    }
}