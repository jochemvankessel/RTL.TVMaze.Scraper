using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RTL.TVMaze.Scraper.Application.Services;
using RTL.TVMaze.Scraper.Domain.Model;

namespace RTL.TVMaze.Scraper.Application.Queries
{
    public class GetPaginatedListOfTVShows : IRequest<List<TVShow>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPaginatedListOfTVShowsHandler : IRequestHandler<GetPaginatedListOfTVShows, List<TVShow>>
    {
        private readonly ITVShowService _service;

        public GetPaginatedListOfTVShowsHandler(ITVShowService service)
        {
            _service = service;
        }

        public async Task<List<TVShow>> Handle(GetPaginatedListOfTVShows request, CancellationToken cancellationToken)
        {
            return await _service.GetTVShowsAsync(request.PageIndex, request.PageSize, cancellationToken);
        }
    }
}
