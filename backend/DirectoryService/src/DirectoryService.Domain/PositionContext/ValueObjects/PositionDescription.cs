public sealed record PositionDescription
{
    public const int MaxLength = 1000;

    public string Value { get; }

    private PositionDescription(string value) => Value = value;

    public static PositionDescription Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new PositionDescription(string.Empty);

        if (value.Length > MaxLength)
            throw new ArgumentException($"Описание не может превышать {MaxLength} символов.", nameof(value));

        return new PositionDescription(value);
    }
}