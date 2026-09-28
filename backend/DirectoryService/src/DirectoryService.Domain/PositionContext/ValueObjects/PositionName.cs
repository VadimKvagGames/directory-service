public sealed record PositionName
{
    public const int MaxLength = 150;
    public const int MinLength = 2;

    public string Value { get; }

    private PositionName(string value) => Value = value;

    public static PositionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Название должности не может быть пустым.", nameof(value));

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException(
                $"Название должности должно быть от {MinLength} до {MaxLength} символов.",
                nameof(value));

        return new PositionName(value);
    }
}