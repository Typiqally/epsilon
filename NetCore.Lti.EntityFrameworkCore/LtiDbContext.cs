using Microsoft.EntityFrameworkCore;

namespace NetCore.Lti.EntityFrameworkCore;

public class LtiDbContext : DbContext
{
    public LtiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ToolPlatform> ToolPlatforms { get; set; }
}