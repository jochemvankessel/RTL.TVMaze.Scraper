using System.Collections.Generic;
using RTL.TVMaze.Scraper.Domain.Model;

namespace RTL.TVMaze.Scraper.Application.Responses
{
    public class GetPaginatedListOfTVShowsResponse
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public IReadOnlyList<TVShow> TVShows { get; }

        public GetPaginatedListOfTVShowsResponse(int pageIndex, int pageSize, List<TVShow> tvShows)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TVShows = tvShows.AsReadOnly();
        }
    }
}