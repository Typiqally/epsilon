﻿namespace Epsilon.Abstractions.Component;

public interface IComponentFetcher
{
    public string ComponentName { get; }
    
    public Task<IEpsilonComponent> FetchUnknown();
}

public interface IComponentFetcher<TComponent> : IComponentFetcher
    where TComponent : IEpsilonComponent
{
    public Task<TComponent> Fetch();
}