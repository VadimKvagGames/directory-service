using DirectoryService.Domain.DepartmentContext.ValueObjects;

public class Department
{
    private readonly List<LocationId> _locations = [];

    public DepartmentId Id { get; }
    public DepartmentName Name { get; }
    public DepartmentSlug Slug { get; }
    public DepartmentId? ParentId { get; }
    public IReadOnlyList<LocationId> Locations => _locations.AsReadOnly();
    public EntityLifeTime LifeTime { get; }
    public DepartmentPath Path { get; }
    public Department(
        DepartmentId id,
        DepartmentName name,
        DepartmentSlug slug,
        DepartmentId? parentId,
        IEnumerable<LocationId> locations,
        EntityLifeTime lifeTime
        )
    {
        Path = DepartmentPath.Create(slug);
        Id = id;
        Name = name;
        Slug = slug;
        ParentId = parentId;
        _locations = [.. locations];
        LifeTime = lifeTime;
    }

    public Department(
        DepartmentId id,
        DepartmentName name,
        DepartmentSlug slug,
        DepartmentId? parentId,
        IEnumerable<LocationId> locations,
        EntityLifeTime lifeTime,
        DepartmentPath path
        )
    {
        Path = path;
        Id = id;
        Name = name;
        Slug = slug;
        ParentId = parentId;
        _locations = [.. locations];
        LifeTime = lifeTime;
    }
}