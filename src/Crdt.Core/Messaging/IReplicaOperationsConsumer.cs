using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crdt.Core.Messaging
{
    public interface IReplicaOperationsConsumer
    {
        void ListenToSibilings(Action<UpdateDocumentCommand> callback);
    }
}
