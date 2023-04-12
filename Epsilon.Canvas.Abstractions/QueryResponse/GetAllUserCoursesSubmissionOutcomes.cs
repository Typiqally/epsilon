using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.Model.GraphQL;

namespace Epsilon.Canvas.Abstractions.QueryResponse;

public record GetAllUserCoursesSubmissionOutcomes(
    [property: JsonPropertyName("data")] GetAllUserCoursesSubmissionOutcomes.CourseData? Data
)
{
    public record CourseData(
        [property: JsonPropertyName("allCourses")] List<Course>? Courses
    );
};