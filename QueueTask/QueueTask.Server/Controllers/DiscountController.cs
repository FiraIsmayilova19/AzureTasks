using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueueTask.Server.Services;

namespace QueueTask.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IQueueService _discountService;

        public DiscountController(IQueueService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscount()
        {
            var result = await _discountService.GetDiscountAsync();
            if (result == null)
                return Ok(new { message = "No discount available" });

            return Ok(result);
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnDiscount([FromQuery] string messageId, [FromQuery] string popReceipt)
        {
            await _discountService.ReturnDiscountAsync(messageId, popReceipt);
            return Ok(new { message = "Discount returned to queue" });
        }
    }

}
