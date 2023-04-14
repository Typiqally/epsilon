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
        query MyQuery {
          allCourses {
            submissionsConnection(studentIds: $studentIds) {
              nodes {
                rubricAssessmentsConnection {
                  nodes {
                    assessmentRatings {
                      criterion {
                        masteryPoints
                        outcome {
                          _id
                        }
                      }
                      points
                    }
                  }
                }
                postedAt
              }
            }
          }
        }
    ";
}