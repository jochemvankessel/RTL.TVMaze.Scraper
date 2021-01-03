using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using Microsoft.Extensions.Hosting;
using RTL.TVMaze.Scraper.Application.Commands;
using RTL.TVMaze.Scraper.Application.HttpClients;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;

namespace RTL.TVMaze.Scraper.Application.Jobs
{
    public class ScraperJob : BackgroundService
    {
        private readonly ITVMazeClient _tvMazeClient;
        private readonly ICommandBus _commandBus;

        public ScraperJob(ITVMazeClient tvMazeClient, ICommandBus commandBus)
        {
            _tvMazeClient = tvMazeClient;
            _commandBus = commandBus;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
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

                foreach (var show in response)
                {
                    await _commandBus.PublishAsync(
                        new CreateTVShow(TVShowId.New, show.Id, show.Name),
                        cancellationToken);
                }

                pageIndex++;
            }
        }
    }
}
