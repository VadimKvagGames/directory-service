using DirectoryService.Domain.Shared;

public sealed record DepartmentName
{
    public const int MaxLength = 200;
    public const int MinLength = 2;

    public string Value { get; }

    private DepartmentName(string value) => Value = value;

    public static DepartmentName Create(string value)
    {
        string formatted = value
            .TrimSpaces()
            .FirstCharacterToUpper();

        if (string.IsNullOrWhiteSpace(formatted))
            throw new ArgumentException("Название подразделения не может быть пустым.", nameof(formatted));

        if (formatted.Length < MinLength || formatted.Length > MaxLength)
            throw new ArgumentException(
                $"Название подразделения должно быть от {MinLength} до {MaxLength} символов.",
                nameof(formatted));

        return new DepartmentName(formatted);
    }
}