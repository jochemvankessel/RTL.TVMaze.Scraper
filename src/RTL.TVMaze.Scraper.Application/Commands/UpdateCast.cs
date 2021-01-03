using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.ValueObjects;

namespace RTL.TVMaze.Scraper.Application.Commands
{
    public class UpdateCast : Command<TVShowAggregate, TVShowId, IExecutionResult>
    {
        public List<Actor> Cast { get; }

        public UpdateCast(TVShowId aggregateId, List<Actor> cast) : base(aggregateId)
        {
            Cast = cast;
        }
    }

    public class UpdateCastHandler : CommandHandler<TVShowAggregate, TVShowId, IExecutionResult, UpdateCast>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(TVShowAggregate aggregate, UpdateCast command, CancellationToken cancellationToken)
        {
            var executionResult = aggregate.UpdateCast(command.Cast);
            return Task.FromResult(executionResult);
        }
    }
}
