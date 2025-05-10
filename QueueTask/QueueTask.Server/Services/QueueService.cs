using Azure.Storage.Queues;
using QueueTask.Server.Models;

namespace QueueTask.Server.Services
{
    public class QueueService:IQueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureQueue");
            string queueName = "discounts";

            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task<DiscountResult?> GetDiscountAsync()
        {
            var messages = await _queueClient.ReceiveMessagesAsync(maxMessages: 1);

            if (messages.Value.Length == 0)
                return null;

            var message = messages.Value[0];

            if (message.DequeueCount >= 10)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                return null;
            }

            return new DiscountResult
            {
                Code = message.MessageText,
                MessageId = message.MessageId,
                PopReceipt = message.PopReceipt,
                DequeueCount = Convert.ToInt32(message.DequeueCount)
            };
        }

        public async Task ReturnDiscountAsync(string messageId, string popReceipt)
        {
            await _queueClient.UpdateMessageAsync(messageId, popReceipt, visibilityTimeout: TimeSpan.Zero);
        }
    }



}
