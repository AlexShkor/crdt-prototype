using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Crdt.Core.Messaging
{
    public class RabbitMqMessageClient
    {
        public RabbitMqMessageClient()
        {

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = "amqp://guest:guest@127.0.0.1:5672/";
            IConnection conn = factory.CreateConnection();

            IModel channel = conn.CreateModel();

            var exchangeName = "crdt-replica-1";
            var queueName = "crdt-replica-1";
            string routingKey = "test-routing-key";
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);


            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("I wanna trulala!");
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                // ... process the message
                Console.WriteLine(body);
                channel.BasicAck(ea.DeliveryTag, false);
            };
            String consumerTag = channel.BasicConsume(queueName, false, consumer);


        }
    }
}
