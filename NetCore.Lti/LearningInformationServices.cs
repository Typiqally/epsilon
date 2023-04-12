using System.Text.Json.Serialization;

namespace NetCore.Lti;

public record LearningInformationServices
{
    [JsonPropertyName("person_sourcedid")]
    public string? PersonSourcedId { get; init; }
    
    [JsonPropertyName("course_offering_sourcedid")]
    public string? CourseOfferingSourcedId { get; init; }
    
    [JsonPropertyName("course_section_sourcedid")]
    public string? CourseSectionSourcedId { get; init; }
}