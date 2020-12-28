using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RTL.TVMaze.Scraper.Application.Responses;
using RTL.TVMaze.Scraper.Application.Services;

namespace RTL.TVMaze.Scraper.Application.Queries
{
    public class GetPaginatedListOfTVShows : IRequest<GetPaginatedListOfTVShowsResponse>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPaginatedListOfTVShowsHandler : IRequestHandler<GetPaginatedListOfTVShows, GetPaginatedListOfTVShowsResponse>
    {
        private readonly ITVShowService _service;

        public GetPaginatedListOfTVShowsHandler(ITVShowService service)
        {
            _service = service;
        }

        public async Task<GetPaginatedListOfTVShowsResponse> Handle(GetPaginatedListOfTVShows request, CancellationToken cancellationToken)
        {
            var tvShows = (await _service.GetTVShowsAsync(cancellationToken))
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new GetPaginatedListOfTVShowsResponse(request.PageIndex, request.PageSize, tvShows);
        }
    }
}
