using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using RTL.TVMaze.Scraper.Application.Queries;

namespace RTL.TVMaze.Scraper.API.Controllers
{
    [ApiController]
    [Route("shows")]
    public class TVShowController : ControllerBase
    {
        private readonly IQueryProcessor _processor;

        public TVShowController(IQueryProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        [Route("with-cast-embedded")]
        public async Task<IActionResult> GetPaginatedListOfTVShows([FromQuery] GetPaginatedListOfTVShows request, CancellationToken cancellationToken)
        {
            var response = await _processor.ProcessAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}
