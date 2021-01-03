using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;
using RTL.TVMaze.Scraper.Application.ReadModels;

namespace RTL.TVMaze.Scraper.Application.Queries
{
    public class GetPaginatedListOfTVShows : IQuery<InMemoryReadModelForTVShowAggregate[]>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPaginatedListOfTVShowsHandler : IQueryHandler<GetPaginatedListOfTVShows, InMemoryReadModelForTVShowAggregate[]>
    {
        private readonly IInMemoryReadStore<InMemoryReadModelForTVShowAggregate> _readModelStore;

        public GetPaginatedListOfTVShowsHandler(IInMemoryReadStore<InMemoryReadModelForTVShowAggregate> readModelStore)
        {
            _readModelStore = readModelStore;
        }

        public async Task<InMemoryReadModelForTVShowAggregate[]> ExecuteQueryAsync(GetPaginatedListOfTVShows query, CancellationToken cancellationToken)
        {
            var result = await _readModelStore.FindAsync(aggregate => true, cancellationToken);
            return result.Skip(query.PageIndex * query.PageSize).Take(query.PageSize).ToArray();
        }
    }
}
