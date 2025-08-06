using Logic;
using Microsoft.AspNetCore.Mvc;

namespace DemoEvDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsOutboxAndViewsController : ControllerBase
    {
        private readonly ILogger<EventsOutboxAndViewsController> _logger;
        private readonly IEvDbFundsWithOutboxWithViewsFactory _factory;

        public EventsOutboxAndViewsController(ILogger<EventsOutboxAndViewsController> logger, IEvDbFundsWithOutboxWithViewsFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }


        [HttpGet("{id}")]
        public async Task<double> GetAsync(int id)
        {
            var stream = await _factory.GetAsync(id);
            return stream.Views.Balance.Amount;
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

