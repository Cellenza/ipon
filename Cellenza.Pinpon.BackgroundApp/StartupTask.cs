using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Cellenza.Pinpon.BackgroundApp
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const int PinponPin = 18;
        private readonly ServiceProvider provider;

        public StartupTask()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddDebug();
                builder.AddEventSourceLogger();
            });
            services.AddSingleton<PinService, PinService>();
            services.AddSingleton<IHubConnectionBuilder, HubConnectionBuilder>();
            services.AddSingleton<QueuePollerService, QueuePollerService>();
            services.AddSingleton<SignalRService, SignalRService>();

            provider = services.BuildServiceProvider();
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var logger = provider.GetService<ILoggerFactory>().CreateLogger<StartupTask>();

            var deferral = taskInstance.GetDeferral();
            var tcs = new TaskCompletionSource<object>();
            var cancellationTokenSource = new CancellationTokenSource();
            taskInstance.Canceled += (sender, reason) => { cancellationTokenSource.Cancel(); tcs.SetResult(true); };

            try
            {
                logger.LogInformation("Application start");

                var pinService = provider.GetService<PinService>();

                await pinService.Initialize(PinponPin);

                var signalR = provider.GetService<SignalRService>();

                await signalR.Initialize(cancellationTokenSource.Token);
                
                await tcs.Task;
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "");
            }
            finally
            {
                logger.LogInformation("Application stop");
                deferral.Complete();
            }
        }
    }
}
