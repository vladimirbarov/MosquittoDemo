using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using System;
using System.Text;

namespace MosquittoClient
{
    public class Publisher
    {
        private IManagedMqttClient managedMqttClientPublisher;
        public Publisher()
        {
            var mqttFactory = new MqttFactory();

            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true,
            };

            var options = new MqttClientOptions
            {
                ClientId = "ClientPublisher",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = "localhost",
                    Port = 1883,
                    TlsOptions = tlsOptions
                }
            };

            //options.Credentials = new MqttClientCredentials
            //{
            //    Username = "username",
            //    Password = Encoding.UTF8.GetBytes("password")
            //};

            options.CleanSession = true;
            //options.KeepAlivePeriod = TimeSpan.FromSeconds(5);
            managedMqttClientPublisher = mqttFactory.CreateManagedMqttClient();
            managedMqttClientPublisher.UseApplicationMessageReceivedHandler(HandleReceivedApplicationMessage);
            managedMqttClientPublisher.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnPublisherConnected);
            managedMqttClientPublisher.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnPublisherDisconnected);

            managedMqttClientPublisher.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = options
                });
        }

        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var item = $"Timestamp: {DateTime.Now:O} | Topic: {eventArgs.ApplicationMessage.Topic} | Payload: {eventArgs.ApplicationMessage.ConvertPayloadToString()} | QoS: {eventArgs.ApplicationMessage.QualityOfServiceLevel};";

            Console.WriteLine(item);
        }

        private void OnPublisherConnected(MqttClientConnectedEventArgs x)
        {
            Console.WriteLine("Publisher Connected;");
        }

        private void OnPublisherDisconnected(MqttClientDisconnectedEventArgs x)
        {
            Console.WriteLine("Publisher Disconnected;");
        }

        public void Publish(string data)
        {
            var payload = Encoding.UTF8.GetBytes($"{data}");
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("topic/imagesqueue")
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithRetainFlag()
                .Build();

            var result = managedMqttClientPublisher.PublishAsync(message).Result;
            Console.WriteLine($"Publisher Has published '{data}';");
        }
    }
}
