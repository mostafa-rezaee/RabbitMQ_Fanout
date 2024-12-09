
using Helper;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, Please enter user phone number:");
var phoneNumber = Console.ReadLine();
Console.WriteLine("Please enter user email:");
var email = Console.ReadLine();
string exchangeName = "userAlertExchange";

var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};
IConnection connection = await connectionFactory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();
await channel.ExchangeDeclareAsync(exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

var user = new User
{
    PhoneNumber = phoneNumber,
    Email = email
};
var userJSON = JsonConvert.SerializeObject(user);

await channel.BasicPublishAsync(exchange: exchangeName, routingKey: "", body: Encoding.UTF8.GetBytes(userJSON));

Console.WriteLine("User informaions sent to queue.");
Console.ReadKey();
