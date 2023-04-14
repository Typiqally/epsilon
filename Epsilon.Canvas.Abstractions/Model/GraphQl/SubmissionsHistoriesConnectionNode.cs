using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsHistoriesConnectionNode(
    [property: JsonPropertyName("attempt")] int? Attempt,
    [property: JsonPropertyName("rubricAssessmentsConnection")] RubricAssessmentsConnection? RubricAssessments
);