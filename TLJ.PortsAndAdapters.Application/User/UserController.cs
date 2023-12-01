using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.CQRS.Core.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TLJ.PortsAndAdapters.Application.User.Commands;
using TLJ.PortsAndAdapters.Application.User.Queries;

namespace TLJ.PortsAndAdapters.Application.User
{
    public class UserController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public UserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        /// <summary>
        /// Create user's betting
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string userName)
        {
            var dto = await _queryDispatcher.QueryAsync(new SpecificCachedUserQuery(){ UserName = userName });
            return Json(dto);
        }

        /// <summary>
        /// Create user's betting
        /// </summary>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] UserCreationCommand command)
        {
            await _commandDispatcher.Send(command);
            return Ok();
        }
        
        /// <summary>
        /// Create user's betting
        /// </summary>
        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Delete([FromBody] RemovalUserCommand command)
        {
            await _commandDispatcher.Send(command);
            return Ok();
        }
    }
}