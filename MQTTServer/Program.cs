using System;
using Common;

namespace MQTTServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new MQTTService();
            service.GetMQTTBroker();
            Console.ReadLine();
        }
    }
}
