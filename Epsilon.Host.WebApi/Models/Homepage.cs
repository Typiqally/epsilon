namespace Epsilon.Host.WebApi.Models;

public class HomePage : DocumentComponent<HomePageConfig, HomePageData>
{
    public override int Id { get; set; }
    public override string Type { get; set; }
    public override string Title { get; set; }
    public override string Description { get; set; }
    public override HomePageConfig Config { get; set; }
    public override HomePageData Data { get; set; }
}

public class HomePageConfig
{
    public string[] columns { get; set; }
    public string[] rows { get; set; }
}
    
public class HomePageData
{
    public string[] colors { get; set; }
}