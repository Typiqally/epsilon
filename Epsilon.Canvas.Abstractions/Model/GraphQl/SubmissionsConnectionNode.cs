using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsConnectionNode(
    [property: JsonPropertyName("postedAt")] DateTime? PostedAt,
    [property: JsonPropertyName("submissionHistoriesConnection")] SubmissionsHistoriesConnection SubmissionsHistories
);