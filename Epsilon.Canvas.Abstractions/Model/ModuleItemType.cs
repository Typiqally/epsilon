using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModuleItemType
{
    File,
    Page,
    Discussion,
    Assignment,
    Quiz,
    SubHeader,
    ExternalUrl,
    ExternalTool,
}