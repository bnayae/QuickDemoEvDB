using Logic;
using Microsoft.AspNetCore.Mvc;

namespace DemoEvDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsAndViewsController : ControllerBase
    {
        private readonly ILogger<EventsAndViewsController> _logger;
        private readonly IEvDbFundsWithBalanceFactory _factory;

        public EventsAndViewsController(ILogger<EventsAndViewsController> logger,
            IEvDbFundsWithBalanceFactory factory)
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
        public async Task<double> PostAsync(int id, [FromBody] Request request)
        {
            var stream = await _factory.GetAsync(id);
            var e = new DepositedEvent(id.ToString())
            {
                Amount = request.Funds
            };
            await stream.AppendAsync(e);
            await stream.StoreAsync();
            return stream.Views.Balance.Amount;
        }
    }
}

