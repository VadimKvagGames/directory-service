public sealed record LocationId
{
    public Guid Value { get; }

    private LocationId(Guid value) => Value = value;

    public static LocationId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Идентификатор локации не может быть пустым.", nameof(value));

        return new LocationId(value);
    }
}