using System.Text;
using AppDemo.Data;
using AppDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using StackExchange.Redis;
namespace AppDemo.Controllers;
[ApiController]
[Route("api")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly ApiDbContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly IConnection _rabbitMQConnection;

    public DriversController(
        ILogger<DriversController> logger,
        IConnectionMultiplexer redis,
        IConnection rabbitMQConnection,
        ApiDbContext context)
    {
        _logger = logger;
        _redis = redis;
        _rabbitMQConnection = rabbitMQConnection;
        _context = context;
    }
    [HttpGet("Database")]
    public async Task<IActionResult> Get()
    {
        var driver =  new Driver()
        {
            DriverNb = 44,
            Name = "Mohammed .."
        };
        Console.WriteLine("************");
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
        var drivers = await _context.Drivers.ToListAsync();
        if (drivers == null )
        {return StatusCode(500, "Database is not available.");}
        else {return Ok(drivers);}
       
    }
    [HttpGet("Redis")]
    public IActionResult CheckRedis()
    {
        var db = _redis.GetDatabase();
        var key = "redis_test_key";
        var value = "test";

        db.StringSet(key, value);
        var getValue = db.StringGet(key);

        if (getValue == value)
        {
            return Ok("Redis is available.");
        }
        else
        {
            return StatusCode(500, "Redis is not available.");
        }
    }

[HttpGet("Rabbitmq")]
    public IActionResult SendMessage()
    {
        try
        {
            using (var channel = _rabbitMQConnection.CreateModel())
            {
                var queueName = "your_queue_name"; // Specify your queue name
                var message = "Hello RabbitMQ!"; // Specify your message

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
            }

            return Ok("Message sent to RabbitMQ.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


}
