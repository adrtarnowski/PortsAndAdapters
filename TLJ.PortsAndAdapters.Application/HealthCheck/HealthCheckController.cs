using System.Threading.Tasks;
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
    }
}