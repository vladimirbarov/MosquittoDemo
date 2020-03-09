using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using System;
using System.Collections.Generic;
using System.Text;

namespace MosquittoClient
{
    public class Subscriber
    {
        private IManagedMqttClient managedMqttClientSubscriber;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public Subscriber()
        {
            var mqttFactory = new MqttFactory();

            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptions
            {
                ClientId = "ClientSubscriber",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = "localhost",
                    Port = 1883,
                    TlsOptions = tlsOptions
                }
            };

            options.CleanSession = true;
            //options.KeepAlivePeriod = TimeSpan.FromSeconds(5);

            managedMqttClientSubscriber = mqttFactory.CreateManagedMqttClient();
            managedMqttClientSubscriber.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnSubscriberConnected);
            managedMqttClientSubscriber.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnSubscriberDisconnected);
            managedMqttClientSubscriber.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnSubscriberMessageReceived);

            managedMqttClientSubscriber.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = options
                });
        }

        private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs x)
        {
            var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel};";
            Console.WriteLine(item);
            if (MessageReceived != null)
                MessageReceived(this, new MessageReceivedEventArgs(x.ApplicationMessage.ConvertPayloadToString()));
        }

        private void OnSubscriberConnected(MqttClientConnectedEventArgs x)
        {
            Console.WriteLine("Subscriber Connected;");
        }

        private void OnSubscriberDisconnected(MqttClientDisconnectedEventArgs x)
        {
            Console.WriteLine("Subscriber Disconnected;");
        }

        public void Subscribe()
        {

            managedMqttClientSubscriber.SubscribeAsync(new TopicFilterBuilder().WithTopic("topic/imagesqueue").Build());
        }
    }
}
