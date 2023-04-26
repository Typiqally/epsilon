using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record CanvasGraphQlSchema(
    [property: JsonPropertyName("allCourses")] IEnumerable<Course>? Courses,
    [property: JsonPropertyName("course")] Course? Course
);