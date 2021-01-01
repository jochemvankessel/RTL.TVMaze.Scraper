using System.Threading;
using System.Threading.Tasks;
using RTL.TVMaze.Scraper.Application.HttpClients.Responses;

namespace RTL.TVMaze.Scraper.Application.HttpClients
{
    public interface ITVMazeClient
    {
        Task<GetShowsResponse> GetShows(int pageIndex, CancellationToken cancellationToken);
        Task<GetShowResponse> GetShow(int id, CancellationToken cancellationToken);
    }
}