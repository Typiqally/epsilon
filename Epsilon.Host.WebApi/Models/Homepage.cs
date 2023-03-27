namespace Epsilon.Host.WebApi.Models;

public class HomePage : IDocumentComponent<HomePageData>
{
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IDictionary<string, string> Config { get; set; }
    public HomePageData Data { get; set; }
}

public class HomePageData
{
    public string[] colors { get; set; }
}