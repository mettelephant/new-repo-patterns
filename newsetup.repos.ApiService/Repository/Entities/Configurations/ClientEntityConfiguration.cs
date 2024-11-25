using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace newsetup.repos.ApiService.Repository.Entities.Configurations;

public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(e => e.ClientId);
        builder.Property(e => e.ClientId)
            .IsRequired();
        builder.Property(e => e.AccelerateId)
            .IsRequired();
        builder.Property(e => e.DatabaseName)
            .IsRequired();
        builder.Property(e => e.Server)
            .IsRequired();
        builder.Property(e => e.IsActive)
            .IsRequired();
    }
}