using Cellenza.Pinpon.Front.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cellenza.Pinpon.Front.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PinponController : ControllerBase
    {
        private readonly ILogger<PinponController> logger;
        private readonly IHubContext<PinponHub> pinponHub;

        public PinponController(
            ILogger<PinponController> logger,
            IHubContext<PinponHub> pinponHub)
        {
            this.logger = logger;
            this.pinponHub = pinponHub;
        }

        [HttpGet]
        public async Task<IActionResult> TurnOn()
        {
            logger.LogInformation("Pinpon turn on requested");
            await pinponHub.Clients.All.SendAsync("TurnOn");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> TurnOff()
        {
            logger.LogInformation("Pinpon turn on requested");
            await pinponHub.Clients.All.SendAsync("TurnOff");
            return Ok();
        }
    }
}
