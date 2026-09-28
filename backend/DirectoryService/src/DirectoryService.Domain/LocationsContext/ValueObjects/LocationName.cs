using DirectoryService.Domain.Shared;

public sealed record LocationName
{
    public const int MaxLength = 128;
    public const int MinLength = 3;

    public string Value { get; } 

    private LocationName(string value)
    {
        Value = value;
    }

    public static LocationName Create(string value)
    {
        string formatted = value.TrimSpaces().FirstCharacterToUpper();

        if (string.IsNullOrWhiteSpace(formatted))
            throw new ArgumentException("Название локации не может быть пустым.", nameof(formatted));

        if (formatted.Length > MaxLength)
            throw new ArgumentException(
                $"Название локации не может превышать {MaxLength} символов.",
                nameof(formatted)
            );

        if (formatted.Length < MinLength)
            throw new ArgumentException(
                $"Название локации должно быть от {MinLength} до {MaxLength} символов.",
                nameof(formatted)
            );

        return new LocationName(formatted);
    }
}