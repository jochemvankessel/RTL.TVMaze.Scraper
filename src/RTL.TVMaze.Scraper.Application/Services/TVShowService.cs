using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using RTL.TVMaze.Scraper.Domain.Model;

namespace RTL.TVMaze.Scraper.Application.Services
{
    public class TVShowService : ITVShowService
    {
        private readonly IDistributedCache _cache;

        private const string CacheKeyForShowIndex = "show-index";
        private static string GetCacheKeyForShow(int id) => $"show-{id}";

        public TVShowService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<List<TVShow>> GetTVShowsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken)
        {
            var showIndexFromCache = await _cache.GetStringAsync(CacheKeyForShowIndex, cancellationToken);
            if (showIndexFromCache == null) return new List<TVShow>();

            var showIndex = JsonSerializer.Deserialize<int[]>(showIndexFromCache);
            var shows = showIndex
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(async id => await GetTVShowAsync(id, cancellationToken))
                .Select(task => task.Result)
                .ToList();

            return shows;
        }

        private async Task<TVShow> GetTVShowAsync(int id, CancellationToken cancellationToken)
        {
            var jsonFromCache = await _cache.GetStringAsync(GetCacheKeyForShow(id), cancellationToken);
            if (string.IsNullOrEmpty(jsonFromCache)) return new TVShow(id, "*awaiting cast info*", new List<Actor>());
            return JsonSerializer.Deserialize<TVShow>(jsonFromCache);
        }
    }
}