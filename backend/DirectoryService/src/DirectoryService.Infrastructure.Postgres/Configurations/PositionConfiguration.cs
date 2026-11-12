using DirectoryService.Domain.PositionContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => PositionId.Create(value))
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .HasConversion(name => name.Value, value => PositionName.Create(value))
            .IsRequired()
            .HasColumnName("name");

        builder.Property(p => p.Description)
            .HasConversion(desc => desc.Value, value => PositionDescription.Create(value))
            .IsRequired()
            .HasColumnName("description");

        builder.OwnsOne(p => p.LifeTime, lt =>
        {
            lt.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            lt.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
            lt.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        });
    }
}