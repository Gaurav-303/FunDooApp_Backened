using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class RabbitMqConsumer
    {
        public void Consume()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "email_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var emailData = JsonSerializer.Deserialize<dynamic>(message);

                Console.WriteLine($"Sending Email To: {emailData.Email}");
            };

            channel.BasicConsume(
                queue: "email_queue",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}
