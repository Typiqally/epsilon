using Microsoft.EntityFrameworkCore;
using NetCore.Lti.Data;

namespace NetCore.Lti.EntityFrameworkCore.Repositories;

public class ToolPlatformEntityFrameworkRepository<TContext> : EntityFrameworkRepository<TContext, ToolPlatform>, IToolPlatformRepository
    where TContext : DbContext
{
    public ToolPlatformEntityFrameworkRepository(TContext context) : base(context)
    {
    }
}