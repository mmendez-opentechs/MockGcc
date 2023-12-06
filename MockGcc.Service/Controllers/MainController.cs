using Microsoft.AspNetCore.Mvc;
using MockGcc.Service.State;
using System.Text.Json;

namespace MockGcc.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class MainController
    {
        private readonly ILogger _logger;
        private readonly State.State _frequencyState;

        public MainController(ILogger logger, State.State frequencyState)
        {
            _logger = logger;
            _frequencyState = frequencyState;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug("Called GetMemberData");
            
            _logger.LogDebug("");

            var isEnabled = true;
            if (isEnabled)
            {
                
            }

            return new OkResult();
        }

        [HttpPost]
        [Route("setfrequency")]
        public async Task<IActionResult> SetFrequency(State.State frequencyState)
        {
            _logger.LogDebug("Called SetFrequency");

            _logger.LogDebug("");

            _frequencyState.MockPersonInfoRate = frequencyState.MockPersonInfoRate;
            _frequencyState.MockAccountRate = frequencyState.MockAccountRate;

            return new OkResult();
        }

        [HttpPost]
        [Route("getlatency")]
        public async Task<IActionResult> GetLatency()
        {
            _logger.LogDebug("Called GetLatency");

            return new ObjectResult(_frequencyState)
            {
                StatusCode = 200
            };
        }
    }
}
