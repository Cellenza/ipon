using Cellenza.Pinpon.Front.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.Front
{
    public class PinponQueueContext : DbContext
    {
        public PinponQueueContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<QueueInformation> QueueInformations { get; set; }
    }
}
