using System.Text.Json.Serialization;

namespace NetCore.Lti;

public record ToolPlatformReference(
    [property: JsonPropertyName("guid")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("product_family_code")] string ProductFamilyCode
);