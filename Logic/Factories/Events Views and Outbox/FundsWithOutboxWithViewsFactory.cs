using EvDb.Core;
using Logic.Outbox;

namespace Logic;

[EvDbAttachView<Balance2View>("Balance")]
[EvDbStreamFactory<IFundsEvents, MyOutboxWithViews>("users")]
public partial class FundsWithOutboxWithViewsFactory
{

}



