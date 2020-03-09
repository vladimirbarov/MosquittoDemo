using BackService.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MosquittoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackService.Services
{
    public class SubscriberHosted : BackgroundService
    {
        private readonly ILogger<SubscriberHosted> _logger;
        private readonly Subscriber _subscriber;
        private readonly IHubContext<ImagesQueueHub> _imagesHub;

        public SubscriberHosted(ILogger<SubscriberHosted> logger, Subscriber subscriber, IHubContext<ImagesQueueHub> imagesHub)
        {
            _logger = logger;
            _subscriber = subscriber;
            _imagesHub = imagesHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Subscriber running at: {time}", DateTimeOffset.Now);
                _subscriber.MessageReceived += _subscriber_MessageReceived;
                _subscriber.Subscribe();
                await Task.CompletedTask;
            }
        }

        private void _subscriber_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _imagesHub.Clients.All.SendAsync("newimage", e.Message);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Subscriber starting.");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Subscriber stopping.");
            await base.StopAsync(cancellationToken);
        }
    }
}
