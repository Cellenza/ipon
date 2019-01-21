using Cellenza.Pinpon.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.BackgroundApp
{
    class QueuePollerService
    {
        private readonly ILogger<QueuePollerService> logger;
        private readonly PinService pinService;
        private readonly Dictionary<QueueInformation, QueuePoller> pollers;

        public QueuePollerService(ILogger<QueuePollerService> logger, PinService pinService)
        {
            this.logger = logger;
            this.pinService = pinService;
            pollers = new Dictionary<QueueInformation, QueuePoller>();
        }

        public void SubscribeTo(QueueInformation queueInformation)
        {
            if (pollers.ContainsKey(queueInformation))
            {
                return;
            }

            var queuePoller = new QueuePoller(queueInformation);
            queuePoller.LogError += (o, m) => logger.LogError(m);
            queuePoller.LogVerbose += (o, m) => logger.LogDebug(m);
            queuePoller.PinponChanged += QueuePoller_PinponChanged;
            queuePoller.StartPolling();

            logger.LogInformation($"Subscribe to new queue : sub [{queueInformation.Subscription }], Topic [{queueInformation.Topic}]");

            pollers.Add(queueInformation, queuePoller);
        }

        public async Task Unsubscribe(int id)
        {
            var queue = pollers.Keys.FirstOrDefault(q => q.Id == id);

            if (queue != null)
            {
                pollers.Remove(queue, out var poller);
                await poller.StopPolling();
            }
        }

        private void QueuePoller_PinponChanged(object sender, bool status)
        {
            if (status)
            {
                pinService.TurnOn();
            }
            else
            {
                pinService.TurnOff();
            }
        }
    }
}
