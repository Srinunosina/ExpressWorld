using ExpressWorld.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpressWorld.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
