using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common
{
    public class MQTTService
    {
        public string ClientId = "";
        public int MessageCounter { get; private set; }
        public IManagedMqttClient GetMQTTClient(string clientId, string userName, string password ) {
            ClientId = clientId;
            Console.WriteLine();
            Console.WriteLine($"=============================================== CLIENT: {clientId} STARTING ==================================================");
            
            var param = new MqttClientOptionsBuilderTlsParameters
            {
                AllowUntrustedCertificates = true,
                UseTls = true,
                SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                Certificates = new List<X509Certificate>
                {
                    new X509Certificate2(@"c:\cert\client.pfx") //add password if created in step 6
                }
            };
            // Creates a new client
            var builder = new MqttClientOptionsBuilder().WithCredentials(userName, password)
                                                    .WithClientId(clientId)
                                                    .WithTls(param)
                                                    .WithTcpServer("localhost", 8883);

            // Create client options objects
            var options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                                    .WithClientOptions(builder.Build())
                                    .Build();

            // Creates the client object
            var _mqttClient = new MqttFactory().CreateManagedMqttClient();

            // Set up handlers
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
            // Starts a connection with the Broker
            _mqttClient.StartAsync(options).GetAwaiter().GetResult();
            return _mqttClient;
        }

        public void GetMQTTBroker() {
            Console.WriteLine("==================================== MQTT BROKER STARTING ==========================================");
            // Create the options for our MQTT Broker
            var options = new MqttServerOptionsBuilder()
                                                 // set endpoint to localhost
                                                 .WithDefaultEndpoint()
                                                 // port used will be 707
                                                 .WithDefaultEndpointPort(707)
                                                 // handler for new connections
                                                 .WithConnectionValidator(OnNewConnection)
                                                 // handler for new messages
                                                 .WithApplicationMessageInterceptor(OnNewMessage);
            // creates a new mqtt server     
            var mqttServer = new MqttFactory().CreateMqttServer();

            // start the server with options  
            mqttServer.StartAsync(options.Build()).GetAwaiter().GetResult();
            Console.ReadLine();
        }
        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            Console.WriteLine($"Client: {ClientId} cuccessfully connected.");
        }

        public void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            Console.WriteLine($"Client: {ClientId} couldn't connect to broker.");
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            Console.WriteLine($"Client: {ClientId} successfully disconnected.");
        }
        public void OnNewConnection(MqttConnectionValidatorContext context)
        {
            Console.WriteLine(string.Format("New connection: ClientId = {0}, Endpoint = {1}", context.ClientId, context.Endpoint));
        }

        public void OnNewMessage(MqttApplicationMessageInterceptorContext context)
        {
            var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);

            MessageCounter++;

            Console.WriteLine(string.Format(
                "MessageId: {0} - TimeStamp: {1} -- Message: ClientId = {2}, Topic = {3}, Payload = {4}, QoS = {5}, Retain-Flag = {6}",
                MessageCounter,
                DateTime.Now,
                context.ClientId,
                context.ApplicationMessage?.Topic,
                payload,
                context.ApplicationMessage?.QualityOfServiceLevel,
                context.ApplicationMessage?.Retain));
        }
    }
}
