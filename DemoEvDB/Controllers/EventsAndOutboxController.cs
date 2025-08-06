using Logic;
using Microsoft.AspNetCore.Mvc;

namespace DemoEvDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsAndOutboxController : ControllerBase
    {
        private readonly ILogger<EventsAndOutboxController> _logger;
        private readonly IEvDbFundsWithOutboxFactory _factory;

        public EventsAndOutboxController(ILogger<EventsAndOutboxController> logger, IEvDbFundsWithOutboxFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        [HttpPost("{id}")]
        public async Task PostAsync(int id, [FromBody] Request request)
        {
            var stream = await _factory.GetAsync(id);

            if (request.Action == ActionType.Deposit)
            {
                var e = new DepositedEvent(id.ToString())
                {
                    Amount = request.Funds
                };
                await stream.AppendAsync(e);
            }
            else
            {
                var e = new WithdrewEvent(id.ToString())
                {
                    Value = request.Funds
                };
                await stream.AppendAsync(e);
            }
            await stream.StoreAsync();
        }
    }
}

