using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.QueryResponse;

public record GetUserSubmissionOutcomes(
    [property: JsonPropertyName("data")] CourseData? Data
);

public record CourseData(
    [property: JsonPropertyName("course")] Course? Course
);

public record Course(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("submissionsConnection")] SubmissionsConnection? SubmissionsConnection
);

public record SubmissionsConnection(
    [property: JsonPropertyName("nodes")] List<Node>? Nodes
);

public record Node(
    [property: JsonPropertyName("assignment")] Assignment? Assignment, 
    [property: JsonPropertyName("rubricAssessmentsConnection")] RubricAssessmentsConnection? RubricAssessmentsConnection
);

public record Assignment(
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("modules")] List<Module>? Modules 
);

public record Module(
    [property: JsonPropertyName("name")] string Name
);

public record RubricAssessmentsConnection(
    [property: JsonPropertyName("nodes")] List<RubricAssessmentNode>? Nodes
);

public record RubricAssessmentNode(
    [property: JsonPropertyName("assessmentRatings")] List<AssessmentRating>? AssessmentRatings, 
    [property: JsonPropertyName("user")] User? User
);

public record AssessmentRating(
    [property: JsonPropertyName("points")] double? Points,
    [property: JsonPropertyName("outcome")] Outcome? Outcome
);

public record Outcome(
    [property: JsonPropertyName("title")] string Title
);

public record User(
    [property: JsonPropertyName("name")] string Name
);