namespace Epsilon.Cli;

public record CanvasSettings
{
    public Uri? ApiUrl { get; set; }
    public int? CourseId { get; set; }
    public string? AccessToken { get; set; }
}