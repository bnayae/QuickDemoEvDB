using EvDb.Core;

namespace Logic;


/// <summary>
/// Funds fetch requested event denied via ATM
/// </summary>
[EvDbDefineEventPayload("withdrewn_v1")]
public readonly partial record struct WithdrewEvent(string AccountId)
{
    public required double Value { get; init; }
}
