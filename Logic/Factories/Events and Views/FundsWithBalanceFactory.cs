using EvDb.Core;

namespace Logic;

[EvDbAttachView<Balance1View>("Balance")]
[EvDbStreamFactory<IFundsEvents>("users")]
public partial class FundsWithBalanceFactory
{

}
