using EvDb.Core;

namespace Logic;

/// <summary>
/// Funds fetch requested event denied via ATM
/// </summary>
[EvDbDefineEventPayload("deposited_v1")]
public readonly partial record struct DepositedEvent(string AccountId)
{   
    public required double Amount { get; init; }
}
