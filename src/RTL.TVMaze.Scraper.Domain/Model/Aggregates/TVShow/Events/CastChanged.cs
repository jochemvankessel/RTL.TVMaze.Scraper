using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.ValueObjects;

namespace RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events
{
    [EventVersion(nameof(CastChanged), 1)]
    public class CastChanged : AggregateEvent<TVShowAggregate, TVShowId>
    {
        public List<Actor> Cast { get; }

        public CastChanged(List<Actor> cast)
        {
            Cast = cast;
        }
    }
}
