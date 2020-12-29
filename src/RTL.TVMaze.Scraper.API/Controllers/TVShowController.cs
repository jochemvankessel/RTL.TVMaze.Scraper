using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RTL.TVMaze.Scraper.Application.Queries;

namespace RTL.TVMaze.Scraper.API.Controllers
{
    [ApiController]
    [Route("shows")]
    public class TVShowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TVShowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("with-cast-embedded")]
        public async Task<IActionResult> GetPaginatedListOfTVShows([FromQuery] GetPaginatedListOfTVShows request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response.TVShows);
        }
    }
}
