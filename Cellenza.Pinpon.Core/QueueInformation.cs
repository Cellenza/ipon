using System;
using System.Collections.Generic;
using System.Text;

namespace Cellenza.Pinpon.Core
{
    public class QueueInformation
    {
        public int Id { get; set; }

        public string ConnectionString { get; set; }

        public string Topic { get; set; }

        public string Subscription { get; set; }
    }
}
