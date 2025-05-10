namespace QueueTask.Server.Models
{
    public class DiscountResult
    {
        public string Code { get; set; } = "";
        public string MessageId { get; set; } = "";
        public string PopReceipt { get; set; } = "";
        public int DequeueCount { get; set; }
    }
}
