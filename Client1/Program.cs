﻿using Common;
using System;

namespace Client1
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new MQTTService();
            service.GetMQTTClient("1");
            Console.ReadLine();
        }
    }
}
