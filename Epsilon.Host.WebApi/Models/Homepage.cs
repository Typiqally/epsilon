using Epsilon.Host.WebApi.Interfaces;

namespace Epsilon.Host.WebApi.Models;

public record HomePage : IComponent<HomePageData>
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IDictionary<string, string> Config { get; set; }
    public HomePageData Data { get; set; }
}

public record HomePageData
{
    public string[] SomeHomePageDataArray { get; set; }
}