using MosquittoClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublisherApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.CompletedTask;
            var p = new Publisher();
            var _rnd = new Random();

            var imagesList = Directory.EnumerateFiles(@"E:\Downloads\frames").ToList();
            imagesList.Sort();

            foreach (var image in imagesList)
            {
                p.Publish(image);
                Thread.Sleep(1000 * _rnd.Next(1, 5));
            }
            
            Console.ReadLine();
        }
    }
}
