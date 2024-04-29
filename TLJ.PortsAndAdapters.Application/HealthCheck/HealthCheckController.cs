using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TLJ.PortsAndAdapters.Application.HealthCheck;

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
    /// Provides liveness status with HttpCode
    /// </summary>
    /// <returns>HttpCode</returns>
    [HttpGet("/health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> Health()
    {
        return Task.FromResult<IActionResult>(StatusCode(200));
    }

    /// <summary>
    /// Provides readiness status with HttpCode
    /// </summary>
    /// <returns>HttpCode</returns>
    [HttpGet("/ready")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Ready()
    {
        var result = await _healthCheckService.CheckHealthAsync(_healthCheckOptions.Predicate);
        if (result.Status == HealthStatus.Healthy)
        {
            return Ok();
        }

        _logger.LogWarning($"Ready check is not {HealthStatus.Healthy}!");
        return StatusCode(503);
    }

    /// <summary>
    /// Provides health check status report
    /// </summary>
    /// <returns>Health check detailed status report</returns>
    [HttpGet("/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> Status()
    {
        var result = await _healthCheckService.CheckHealthAsync(_healthCheckOptions.Predicate);
        return Ok(result);
    }
}