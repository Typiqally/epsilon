using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Lti.Data;
using NetCore.Lti.EntityFrameworkCore.Repositories;

namespace NetCore.Lti.EntityFrameworkCore;

public static class EntityFrameworkCoreLtiBuilderExtensions
{
    public static LtiBuilder AddEntityFrameworkRepositories<TContext>(this LtiBuilder builder)
        where TContext : DbContext
    {
        builder.Services.AddScoped<IToolPlatformRepository, ToolPlatformEntityFrameworkRepository<TContext>>();

        return builder;
    }
}