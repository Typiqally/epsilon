using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.QueryResponse;

public record GetUserSubmissionOutcomes(
    [property: JsonPropertyName("data")] CourseData? Data
);





