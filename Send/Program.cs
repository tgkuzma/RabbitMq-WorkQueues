using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var message = "";
            var closeApp = false;
            Console.WriteLine("----------Sender----------");
            Console.WriteLine("Write a message to send. Each \".\" is one second of work for the task.");
            Console.WriteLine("[Enter] to exit");

            do
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    if (!string.IsNullOrEmpty(message))
                    {

                        channel.QueueDeclare(queue: "task_queue",
                            durable: true,
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
                    }
                }
                message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    closeApp = true;
                }

            } while (!closeApp);

            Environment.Exit(0);
        }
    }
}
