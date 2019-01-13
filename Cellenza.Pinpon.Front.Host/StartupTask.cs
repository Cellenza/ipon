using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace Cellenza.Pinpon.Front.Host
{
    public sealed class StartupTask : IBackgroundTask
    {

        public StartupTask()
        {
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                var host = WebHost
                    .CreateDefaultBuilder()
                    .ConfigureLogging(logging =>
                    {
                        logging.AddEventSourceLogger();
                    })
                    .ConfigureAppConfiguration(config =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            {
                                "ConnectionStrings:DefaultConnection",
                                $"Data Source={Path.Combine(ApplicationData.Current.LocalFolder.Path, "QueuePoller.db")}"
                            }
                        });
                    })
                    .UseStartup<Startup>()
                    .UseUrls("http://+:80")
                    .Build();

                Program.RunMigration(host);

                host.Run();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            deferral.Complete();
        }

    }
}
