using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using RTL.TVMaze.Scraper.Application.Services;
using RTL.TVMaze.Scraper.Application.Services.Responses;
using RTL.TVMaze.Scraper.Domain.Model;

namespace RTL.TVMaze.Scraper.Application.HttpClients
{
    public class TVMazeClient : ITVShowService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        private const string CacheKey = "tvshows-with-cast";

        public TVMazeClient(HttpClient httpClient, IDistributedCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<IList<TVShow>> GetTVShowsAsync(CancellationToken cancellationToken)
        {
            var tvShowsFromCache = await _cache.GetStringAsync(CacheKey, cancellationToken);
            if (tvShowsFromCache != null) return JsonSerializer.Deserialize<List<TVShow>>(tvShowsFromCache);

            var getShowsFromApiResponse = await GetTVShowsFromApi(cancellationToken);
            var getShowsWithCastEmbeddedResponses =
                await GetTVShowsWithCastEmbeddedFromApi(
                    getShowsFromApiResponse
                        .Select(response => response.Id)
                        .ToList(),
                    cancellationToken);

            var tvShows= getShowsWithCastEmbeddedResponses.Select(tvShowDto =>
                    new TVShow(
                        tvShowDto.Id,
                        tvShowDto.Name,
                        tvShowDto.Embedded.Cast
                            .Select(cast =>
                                new Actor(
                                    cast.Person.Id,
                                    cast.Person.Name,
                                    cast.Person.Birthday))
                            .OrderByDescending(p => p.Birthday)
                            .ToList()))
                .OrderBy(s => s.Id)
                .ToList();

            await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(tvShows), cancellationToken);
            return tvShows;
        }

        private async Task<GetShowsResponse> GetTVShowsFromApi(CancellationToken cancellationToken)
        {
            var apiResponse = await _httpClient.GetAsync("shows", cancellationToken);
            return JsonSerializer.Deserialize<GetShowsResponse>(await apiResponse.Content.ReadAsStringAsync());
        }

        private async Task<GetShowResponse> GetTVShowWithCastEmbeddedFromApi(int id, CancellationToken cancellationToken)
        {
            var apiResponse = await _httpClient.GetAsync($"shows/{id}?embed=cast", cancellationToken);
            return JsonSerializer.Deserialize<GetShowResponse>(await apiResponse.Content.ReadAsStringAsync());
        }

        private async Task<List<GetShowResponse>> GetTVShowsWithCastEmbeddedFromApi(IList<int> ids, CancellationToken cancellationToken)
        {
            var result = new List<GetShowResponse>();

            // Divide in batches to gain more control and avoid socket exhaustion due to number of concurrent http connections.
            var batches = BuildBatches(ids, 10);
            foreach (var batch in batches)
            {
                var getShowWithCastEmbeddedTasks = batch.Select(id => GetTVShowWithCastEmbeddedFromApi(id, cancellationToken));
                var getShowsWithCastEmbeddedResponses = await Task.WhenAll(getShowWithCastEmbeddedTasks);
                result.AddRange(getShowsWithCastEmbeddedResponses);
            }

            return result;
        }

        private IEnumerable<IEnumerable<T>> BuildBatches<T>(IList<T> fullList, int batchSize)
        {
            int total = 0;
            while (total < fullList.Count)
            {
                yield return fullList.Skip(total).Take(batchSize);
                total += batchSize;
            }
        }
    }
}
