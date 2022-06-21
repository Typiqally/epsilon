﻿using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record Submission(
    [property: JsonPropertyName("submitted_at")] DateTime? SubmittedAt,
    [property: JsonPropertyName("graded_at")] DateTime? GradedAt,
    [property: JsonPropertyName("full_rubric_assessment")] RubricAssessment? RubricAssessment,
    [property: JsonPropertyName("assignment")] Assignment? Assignment
);