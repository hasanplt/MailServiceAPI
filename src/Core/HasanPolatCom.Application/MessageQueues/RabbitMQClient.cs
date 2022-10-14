using HasanPolatCom.Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.MessageQueues
{
    public class RabbitMQClient
    {
        public void Add(string QueueName, object obje)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                string message = JsonConvert.SerializeObject(obje);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: QueueName,
                                     basicProperties: null,
                                     body: body);
            }
        }

    }
}
