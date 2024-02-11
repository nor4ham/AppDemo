using AppDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var dbUser = Environment.GetEnvironmentVariable("UserID");
var dbPassword = Environment.GetEnvironmentVariable("Password");
var dbHost = Environment.GetEnvironmentVariable("Server");
var dbPort = Environment.GetEnvironmentVariable("Port");
var dbName = Environment.GetEnvironmentVariable("Database");


var connString = $"Host={dbHost};Port={dbPort};Username=postgres;Password={dbPassword};Database={dbName};Pooling=true;";
Console.WriteLine("Connection String Debug:");
Console.WriteLine(connString);

 // Configure database context directly with constructed connection string
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(connString));

// redis

var redisHost = Environment.GetEnvironmentVariable("RedisHost"); 
var redisPort = Environment.GetEnvironmentVariable("RedisPort");
var redisConnection = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort},abortConnect=false");
Console.WriteLine("Redis Connection String Debug:");
Console.WriteLine($"{redisHost}:{redisPort},abortConnect=false");
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);

// rabbitMQ.

var rabbitMQHost = Environment.GetEnvironmentVariable("RabbitMQHost");
var rabbitMQPort = Environment.GetEnvironmentVariable("RabbitMQPort");

Console.WriteLine("RabbitMQ Connection String Debug:");
Console.WriteLine($"{rabbitMQHost}:{rabbitMQPort}");

var rabbitMQConnectionFactory = new ConnectionFactory
{
    HostName = rabbitMQHost,
    Port = int.Parse(rabbitMQPort),
    UserName = "admin", // Set your RabbitMQ credentials
    Password = "admin"
    // Add other RabbitMQ configurations as needed
};

var rabbitMQConnection = rabbitMQConnectionFactory.CreateConnection();

builder.Services.AddSingleton<IConnection>(rabbitMQConnection);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AppDemo",
        Version = "v1"
    });

});

var app = builder.Build();

var env = app.Services.GetRequiredService<IWebHostEnvironment>();
DatabaseInitializer.InitializeDatabase(app, env);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppDemo V1");
        c.RoutePrefix = "swagger"; // Make sure this matches the one you are trying to access

    });
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
