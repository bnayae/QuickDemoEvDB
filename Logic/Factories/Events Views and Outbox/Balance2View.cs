using EvDb.Core;

namespace Logic;

[EvDbViewType<Balance, IFundsEvents>("balance_v1")]
public partial class Balance2View
{
    protected override Balance DefaultState { get; } = Balance.Empty;

    public override bool ShouldStoreSnapshot(
        long offsetGapFromLastSave,
        TimeSpan durationSinceLastSave)
    {
        return offsetGapFromLastSave > 3;
    }

    protected override Balance Apply(Balance state,
                                     DepositedEvent payload,
                                     IEvDbEventMeta meta)
    {
        return state with { Amount = state.Amount + payload.Amount };
    }

    protected override Balance Apply(Balance state, WithdrewEvent payload, IEvDbEventMeta meta)
    {
        return state with { Amount = state.Amount + payload.Value };
    }
}
