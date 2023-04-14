using System.Collections;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record EnrollmentTermCollection(
    [property: JsonPropertyName("enrollment_terms")] IEnumerable<EnrollmentTerm> Terms
);