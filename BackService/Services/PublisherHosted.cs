using BackService.Hubs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MosquittoClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackService.Services
{
    public class PublisherHosted: BackgroundService
    {
        private readonly ILogger<PublisherHosted> _logger;
        private readonly Publisher _publisher;
        Random _rnd = new Random();

        public PublisherHosted(ILogger<PublisherHosted> logger, Publisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(() =>
            {
                while(!stoppingToken.IsCancellationRequested)
                {
                    var imagesList = Directory.EnumerateFiles(@"E:\Downloads\frames").ToList();
                    imagesList.Sort();

                    foreach (var image in imagesList)
                    {
                        _publisher.Publish(image);
                        Thread.Sleep(80);
                    }
                }
            }, stoppingToken);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Publisher starting.");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Publisher stopping.");
            await base.StopAsync(cancellationToken);
        }
    }
}
