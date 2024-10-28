using ExpressWorld.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpressWorld.API.Controllers
{
    /// <summary>
    /// Controller for searching products in the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Searches for products based on provided query parameters.
        /// </summary>
        /// <param name="query">The search criteria including filters and pagination options.</param>
        /// <returns>A list of products that match the search criteria.</returns>
        /// <response code="200">Returns a list of matching products.</response>
        /// <response code="400">If the search parameters are invalid.</response>
        /// <response code="500">If an internal error occurs.</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search([FromQuery] SearchProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
