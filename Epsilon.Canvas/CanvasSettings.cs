using System.ComponentModel.DataAnnotations;

namespace Epsilon.Canvas;

public record CanvasSettings
{
    [Required]
    public Uri ApiUrl { get; set; } = new Uri("https://fhict.instructure.com/api/");

    [Required]
    public int CourseId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string AccessToken { get; set; } = string.Empty;
}