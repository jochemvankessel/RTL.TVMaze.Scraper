using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using RTL.TVMaze.Scraper.Application.HttpClients;
using RTL.TVMaze.Scraper.Domain.Model;

namespace RTL.TVMaze.Scraper.Application.Jobs
{
    public class ScraperJob : BackgroundService
    {
        private readonly ITVMazeClient _tvMazeClient;
        private readonly IDistributedCache _cache;

        public ScraperJob(ITVMazeClient tvMazeClient, IDistributedCache cache)
        {
            _tvMazeClient = tvMazeClient;
            _cache = cache;
        }

        private const string CacheKeyForShowIndex = "show-index";
        private static string GetCacheKeyForShow(int id) => $"show-{id}";

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var showIds = new List<int>();
            var pageIndex = 0;
            var @continue = true;

            while (@continue)
            {
                var response = await _tvMazeClient.GetShows(pageIndex, cancellationToken);
                if (response == null)
                {
                    @continue = false;
                    continue;
                }
                showIds.AddRange(response.Select(x => x.Id));
                pageIndex++;
            }

            // Sort list and store in cache
            showIds = showIds.Distinct().OrderBy(x => x).ToList();
            await _cache.SetStringAsync(CacheKeyForShowIndex, JsonSerializer.Serialize(showIds.ToArray()), cancellationToken);

            // Process sub tasks in batches
            var batches = BuildBatches(showIds, 10);
            foreach (var batch in batches)
            {
                var processShowTasks = batch.Select(x => ProcessShow(x, cancellationToken));
                await Task.WhenAll(processShowTasks);
            }
        }

        private async Task ProcessShow(int id, CancellationToken cancellationToken)
        {
            var getShowResponse = await _tvMazeClient.GetShow(id, cancellationToken);
            var tvShow = new TVShow(
                getShowResponse.Id,
                getShowResponse.Name,
                getShowResponse.Embedded.Cast
                    .Select(cast =>
                        new Actor(
                            cast.Person.Id,
                            cast.Person.Name,
                            cast.Person.Birthday))
                    .OrderByDescending(p => p.Birthday)
                    .ToList());

            await _cache.SetStringAsync(
                GetCacheKeyForShow(tvShow.Id),
                JsonSerializer.Serialize(tvShow),
                cancellationToken);
        }

        private IEnumerable<IEnumerable<T>> BuildBatches<T>(ICollection<T> fullList, int batchSize)
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
