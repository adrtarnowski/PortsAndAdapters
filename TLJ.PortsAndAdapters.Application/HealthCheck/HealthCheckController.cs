using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TLJ.PortsAndAdapters.Application.HealthCheck
{
    public class HealthCheckController : BaseController
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly HealthCheckOptions _healthCheckOptions;
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(
            HealthCheckService healthCheckService,
            IOptions<HealthCheckOptions> healthCheckOptions,
            ILogger<HealthCheckController> logger)
        {
            _healthCheckService = healthCheckService;
            _healthCheckOptions = healthCheckOptions.Value;
            _logger = logger;
        }

        /// <summary>
        /// Provides health check report
        /// </summary>
        /// <returns>Health check detailed report</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json", Type = typeof(HealthReport))]
        public async Task<IActionResult> Index()
        {
            var result = await _healthCheckService.CheckHealthAsync(_healthCheckOptions.Predicate);
            return Ok(result);
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