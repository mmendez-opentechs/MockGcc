using Microsoft.AspNetCore.Mvc;
using MockGcc.Service.HttpClients;
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
        private readonly State.State _state;
        private readonly MockPersonInfoClient _mockPersonInfoClient;
        private readonly MockAccountClient _mockAccountClient;

        public MainController(
            ILogger logger,
            State.State frequencyState,
            MockPersonInfoClient mockPersonInfoClient,
            MockAccountClient mockAccountClient)
        {
            _logger = logger;
            _state = frequencyState;
            _mockPersonInfoClient = mockPersonInfoClient;
            _mockAccountClient = mockAccountClient;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Called GetMemberData");
            
            _logger.LogInformation("");

            var isEnabled = true;
            if (isEnabled)
            {
                
            }

            return new OkResult();
        }

        [HttpPost]
        [Route("setstate")]
        public IActionResult SetState(State.State state)
        {
            _logger.LogInformation("Called SetState");

            _logger.LogInformation("");

            _state.MockPersonInfoRequestRate = state.MockPersonInfoRequestRate;
            _state.MockAccountRequestRate = state.MockAccountRequestRate;
            _state.TestHorizontalAutoscaling = state.TestHorizontalAutoscaling;
            _state.TestVerticalAutoscaling = state.TestVerticalAutoscaling;

            return new OkResult();
        }

        [HttpPost]
        [Route("getlatency")]
        public IActionResult GetLatency()
        {
            _logger.LogInformation("Called GetLatency");

            return new ObjectResult(_state)
            {
                StatusCode = 200
            };
        }

        [HttpPost]
        [Route("callPersonInfo")]
        public async Task<IActionResult> CallPersonInfo()
        {
            _logger.LogInformation("Called GetLatency");

            await _mockPersonInfoClient.CallPersonInfo();

            return new OkResult();
        }

        [HttpPost]
        [Route("callaccount")]
        public async Task<IActionResult> CallAccount()
        {
            _logger.LogInformation("Called GetLatency");

            await _mockAccountClient.CallAccountInfo();

            return new OkResult();
        }
    }
}
