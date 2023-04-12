using System.Text.Json.Serialization;

namespace NetCore.Lti;

public record LaunchPresentation(
    [property: JsonPropertyName("document_target")] string DocumentTarget,
    [property: JsonPropertyName("width")] int Width,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("return_url")] string ReturnUrl
);