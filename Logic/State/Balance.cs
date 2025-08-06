namespace Logic;

public readonly record struct Balance
{
    public static readonly Balance Empty = new Balance
    {
        Amount = 0.0,
        Comment = null
    };

    public required double Amount { get; init; }
    public string? Comment { get; init; }
}
