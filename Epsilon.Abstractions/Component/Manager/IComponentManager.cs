namespace Epsilon.Abstractions.Component.Manager;

public interface IComponentManager<in TParam, TResponse>
{
    public Task<TResponse> GetComponent(TParam param);
}