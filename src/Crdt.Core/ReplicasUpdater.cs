using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crdt.Core.Messaging;

namespace Crdt.Core
{
    public class ReplicasUpdater : IReplicasUpdater
    {
        private readonly IReplicaOperationsConsumer _consumer;
        private readonly IReplicaOperationsSender _sender;

        public ReplicasUpdater(IReplicaOperationsConsumer consumer, IReplicaOperationsSender sender)
        {
            _consumer = consumer;
            _sender = sender;
        }

        public void ListenForUpdate(Action<UpdateDocumentCommand> callback)
        {
            _consumer.ListenToSibilings(callback);
        }

        public void SendUpdate(UpdateDocumentCommand update)
        {
            _sender.SendToSibilings(update);
        }
    }
}
