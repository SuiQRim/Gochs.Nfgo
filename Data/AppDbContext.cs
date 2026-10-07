using Gochs.Nfgo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Formation> Formations => Set<Formation>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
