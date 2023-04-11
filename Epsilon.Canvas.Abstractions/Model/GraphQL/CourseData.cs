using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record CourseData(
    [property: JsonPropertyName("course")] Course? Course
);