using Cellenza.Pinpon.Core;
using System;

namespace Cellenza.Pinpon.QueueDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueInformation = new QueueInformation
            {
                ConnectionString = @"Endpoint=sb://pinpon-test.servicebus.windows.net/;SharedAccessKeyName=build-consumer;SharedAccessKey=Oa3j00w6XDWfrfNvI6gGYJG6EtQhJyOip2bxtudL674=",
                Topic = @"build",
                Subscription = @"pinpon",
            };
            var queuePoller = new QueuePoller(queueInformation);

            queuePoller.LogError += (o, e) => Console.Error.WriteLine(e);
            queuePoller.LogVerbose += (o, m) => Console.WriteLine(m);
            queuePoller.PinponChanged += (o, b) => Console.WriteLine(b);
            queuePoller.StartPolling();
            Console.ReadKey();
        }
    }
}
