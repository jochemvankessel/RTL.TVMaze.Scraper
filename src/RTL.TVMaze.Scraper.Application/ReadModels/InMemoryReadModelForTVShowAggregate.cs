using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events;
using RTL.TVMaze.Scraper.Domain.Model.ReadModels;

namespace RTL.TVMaze.Scraper.Application.ReadModels
{
    public class InMemoryReadModelForTVShowAggregate : 
        IReadModel, 
        IAmReadModelForTVShowAggregate
    {
        public class ActorDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Birthday { get; set; }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ActorDto[] Cast { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<TVShowAggregate, TVShowId, TVShowCreated> domainEvent)
        {
            Id = domainEvent.AggregateEvent.TVMazeId;
            Name = domainEvent.AggregateEvent.Name;
        }

        public void Apply(IReadModelContext context, IDomainEvent<TVShowAggregate, TVShowId, CastChanged> domainEvent)
        {
            Cast = domainEvent.AggregateEvent.Cast.Select(a =>
                new ActorDto
                {
                    Id = a.TVMazeId,
                    Name = a.Name,
                    Birthday = a.Birthday.HasValue ? a.Birthday.Value.ToString("yyyy-MM-dd") : ""
                }).ToArray();

        }

        public void Apply(IReadModelContext context, IDomainEvent<TVShowAggregate, TVShowId, CastDeleted> domainEvent)
        {
            Cast = null;
        }
    }
}
