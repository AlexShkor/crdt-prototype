using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crdt.Core.Messaging.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crdt.Core.Messaging
{
    public class RabbitMqConsumer : IReplicaOperationsConsumer
    {
        private readonly IModel _channel;
        private string _consumerTag;
        private const string ConsumingQueueEnvVarName = "MY_QUEUE";

        public RabbitMqConsumer()
        {
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://guest:guest@127.0.0.1:5672/";
            var connection = factory.CreateConnection();

            var myQueue = Environment.GetEnvironmentVariable(ConsumingQueueEnvVarName);
            if (myQueue == null) throw new Exception("Consuming queue is undefined for current replica");

            var channel = connection.CreateModel();
            var exchangeName = myQueue;
            var queueName = myQueue;
            string routingKey = myQueue;
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            _channel = channel;
        }


        public void ListenToSibilings(Action<UpdateDocumentCommand> update)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var bodyBytes = ea.Body;
                var bodyString = System.Text.Encoding.UTF8.GetString(bodyBytes);
                var operation = Serializer.DeserializeFromString(bodyString);

                // process operation 
                if (operation is UpdateDocumentCommand)
                    update(operation as UpdateDocumentCommand);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            var myQueue = Environment.GetEnvironmentVariable(ConsumingQueueEnvVarName);
            if (myQueue == null) throw new Exception("Consuming queue is undefined for current replica");

            _consumerTag = _channel.BasicConsume(myQueue, false, consumer);
        }
    }
}
