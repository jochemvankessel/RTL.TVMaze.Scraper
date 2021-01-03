using System.Collections.Generic;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using RTL.TVMaze.Scraper.Domain.Common.Specifications;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.ValueObjects;

namespace RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow
{
    public class TVShowAggregate : 
        AggregateRoot<TVShowAggregate, TVShowId>,
        IEmit<TVShowCreated>,
        IEmit<CastChanged>,
        IEmit<CastDeleted>
    {
        public int TVMazeId { get; private set; }
        public new string Name { get; private set; }
        public IReadOnlyList<Actor> Cast { get; private set; }


        public TVShowAggregate(TVShowId id) : base(id)
        {
        }

        public IExecutionResult Create(int tvMazeId, string name)
        {
            if (!AggregateSpecifications.AggregateIsNew.IsSatisfiedBy(this))
            {
                return ExecutionResult.Failed(AggregateSpecifications.AggregateIsNew.WhyIsNotSatisfiedBy(this));
            }

            Emit(new TVShowCreated(tvMazeId, name));

            return ExecutionResult.Success();
        }

        public IExecutionResult UpdateCast(List<Actor> cast)
        {
            if (!AggregateSpecifications.AggregateIsCreated.IsSatisfiedBy(this))
            {
                return ExecutionResult.Failed(AggregateSpecifications.AggregateIsCreated.WhyIsNotSatisfiedBy(this));
            }

            if (cast.Any())
            {
                Emit(new CastChanged(cast.OrderByDescending(a => a.Birthday).ToList()));
            }
            else if (Cast != null && Cast.Any())
            {
                Emit(new CastDeleted());
            }

            return ExecutionResult.Success();
        }

        public void Apply(TVShowCreated aggregateEvent)
        {
            TVMazeId = aggregateEvent.TVMazeId;
            Name = aggregateEvent.Name;
        }

        public void Apply(CastChanged aggregateEvent)
        {
            Cast = aggregateEvent.Cast.AsReadOnly();
        }

        public void Apply(CastDeleted aggregateEvent)
        {
            Cast = null;
        }
    }
}
