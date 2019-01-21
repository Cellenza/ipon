using Cellenza.Pinpon.Core;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.BackgroundApp
{
    class SignalRService
    {
        private readonly PinService pinService;
        private readonly QueuePollerService queuePollerService;
        private readonly ILogger<SignalRService> logger;
        private readonly HubConnection connection;

        public SignalRService(
            IHubConnectionBuilder hubConnectionBuilder,
            PinService pinService,
            QueuePollerService queuePollerService,
            ILogger<SignalRService> logger)
        {
            hubConnectionBuilder.WithUrl("http://localhost/pinponhub");

            connection = hubConnectionBuilder.Build();
            this.pinService = pinService;
            this.queuePollerService = queuePollerService;
            this.logger = logger;
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            connection.On("TurnOn", TurnPinponOn);
            connection.On("TurnOff", TurnPinponOff);
            connection.On<QueueInformation>("AddQueue", AddQueuePoller);
            connection.On<int>("RemoveQueue", RemoveQueuePoller);
            await OpenConnection(cancellationToken);
            connection.Closed += Connection_Closed;
            await RetrieveConfiguration();
        }

        private async Task RetrieveConfiguration()
        {
            logger.LogDebug("Websocket : Retrieve Configuration");
            var queueInformations = await connection.InvokeAsync<IEnumerable<QueueInformation>>("GetQueues");
            var queues = queueInformations.ToList();

            foreach (var queue in queues)
            {
                queuePollerService.SubscribeTo(queue);
            }

            logger.LogDebug($"Websocket : Configuration loaded, listen to {queues.Count} queues");
        }


        private async Task Connection_Closed(Exception ex)
        {
            logger.LogError(ex, "Websocket : connection closed unexpectedly");
            logger.LogInformation("Websocket : try to reconnect");
            await OpenConnection(CancellationToken.None);
        }

        private async Task OpenConnection(CancellationToken cancellationToken)
        {
            logger.LogDebug("Websocket : try to connect to server");
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await connection.StartAsync(cancellationToken);
                    logger.LogInformation("Websocket : connection open");
                    break;
                }
                catch (TaskCanceledException)
                {
                    logger.LogError("Websocket : connection cancelled");
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Websocket : connection failed");
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
            }
        }

        private void TurnPinponOn()
        {
            logger.LogDebug("Websocket : turn pinpon on");
            pinService.TurnOn();
        }

        private void TurnPinponOff()
        {
            logger.LogDebug("Websocket : turn pinpon off");
            pinService.TurnOff();
        }

        private void AddQueuePoller(QueueInformation obj)
        {
            logger.LogDebug($"Websocket : add new poller {obj.Subscription} / {obj.Topic}");
            queuePollerService.SubscribeTo(obj);
        }

        private void RemoveQueuePoller(int id)
        {
            logger.LogDebug($"Websocket : remove poller {id}");
            queuePollerService.Unsubscribe(id).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    logger.LogDebug($"Websocket : remove poller {id} successful");
                }
                else
                {
                    logger.LogError(t.Exception, $"Websocket : remove poller {id} failed");
                }
            });
        }
    }
}
