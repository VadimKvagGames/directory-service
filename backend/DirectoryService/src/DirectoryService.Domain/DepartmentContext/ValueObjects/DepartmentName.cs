public sealed record DepartmentName
{
    public const int MaxLength = 200;
    public const int MinLength = 2;

    public string Value { get; }

    private DepartmentName(string value) => Value = value;

    public static DepartmentName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Название подразделения не может быть пустым.", nameof(value));

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException(
                $"Название подразделения должно быть от {MinLength} до {MaxLength} символов.",
                nameof(value));

        return new DepartmentName(value);
    }
}