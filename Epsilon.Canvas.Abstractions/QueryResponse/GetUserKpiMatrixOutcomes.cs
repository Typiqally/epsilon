using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.Model.GraphQl;

namespace Epsilon.Canvas.Abstractions.QueryResponse;
public record GetUserKpiMatrixOutcomes(
    [property: JsonPropertyName("data")] GetUserKpiMatrixOutcomes.CourseData? Data
)
{
    public record CourseData(
        [property: JsonPropertyName("course")] Course Course
    );
};