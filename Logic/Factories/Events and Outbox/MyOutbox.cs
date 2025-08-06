using EvDb.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Outbox;


[EvDbAttachMessageType<RiskDetectedMessage>]
[EvDbOutbox<FundsWithOutboxFactory>]
internal partial class MyOutbox
{
}
