﻿using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record RubricAssessmentNode(
    [property: JsonPropertyName("assessmentRatings")] List<AssessmentRating>? AssessmentRatings, 
    [property: JsonPropertyName("user")] User? User
);