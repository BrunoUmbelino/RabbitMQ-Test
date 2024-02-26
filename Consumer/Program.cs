using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            using var connection = connectionFactory.CreateConnection();

            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "FilaDePensamentos",
                   durable: false,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Mensagem recebida: \n{message}");
                };

                channel.BasicConsume(queue: "FilaDePensamentos",
                    autoAck: true,
                    consumer: consumer);

                Console.ReadLine();
            }
        }
    }
}