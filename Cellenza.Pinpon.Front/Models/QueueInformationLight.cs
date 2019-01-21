using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.Front.Models
{
    public class QueueInformationLight
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Subscription { get; set; }

        public string Topic { get; set; }

        public bool IsActive { get; set; }
    }

    public static class QueueInformationExtension
    {
        public static IQueryable<QueueInformationLight> ToQueryableQueueInformationLight(this IQueryable<QueueInformation> queues)
        {
            return queues.Select(queue => queue.ToQueueInformationLight());
        }

        public static QueueInformationLight ToQueueInformationLight(this QueueInformation queue)
        {
            return new QueueInformationLight
            {
                Id = queue.Id,
                DisplayName = queue.DisplayName,
                Subscription = queue.Subscription,
                Topic = queue.Topic,
                IsActive = queue.IsActive
            };
        }
    }
}
