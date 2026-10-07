using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
