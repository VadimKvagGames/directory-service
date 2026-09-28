public sealed record CoinsWallet
{
    public int Amount { get; }

    private CoinsWallet(int amount) => Amount = amount;

    public static CoinsWallet Create(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

        return new CoinsWallet(amount);
    }
}