using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.Core
{
    public class QueuePoller
    {
        private readonly ConcurrentDictionary<BuildDefinition, bool> _status;
        private readonly QueueInformation _queueInformation;
        private SubscriptionClient client;

        public event EventHandler<bool> PinponChanged;
        public event EventHandler<string> LogVerbose;
        public event EventHandler<string> LogError;

        public QueuePoller(QueueInformation queueInformation)
        {
            this._queueInformation = queueInformation;
            this._status = new ConcurrentDictionary<BuildDefinition, bool>();
        }

        public void StartPolling()
        {
            client = new SubscriptionClient(
                _queueInformation.ConnectionString,
                _queueInformation.Topic,
                _queueInformation.Subscription);

            client.RegisterMessageHandler(HandleMessage, HandleError);
        }

        private Task HandleError(ExceptionReceivedEventArgs arg)
        {
            LogError?.Invoke(this, arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private Task HandleMessage(Message message, CancellationToken cancellationToken)
        {
            try
            {
                LogVerbose?.Invoke(this, $"New message {message.MessageId}");
                var jObject = JObject.Parse(Encoding.UTF8.GetString(message.Body));
                var definition = new BuildDefinition
                (
                    Guid.Parse(jObject["resourceContainers"]["collection"].Value<string>("id")),
                    Guid.Parse(jObject["resourceContainers"]["project"].Value<string>("id")),
                    jObject["resource"]["definition"].Value<int>("id")
                );
                var status = ParseStatus(jObject["resource"].Value<string>("result"));

                _status.AddOrUpdate(definition, status, (buildDefinition, b) => status);

                var hasFailingBuild = _status.Any(d => !d.Value);
                PinponChanged?.Invoke(this, hasFailingBuild);
            }
            catch (Exception e)
            {
                LogError?.Invoke(this, e.ToString());
            }

            return Task.CompletedTask;
        }

        private static bool ParseStatus(string status)
        {
            return status.Equals("succeeded", StringComparison.OrdinalIgnoreCase)
                || status.Equals("partiallySucceeded", StringComparison.OrdinalIgnoreCase);
        }

        public async Task StopPolling()
        {
            await client.CloseAsync();
        }
    }
}
