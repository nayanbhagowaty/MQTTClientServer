using Common;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client3
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientId = "client3";
            var service = new MQTTService();
            var client = service.GetMQTTClient(clientId, clientId, "password");

            string[] clients = { "1", "2" };
            do
            {
                Task.Delay(1000).GetAwaiter().GetResult();
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Select client Id to publish to: (1) Client 1, (2) Client 2");
                var val = Console.ReadLine();
                if (!clients.Any(a => a.Contains(val)))
                    //if (val != "1" && val != "2")
                    Console.WriteLine("Invalid Input!");
                else
                {
                    string json = JsonSerializer.Serialize(new { message = $"Hi to client{val})", sent = DateTimeOffset.UtcNow });
                    client.PublishAsync($"client{val}/topic/json", json);
                }
            } while (true);

            Console.ReadLine();
        }
    }
}
