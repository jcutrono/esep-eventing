using System;

namespace message_broker.Controllers
{
    public class Order
    {
        public DateTime Occurred { get; set; }
        public Guid UserId { get; set; }
        public string EventType { get; set; }
        public Guid ItemId { get; set; }
        public int Amount { get; set; }
    }
}