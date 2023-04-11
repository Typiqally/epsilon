using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record Node(
    [property: JsonPropertyName("assignment")] Assignment? Assignment, 
    [property: JsonPropertyName("rubricAssessmentsConnection")] RubricAssessmentsConnection? RubricAssessmentsConnection
);