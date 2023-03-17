using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record Page(
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("created_at")] string? CreatedAt,
    [property: JsonPropertyName("url")] string? Url,
    [property: JsonPropertyName("editing_roles")] string? EditingRoles,
    [property: JsonPropertyName("page_id")] string? PageId,
    [property: JsonPropertyName("last_edited_by")] string? LastEditedBy,
    [property: JsonPropertyName("published")] string? Published,
    [property: JsonPropertyName("hide_from_students")] string? HideFromStudents,
    [property: JsonPropertyName("front_page")] string? FrontPage,
    [property: JsonPropertyName("html_url")] string? HTMLUrl,
    [property: JsonPropertyName("todo_date")] string? TodoDate,
    [property: JsonPropertyName("publish_at")] string? PublishAt,
    [property: JsonPropertyName("updated_at")] string? UpdatedAt,
    [property: JsonPropertyName("locked_for_user")] string? LockedForUser,
    [property: JsonPropertyName("body")] string? Body
);