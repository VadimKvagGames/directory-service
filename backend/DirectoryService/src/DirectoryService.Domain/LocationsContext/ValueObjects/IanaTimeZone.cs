public sealed record IanaTimeZone
{
    public string Value { get; }

    private IanaTimeZone(string value) => Value = value;

    public static IanaTimeZone Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("IANA временная зона не может быть пустой.", nameof(value));

        string[] parts = value.Split('/');
        if (parts.Length != 2 || parts.Any(p => string.IsNullOrWhiteSpace(p)))
            throw new ArgumentException("Некорректный формат IANA временной зоны.", nameof(value));

        return new IanaTimeZone(value);
    }
}