using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events
{
    [EventVersion(nameof(TVShowCreated), 1)]
    public class TVShowCreated : AggregateEvent<TVShowAggregate, TVShowId>
    {
        public int TVMazeId { get; }
        public string Name { get; }

        public TVShowCreated(int tvMazeId, string name)
        {
            TVMazeId = tvMazeId;
            Name = name;
        }
    }
}
