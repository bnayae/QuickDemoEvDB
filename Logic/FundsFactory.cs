using EvDb.Core;
using System.Transactions;

namespace Logic;

[EvDbStreamFactory<IFundsEvents>( "users")]
public partial class FundsFactory
{

}
