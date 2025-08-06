using EvDb.Core;

namespace Logic;

[EvDbDefineMessagePayload("risk-detected-v1")]
public readonly partial record struct RiskDetectedMessage
{
    public string Reason { get; init; }
}
