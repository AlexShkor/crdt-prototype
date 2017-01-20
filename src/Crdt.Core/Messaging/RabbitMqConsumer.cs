using System;
using System.Text;
using Crdt.Core.Configuration;
using Crdt.Core.Messaging.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crdt.Core.Messaging
{
    public class RabbitMqConsumer : IReplicaOperationsConsumer
    {
        private readonly QueueConfiguration _configuration;
        private readonly IModel _channel;
        private string _consumerTag;

        public RabbitMqConsumer(QueueConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://guest:guest@127.0.0.1:5672/";
            var connection = factory.CreateConnection();

            var myQueue = configuration.ConsumingQueue;
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


        public void ListenToSibilings(Action<AddToSetCommand> update)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var bodyBytes = ea.Body;
                var bodyString = Encoding.UTF8.GetString(bodyBytes);
                var operation = Serializer.DeserializeFromString(bodyString);

                // process operation 
                if (operation is AddToSetCommand)
                    update(operation as AddToSetCommand);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            var myQueue = _configuration.ConsumingQueue;
            if (myQueue == null) throw new Exception("Consuming queue is undefined for current replica");

            _consumerTag = _channel.BasicConsume(myQueue, false, consumer);
        }
    }
}
