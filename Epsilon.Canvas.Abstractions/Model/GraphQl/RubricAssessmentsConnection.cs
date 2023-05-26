﻿using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record RubricAssessmentsConnection(
    [property: JsonPropertyName("nodes")] IEnumerable<RubricAssessmentNode>? Nodes
);