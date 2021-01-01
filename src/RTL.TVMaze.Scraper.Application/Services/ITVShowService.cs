using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RTL.TVMaze.Scraper.Domain.Model;

namespace RTL.TVMaze.Scraper.Application.Services
{
    public interface ITVShowService
    {
        Task<List<TVShow>> GetTVShowsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
