using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LegalZoomMVP.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // Use connection string from appsettings.json
        var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=4pl!ffG@nja";
        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
