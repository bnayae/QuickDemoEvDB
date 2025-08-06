using EvDb.Core;

namespace Logic;

[EvDbAttachEventType<WithdrewEvent>]
[EvDbAttachEventType<DepositedEvent>]
public partial interface IFundsEvents 
{ }