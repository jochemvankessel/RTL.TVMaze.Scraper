using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RTL.TVMaze.Scraper.Application.HttpClients.Responses;

namespace RTL.TVMaze.Scraper.Application.HttpClients
{
    public class TVMazeClient : ITVMazeClient
    {
        private readonly HttpClient _httpClient;

        public TVMazeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetShowsResponse> GetShows(int pageIndex, CancellationToken cancellationToken)
        {
            var apiResponse = await _httpClient.GetAsync($"shows?page={pageIndex}", cancellationToken);
            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            return JsonSerializer.Deserialize<GetShowsResponse>(await apiResponse.Content.ReadAsStringAsync());
        }

        public async Task<GetShowResponse> GetShow(int id, CancellationToken cancellationToken)
        {
            var apiResponse = await _httpClient.GetAsync($"shows/{id}?embed=cast", cancellationToken);
            return JsonSerializer.Deserialize<GetShowResponse>(await apiResponse.Content.ReadAsStringAsync());
        }
    }
}
