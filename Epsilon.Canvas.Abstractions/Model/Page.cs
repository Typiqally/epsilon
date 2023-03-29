using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record Page(
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("created_at")] string? CreatedAt,
    [property: JsonPropertyName("url")] string? Url,
    [property: JsonPropertyName("editing_roles")] string? EditingRoles,
    [property: JsonPropertyName("page_id")] int? PageId,
    [property: JsonPropertyName("published")] bool? Published,
    [property: JsonPropertyName("hide_from_students")] bool? HideFromStudents,
    [property: JsonPropertyName("front_page")] bool? FrontPage,
    [property: JsonPropertyName("html_url")] string? HTMLUrl,
    [property: JsonPropertyName("todo_date")] DateTime? TodoDate,
    [property: JsonPropertyName("publish_at")] DateTime? PublishAt,
    [property: JsonPropertyName("updated_at")] DateTime? UpdatedAt,
    [property: JsonPropertyName("locked_for_user")] bool? LockedForUser,
    [property: JsonPropertyName("body")] string? Body
);