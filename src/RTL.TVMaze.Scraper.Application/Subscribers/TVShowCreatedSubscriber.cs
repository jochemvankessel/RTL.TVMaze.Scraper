using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using RTL.TVMaze.Scraper.Application.Commands;
using RTL.TVMaze.Scraper.Application.HttpClients;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.ValueObjects;

namespace RTL.TVMaze.Scraper.Application.Subscribers
{
    public class TVShowCreatedSubscriber : ISubscribeAsynchronousTo<TVShowAggregate, TVShowId, TVShowCreated>
    {
        private readonly ITVMazeClient _tvMazeClient;
        private readonly ICommandBus _commandBus;

        public TVShowCreatedSubscriber(ITVMazeClient tvMazeClient, ICommandBus commandBus)
        {
            _tvMazeClient = tvMazeClient;
            _commandBus = commandBus;
        }

        public async Task HandleAsync(IDomainEvent<TVShowAggregate, TVShowId, TVShowCreated> domainEvent, CancellationToken cancellationToken)
        {
            var show = await _tvMazeClient.GetShow(domainEvent.AggregateEvent.TVMazeId, cancellationToken);

            await _commandBus.PublishAsync(
                new UpdateCast(
                    domainEvent.AggregateIdentity,
                    show.Embedded.Cast
                        .Select(a => new Actor(a.Person.Id, a.Person.Name, ParseBirthday(a.Person.Birthday)))
                        .ToList()), cancellationToken);
        }

        private static DateTime? ParseBirthday(string birthday)
        {
            if (DateTime.TryParse(birthday, out var birthdayFromString))
            {
                return birthdayFromString;
            }

            return null;
        }
    }
}
