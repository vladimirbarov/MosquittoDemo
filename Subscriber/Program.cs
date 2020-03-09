using System;
using System.Threading;
using MosquittoClient;

namespace SubscriberApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Random _rnd = new Random();
            var s = new Subscriber();
            s.Subscribe();

            while(Console.ReadKey().KeyChar != 'e')
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine("Subscriber app has finished. Press enter to exit.");
            Console.ReadLine();
        }
    }
}
