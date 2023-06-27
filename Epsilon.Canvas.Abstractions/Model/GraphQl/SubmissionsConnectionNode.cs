using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsConnectionNode(
    [property: JsonPropertyName("submittedAt")] DateTime? SubmittedAt,
    [property: JsonPropertyName("postedAt")] DateTime? PostedAt,
    [property: JsonPropertyName("assignment")] Assignment? Assignment,
    [property: JsonPropertyName("user")] User? User,
    [property: JsonPropertyName("submissionHistoriesConnection")] SubmissionsHistoriesConnection SubmissionsHistories,
    [property: JsonPropertyName("rubricAssessmentsConnection")] RubricAssessmentsConnection RubricAssessmentsConnection
);