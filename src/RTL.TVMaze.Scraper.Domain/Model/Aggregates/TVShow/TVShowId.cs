using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class TVShowId : Identity<TVShowId>
    {
        public TVShowId(string value) : base(value)
        {
        }
    }
}
