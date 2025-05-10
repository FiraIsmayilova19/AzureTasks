using QueueTask.Server.Models;

namespace QueueTask.Server.Services
{
    public interface IQueueService
    {
        Task<DiscountResult?> GetDiscountAsync();
        Task ReturnDiscountAsync(string messageId, string popReceipt);
    }
}
