namespace Epsilon.Canvas;

public record CanvasSettings
{
    public Uri ApiUrl { get; set; } = new("https://fhict.instructure.com/api/");
    public int CourseId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
}