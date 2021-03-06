namespace FireFightersApp.Models
{
    public class Call
    {
        public int CallId { get; set; }
        public string CallerId { get; set; }
        public string Address { get; set; }
        public DateTime CallDatetime { get; set; }
        public CallStatus Status { get; set; }
        public string? Responsible { get; set; }
    }

    public enum CallStatus
    {
        Received,
        Assigned,
        Completed
    }
}
