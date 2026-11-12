using DirectoryService.Domain.LocationsContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(id => id.Value, value => LocationId.Create(value))
            .HasColumnName("id");

        builder.Property(l => l.Name)
            .HasConversion(name => name.Value, value => LocationName.Create(value))
            .HasMaxLength(LocationName.MaxLength)
            .IsRequired()
            .HasColumnName("name");

        builder.Property(l => l.Address)
            .HasConversion(addr => addr.Value, value => LocationAddress.Create(value))
            .IsRequired()
            .HasColumnName("address");

        builder.Property(l => l.TimeZone)
            .HasConversion(tz => tz.Value, value => IanaTimeZone.Create(value))
            .IsRequired()
            .HasColumnName("time_zone");

        builder.OwnsOne(l => l.LifeTime, lt =>
        {
            lt.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
            lt.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();
            lt.Property(p => p.IsActive).HasColumnName("is_active").IsRequired();
        });
    }
}