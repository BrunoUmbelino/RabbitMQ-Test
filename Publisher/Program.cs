using RabbitMQ.Client;
using System.Text;

namespace Publisher
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

                string message = "O urgente geralmente atenta contra o necessário. - Mao Tse-Tung";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "FilaDePensamentos",
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"[x] Mensagem enviada: \n{message}");
            }
        }
    }
}