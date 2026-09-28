public sealed record DepartmentSlug
{
    public const int MaxLength = 100;

    public string Value { get; }

    private DepartmentSlug(string value) => Value = value;

    public static DepartmentSlug Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Slug подразделения не может быть пустым.", nameof(value));

        if (value.Length > MaxLength)
            throw new ArgumentException($"Slug не может превышать {MaxLength} символов.", nameof(value));

        if (!value.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException("Slug может содержать только буквы, цифры и дефис.", nameof(value));

        return new DepartmentSlug(value);
    }
}