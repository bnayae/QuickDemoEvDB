using EvDb.Core;

namespace Logic.Outbox;

[EvDbAttachMessageType<BalanceUpdatedMessage>]
[EvDbAttachMessageType<RiskDetectedMessage>]
[EvDbOutbox<FundsWithOutboxWithViewsFactory>]
internal partial class MyOutboxWithViews
{
    private readonly List<(DateTimeOffset Date, double Amount)> resentWithdraws = new();

    protected override void ProduceOutboxMessages(DepositedEvent payload,
                                                  IEvDbEventMeta meta,
                                                  EvDbFundsWithOutboxWithViewsViews views,
                                                  MyOutboxWithViewsContext outbox)
    {
        var balanceMessage = new BalanceUpdatedMessage
        {
            Amount = payload.Amount,
            Balance = views.Balance.Amount
        };
        outbox.Append(balanceMessage);
    }

    protected override void ProduceOutboxMessages(WithdrewEvent payload,
                                                  IEvDbEventMeta meta,
                                                  EvDbFundsWithOutboxWithViewsViews views,
                                                  MyOutboxWithViewsContext outbox)
    {
        var balanceMessage = new BalanceUpdatedMessage
        {
            Amount = -payload.Value,
            Balance = views.Balance.Amount
        };
        outbox.Append(balanceMessage);

        resentWithdraws.Add((DateTimeOffset.UtcNow, payload.Value));
        if (resentWithdraws.Count > 5)
        {
            resentWithdraws.RemoveAt(0);
            if (views.Balance.Amount < 1000 &&
                resentWithdraws.Sum(m => m.Amount) > 200 &&
                resentWithdraws[4].Date - resentWithdraws[0].Date < TimeSpan.FromSeconds(10))
            { 
                RiskDetectedMessage riskMessage = new() { Reason = "?" };
                outbox.Append(riskMessage);
            }
        }
    }

}
