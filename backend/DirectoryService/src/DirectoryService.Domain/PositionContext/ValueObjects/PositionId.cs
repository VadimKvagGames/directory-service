public sealed record PositionId
{
    public Guid Value { get; }

    private PositionId(Guid value) => Value = value;

    public static PositionId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Идентификатор должности не может быть пустым.", nameof(value));

        return new PositionId(value);
    }
}