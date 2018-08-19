using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ProducerApp
{
    class Program
    {
        public static string GeneratePin()
        {
            string pin = string.Empty;
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                pin += r.Next(0, 10);
            }

            return pin;
        }

        public static void SendMessageToQueue(string message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //создает пулл коннекшенов
            using (IConnection connection = factory.CreateConnection())
                // дает один коннекшн
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

           
        }
        static void Main(string[] args)
        {
            while (true)
            {
                SendMessageToQueue("Nastya", "messages");
                Thread.Sleep(5000);
            }
           
        }
    }
}
