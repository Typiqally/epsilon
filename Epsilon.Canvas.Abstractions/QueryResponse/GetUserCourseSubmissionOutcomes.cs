using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.Model.GraphQL;

namespace Epsilon.Canvas.Abstractions.QueryResponse;

public record GetUserCourseSubmissionOutcomes(
    [property: JsonPropertyName("data")] GetUserCourseSubmissionOutcomes.CourseData? Data
)
{
    public record CourseData(
        [property: JsonPropertyName("course")] Course? Course
    );
};