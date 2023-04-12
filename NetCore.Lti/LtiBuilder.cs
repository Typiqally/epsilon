using Microsoft.Extensions.DependencyInjection;

namespace NetCore.Lti;

public class LtiBuilder
{
    public LtiBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}