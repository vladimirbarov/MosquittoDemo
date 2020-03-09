using Microsoft.AspNetCore.SignalR;
using MosquittoClient;
using MQTTnet;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosquittoDemo.Models
{
    public class Subscriberhub
    {
        private readonly IHubContext<ImageQueueHub> _imageshub;
        private readonly Subscriber _subscriber;

        public Subscriberhub(IHubContext<ImageQueueHub> imagesHub)
        {
            _imageshub = imagesHub;
            _subscriber = new Subscriber();

            _subscriber.Subscribe();
            //_subscriber.managedMqttClientSubscriber.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnSubscriberMessageReceived);
        }

        private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs x)
        {
            var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel};";
            _imageshub.Clients.All.SendAsync("newimage", item);
        }
    }
}
