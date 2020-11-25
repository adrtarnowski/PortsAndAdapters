using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TLJ.PortsAndAdapters.Application.HealthCheck
{
    public class HealthCheckController : BaseController
    {
        /// <summary>
        /// Provides health check report
        /// </summary>
        /// <returns>Health check detailed report</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}