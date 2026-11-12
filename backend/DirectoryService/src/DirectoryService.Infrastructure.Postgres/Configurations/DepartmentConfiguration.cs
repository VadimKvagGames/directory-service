using System.Text.Json;
using DirectoryService.Domain.DepartmentContext;
using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.PositionContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(id => id.Value, value => DepartmentId.Create(value))
            .HasColumnName("id");

        builder.Property(d => d.Name)
            .HasConversion(name => name.Value, value => DepartmentName.Create(value))
            .IsRequired()
            .HasColumnName("name");

        builder.Property(d => d.Slug)
            .HasConversion(slug => slug.Value, value => DepartmentSlug.Create(value))
            .IsRequired()
            .HasColumnName("slug");

        builder.Property(d => d.ParentId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? DepartmentId.Create(value.Value) : null)
            .HasColumnName("parent_id");

        builder.Property(d => d.Path)
            .HasConversion(path => path.Value, value => DepartmentPath.FromDatabase(value))
            .IsRequired()
            .HasColumnName("path");

        builder.OwnsOne(d => d.LifeTime, lt =>
        {
            lt.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            lt.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
            lt.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        });

        builder.Ignore(d => d.Locations);
        builder.Ignore(d => d.Positions);
        builder.Ignore(d => d.PositionRanks);

        builder.Property<List<LocationId>>("_locations")
            .HasColumnName("location_ids")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(x => x.Value), (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null)!
                    .Select(LocationId.Create).ToList()
            );

        builder.Property<List<PositionId>>("_positions")
            .HasColumnName("position_ids")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(x => x.Value), (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null)!
                    .Select(PositionId.Create).ToList()
            );

        builder.Property<List<DepartmentPositionRank>>("_positionRanks")
            .HasColumnName("position_ranks")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(x => new { PositionId = x.PositionId.Value, x.Rank }), (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<RankDto>>(v, (JsonSerializerOptions?)null)!
                    .Select(x => new DepartmentPositionRank(PositionId.Create(x.PositionId), x.Rank)).ToList()
            );
    }

    private sealed record RankDto(Guid PositionId, int Rank);
}