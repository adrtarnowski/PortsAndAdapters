using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TLJ.PortsAndAdapters.Application.HealthCheck
{
    public class HealthCheckController : BaseController
    {
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Provides health check report
        /// </summary>
        /// <returns>Health check detailed report</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Health check works!");
            return Ok();
        }
        
        /// <summary>
        /// Provides health check report
        /// </summary>
        /// <returns>Health check detailed report</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> Echo([FromBody] HealthCheckCommand command)
        {
            string info = $"Health check works.Receive the message {command.echoMessage}";
            _logger.LogInformation(info);
            return Ok();
        }
    }
    
    public class HealthCheckCommand : ICommand
    {
        public string? echoMessage { get; set; }
    }
}