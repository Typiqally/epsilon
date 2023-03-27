namespace Epsilon.Host.WebApi.Responses;

public class GetComponentResponse
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public object Config { get; set; }
    public object Data { get; set; }
}