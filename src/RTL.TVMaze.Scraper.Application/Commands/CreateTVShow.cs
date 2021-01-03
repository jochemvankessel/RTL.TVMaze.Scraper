using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;

namespace RTL.TVMaze.Scraper.Application.Commands
{
    public class CreateTVShow : Command<TVShowAggregate, TVShowId, IExecutionResult>
    {
        public int Id { get; }
        public string Name { get; }

        public CreateTVShow(TVShowId aggregateId, int id, string name) : base(aggregateId)
        {
            Id = id;
            Name = name;
        }
    }

    public class CreateTVShowHandler : CommandHandler<TVShowAggregate, TVShowId, IExecutionResult, CreateTVShow>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(TVShowAggregate aggregate, CreateTVShow command, CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Create(command.Id, command.Name);
            return Task.FromResult(executionResult);
        }
    }
}
