
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;

namespace Infrastructure.Messaging;


public static class RabbitMqConnection
{
    public static IConnection GetConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        return factory.CreateConnection();
    }
}
