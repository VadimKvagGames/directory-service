namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public sealed record DepartmentPath
{
    public const char SEPARATOR = '.';
    public string Value { get; }

    private DepartmentPath(string value)
    {
        Value = value;
    }

    public static DepartmentPath Create(DepartmentSlug slug)
    {
        return new DepartmentPath(slug.Value.ToLower());
    }

    public static DepartmentPath CreateForChild(DepartmentPath parentPath, DepartmentSlug childIdentifier)
    {
        string joinedPath = string.Join(SEPARATOR, parentPath.Value, childIdentifier.Value);
        return new DepartmentPath(joinedPath);
    }
}