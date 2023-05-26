using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record CanvasGraphQlQueryResponse(
    [property: JsonPropertyName("data")] CanvasGraphQlSchema? Data
);