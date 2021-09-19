using Common;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Text;

namespace Client1
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientId = "client1";
            var service = new MQTTService();
            var _mqttClient = service.GetMQTTClient(clientId, clientId, "password");
            _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic($"{clientId}/topic/json").Build());
            _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic($"client2/topic/json").Build());
            // Starts a connection with the Broker
            //_mqttClient.StartAsync(options).GetAwaiter().GetResult();
            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });
            Console.ReadLine();
        }
    }
}
