using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.Provided.Specifications;
using EventFlow.Specifications;

namespace RTL.TVMaze.Scraper.Domain.Common.Specifications
{
    public class AggregateSpecifications
    {
        public static ISpecification<IAggregateRoot> AggregateIsNew { get; } = new AggregateIsNewSpecification();
        public static ISpecification<IAggregateRoot> AggregateIsCreated { get; } = new AggregateIsCreatedSpecification();

        public class AggregateIsCreatedSpecification : Specification<IAggregateRoot>
        {
            protected override IEnumerable<string> IsNotSatisfiedBecause(IAggregateRoot obj)
            {
                if (obj.IsNew)
                {
                    yield return $"Aggregate '{obj.Name}' with ID '{obj.GetIdentity()}' is new";
                }
            }
        }
    }
}
