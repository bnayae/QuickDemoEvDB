using EvDb.Core;

namespace Logic;

[EvDbDefineMessagePayload("balance-updated-v1")]
public readonly partial record struct BalanceUpdatedMessage
{
    public double Amount { get; init; }
    public double Balance { get; init; }
}
