using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RTL.TVMaze.Scraper.Application.Services.Responses
{
    public class GetShowsResponse : List<Show>
    {
    }

    public class Show
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
