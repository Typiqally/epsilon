using System.Text.Json.Serialization;

namespace NetCore.Lti.DeepLinking;

public record DeepLinkingSettings(
    [property: JsonPropertyName("deep_link_return_url")] string DeepLinkReturnUrl,
    [property: JsonPropertyName("accept_types")] IEnumerable<string> AcceptTypes,
    [property: JsonPropertyName("accept_presentation_document_targets")] IEnumerable<string> AcceptPresentationDocumentTargets,
    [property: JsonPropertyName("accept_media_types")] string AcceptMediaTypes,
    [property: JsonPropertyName("auto_create")] bool AutoCreate,
    [property: JsonPropertyName("accept_multiple")] bool AcceptMultiple
);