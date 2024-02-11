using System.Text;
using RabbitMQ.Client;

public class RabbitMqService
{
    private readonly string _connectionString;
    private readonly string _queueName;

    public RabbitMqService(string connectionString, string queueName)
    {
        _connectionString = connectionString;
        _queueName = queueName;
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { Uri = new Uri(_connectionString) };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
