using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record RubricAssessmentNode(
    [property: JsonPropertyName("assessmentRatings")] List<AssessmentRating>? AssessmentRatings, 
    [property: JsonPropertyName("user")] User? User
);