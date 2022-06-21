namespace Epsilon.Canvas.Abstractions.Model;

public record LinkHeader
{
    public string? FirstLink { get; set; }
    public string? PrevLink { get; set; }
    public string? NextLink { get; set; }
    public string? LastLink { get; set; }
}