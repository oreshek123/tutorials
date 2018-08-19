using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace NewTask
{
    class Program
    {
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }

        public static void Run(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                    routingKey: "task_queue",
                    basicProperties: properties,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
        static void Main(string[] args)
        {
            int i = 0;
            while (true)
            {
                
                Run($"{i}");
                Thread.Sleep(1000);
                i++;
            }
        }
    }
}
