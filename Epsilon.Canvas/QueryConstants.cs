namespace Epsilon.Canvas;

public static class QueryConstants
{
    public const string GetUserCourseSubmissionOutcomes = @"
        query GetUserCourseSubmissionOutcomes {
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

    public const string GetAllUserCoursesSubmissionOutcomes = @"
        query GetAllUserCoursesSubmissionOutcomes {
            allCourses {
                name
                submissionsConnection(studentIds: $studentIds) {
                    nodes {
                        assignment {
                            name
                        }
                        postedAt
                        rubricAssessmentsConnection {
                            nodes {
                                assessmentRatings {
                                    points
                                    outcome {
                                        _id
                                        title
                                        masteryPoints
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