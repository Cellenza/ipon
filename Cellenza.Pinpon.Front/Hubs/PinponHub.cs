using Cellenza.Pinpon.Front.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.Front.Hubs
{
    public class PinponHub : Hub
    {
        private readonly ILogger<PinponHub> logger;
        private readonly PinponQueueContext context;

        public PinponHub(
            ILogger<PinponHub> logger,
            PinponQueueContext context)
            : base()
        {
            this.logger = logger;
            this.context = context;
        }

        public override Task OnConnectedAsync()
        {
            logger.LogInformation($"{Context.ConnectionId} / {Context.UserIdentifier} joined the pinpon hub");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            logger.LogInformation($"{Context.ConnectionId} / {Context.UserIdentifier} lefted the pinpon hub");
            return base.OnDisconnectedAsync(exception);
        }


        public IEnumerable<QueueInformation> GetQueues()
        {
            logger.LogInformation($"Queues configuration requested");
            return context.QueueInformations.Where(queue => queue.IsActive).ToList();
        }
    }
}
