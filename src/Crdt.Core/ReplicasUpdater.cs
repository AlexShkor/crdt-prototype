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

        public ReplicasUpdater()
        {
            _sender = new RabbitMqSender();
            _consumer = new RabbitMqConsumer();
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
