namespace Epsilon.Host.WebApi.Models;

public interface IDocumentComponent<TData> 
{ 
    string Type { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    IDictionary<string, string> Config { get; set; }
    TData Data  { get; set; }
}