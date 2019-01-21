namespace Cellenza.Pinpon.Front.Models
{
    public class QueueInformation
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string ConnectionString { get; set; }

        public string Topic { get; set; }

        public string Subscription { get; set; }

        public bool IsActive { get; set; }
    }
}
