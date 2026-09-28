using DirectoryService.Domain.Shared;

public sealed record PositionName
{
    public const int MaxLength = 150;
    public const int MinLength = 2;

    public string Value { get; }

    private PositionName(string value) => Value = value;

    public static PositionName Create(string value)
    {
        string formatted = value.TrimSpaces().FirstCharacterToUpper();

        if (string.IsNullOrWhiteSpace(formatted))
            throw new ArgumentException("Название должности не может быть пустым.", nameof(formatted));

        if (formatted.Length < MinLength || formatted.Length > MaxLength)
            throw new ArgumentException(
                $"Название должности должно быть от {MinLength} до {MaxLength} символов.",
                nameof(formatted));

        return new PositionName(formatted);
    }
}