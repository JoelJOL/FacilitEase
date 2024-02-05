using System.Net.Sockets;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_NOTIFICATION
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TicketId { get; set; }
        public int? Sender { get; set; }
        public int Receiver { get; set; }
        public DateTime NotificationTimestamp { get; set; }
    }
}
