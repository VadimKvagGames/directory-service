public class Location
{
    public Guid Id { get; }
    public string Address { get; }
    public string Name { get; }
    public string TimeZone { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
}

private Location(
    Guid id,
    string address,
    string name,
    string timeZone,
    DateTime createdAt,
    DateTime updatedAt
)
{
    Id = id;
    Address = address;
    Name = name;
    TimeZone = timeZone;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
}

public static Location Create(
    Guid id,
    string address,
    string name,
    string timeZone,
    DateTime createdAt,
    DateTime updatedAt
)
{
    if (id == Guid.Empty)
        throw new ArgumentException("Идентификатор не может быть пустым.", nameof(id));

    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Название локации не может быть пустым.", nameof(name));

    if (string.IsNullOrWhiteSpace(address))
        throw new ArgumentException("Адрес локации не может быть пустым.", nameof(address));

    if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
        throw new ArgumentException("Некорректное значение даты создания.", nameof(createdAt));

    if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
        throw new ArgumentException(
            "Некорректное значение даты обновления.",
            nameof(updatedAt)
        );

    ValidateIanaTimeZone(timeZone);

    
    return new Location(id, address, name, timeZone, createdAt, updatedAt);
}

private static void ValidateIanaTimeZone(string input)
{
    if (string.IsNullOrWhiteSpace(input))
        throw new ArgumentException("IANA временная зона не может быть пустой.", nameof(input));

    if (!input.Contains('/'))
        throw new ArgumentException("Некорректный формат IANA временной зоны.", nameof(input));

    string[] parts = input.Split('/');
    if (parts.Length != 2)
        throw new ArgumentException("Некорректный формат IANA временной зоны.", nameof(input));

    if (parts.Any(p => string.IsNullOrWhiteSpace(p)))
        throw new ArgumentException("Некорректный формат IANA временной зоны.", nameof(input));

}

public class Location
{
    public LocationId Id { get; }
    public LocationName Name { get; }
    public LocationAddress Address { get; }
    public EntityLifeTime LifeTime { get; }
    public IanaTimeZone TimeZone { get; }

    public Location(
        LocationId id,
        LocationAddress address,
        LocationName name,
        IanaTimeZone timeZone,
        EntityLifeTime lifeTime
    )
    {
        Id = id;
        Address = address;
        Name = name;
        TimeZone = timeZone;
        LifeTime = lifeTime;
    }
}