namespace Epsilon.Canvas;

public static class QueryConstants
{
    public const string GetUserSubmissionOutcomes = @"
        query GetUserSubmissionOutcomes {
            course(id: $courseId) {
                name
                submissionsConnection {
                    nodes {
                        assignment {
                            name
                            modules {
                                name
                            }
                        }
                        rubricAssessmentsConnection {
                            nodes {
                                assessmentRatings {
                                    points
                                    outcome {
                                        title
                                    }
                                }
                                user {
                                    name
                                }
                            }
                        }
                    }
                }
            }
        }
    ";
}