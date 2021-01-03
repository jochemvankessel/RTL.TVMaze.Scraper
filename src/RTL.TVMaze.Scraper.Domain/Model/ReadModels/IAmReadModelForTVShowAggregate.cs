using EventFlow.ReadStores;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.Events;

namespace RTL.TVMaze.Scraper.Domain.Model.ReadModels
{
    public interface IAmReadModelForTVShowAggregate : 
        IAmReadModelFor<TVShowAggregate, TVShowId, TVShowCreated>,
        IAmReadModelFor<TVShowAggregate, TVShowId, CastChanged>,
        IAmReadModelFor<TVShowAggregate, TVShowId, CastDeleted>
    {
    }
}
