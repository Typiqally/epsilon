using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.Model.GraphQl;

namespace Epsilon.Canvas.Abstractions.QueryResponse;

public record GetUserSubmissionOutcomes(
    [property: JsonPropertyName("data")] GetUserSubmissionOutcomes.CourseData? Data
)
{
    public record CourseData(
        [property: JsonPropertyName("course")] Course? Course
    );
};