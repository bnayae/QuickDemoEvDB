using EvDb.Core;
using Logic.Outbox;

namespace Logic;

[EvDbStreamFactory<IFundsEvents, MyOutbox>("users")]
public partial class FundsWithOutboxFactory
{

}



