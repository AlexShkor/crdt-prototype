using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crdt.Core.Messaging.Utils;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Crdt.Core.Messaging
{
    public class RabbitMqSender : IReplicaOperationsSender
    {
        private readonly Dictionary<string, IModel> _siblings = new Dictionary<string, IModel>();
        private const string SendingQueuesEnvVarName = "SIBILINGS_QUEUES";

        public RabbitMqSender()
        {
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://guest:guest@127.0.0.1:5672/";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var sibilings = Environment.GetEnvironmentVariable(SendingQueuesEnvVarName);

            if (sibilings == null) throw new Exception("Sibiling replicated dbs are undefined");
            
            foreach (var sibiling in sibilings.Split(';'))
            {
                var exchangeName = sibiling;
                var queueName = sibiling;
                string routingKey = sibiling;
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.QueueBind(queueName, exchangeName, routingKey, null);
                _siblings[sibiling] = channel;
            }
        }


        public void SendToSibilings(UpdateDocumentCommand operation)
        {
            var jsonOperation = Serializer.SerializeToString(operation);

            foreach (var replica in _siblings)
            {
                var channel = replica.Value;
                var sibiling = replica.Key;

                byte[] operationBodyBytes = System.Text.Encoding.UTF8.GetBytes(jsonOperation);
                channel.BasicPublish(sibiling, sibiling, null, operationBodyBytes);
            }
        }
    }
}
