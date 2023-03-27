namespace Epsilon.Host.WebApi.Interfaces;

public interface IComponent
{
    int Id { get; set; }
    string Type { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    IDictionary<string, string> Config { get; set; }
}

public interface IComponent<TData>  : IComponent
{
    TData Data  { get; set; }
}