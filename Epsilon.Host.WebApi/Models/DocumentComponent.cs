namespace Epsilon.Host.WebApi.Models;

public abstract class DocumentComponent<TConfig, TData>
{
    public abstract int Id { get; set; }
    public abstract string Type { get; set; }
    public abstract string Title { get; set; }
    public abstract string Description { get; set; }
    public abstract TConfig Config { get; set; }
    public abstract TData Data { get; set; }
}