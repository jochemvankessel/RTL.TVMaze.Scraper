using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events
{
    [EventVersion(nameof(CastDeleted), 1)]
    public class CastDeleted : AggregateEvent<TVShowAggregate, TVShowId>
    {
    }
}
