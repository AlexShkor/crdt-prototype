using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public interface IReplicasUpdater
    {
        void ListenForUpdate(Action<UpdateDocumentCommand> callback);
        void SendUpdate(UpdateDocumentCommand update);
    }
}
