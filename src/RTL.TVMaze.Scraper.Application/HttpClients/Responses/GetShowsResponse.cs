using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RTL.TVMaze.Scraper.Application.HttpClients.Responses
{
    public class GetShowsResponse : List<Show>
    {
    }

    public class Show
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
