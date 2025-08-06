using Logic;
using Microsoft.AspNetCore.Mvc;

namespace DemoEvDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsOnlyController : ControllerBase
    {
        private readonly ILogger<EventsOnlyController> _logger;
        private readonly IEvDbFundsFactory _factory;

        public EventsOnlyController(ILogger<EventsOnlyController> logger, IEvDbFundsFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        [HttpPost("{id}")]
        public async Task PostAsync(int id, [FromBody] Request request)
        {
            var stream = await _factory.GetAsync(id);
            var e = new DepositedEvent(id.ToString())
            {
                Amount = request.Funds
            };
            await stream.AppendAsync(e);
            await stream.StoreAsync();
        }
    }
}

