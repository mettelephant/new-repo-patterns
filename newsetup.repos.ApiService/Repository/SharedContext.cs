using newsetup.repos.ApiService.Repository.Converters;
using newsetup.repos.ApiService.Repository.Entities;
using newsetup.repos.ApiService.Repository.ValueObjects;

namespace newsetup.repos.ApiService.Repository;

using Microsoft.EntityFrameworkCore;

public class SharedContext : DbContext
{
    public virtual DbSet<ClientEntity> Clients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SharedContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<AccelerateId>()
            .HaveConversion<AccelerateIdConverter>();
    }
}
