using System;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.CQRS.Core.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TLJ.PortsAndAdapters.Application.Bookmaking.Commands;
using TLJ.PortsAndAdapters.Application.Bookmaking.Queries;

namespace TLJ.PortsAndAdapters.Application.Bookmaking
{
    public class BookmakingController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BookmakingController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
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
        public async Task<IActionResult> Get(Guid matchId, Guid userId)
        {
            var dto = await _queryDispatcher.QueryAsync(new BookMatchCommandByMatchAndUserQuery{ MatchId = matchId, UserId = userId});
            return Json(dto);
        }

        /// <summary>
        /// Create user's betting
        /// </summary>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] BookMatchCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        /// <summary>
        /// Modify user's betting value
        /// </summary>
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Put([FromBody] ChangeBookValueCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
        
        /// <summary>
        /// Create user's betting
        /// </summary>
        [HttpDelete("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> CloseBookmaking([FromBody] CloseBookmakingCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
    }
}