using DirectoryService.Domain.DepartmentContext;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.PositionContext;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Postgres;

public class DirectoryDbContext : DbContext
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Department> Departments => Set<Department>();

    public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryDbContext).Assembly);
    }
}