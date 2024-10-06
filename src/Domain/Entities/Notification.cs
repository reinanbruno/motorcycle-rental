namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string EventType { get; set; }
        public string Message { get; set; }
    }
}
