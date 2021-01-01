using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RTL.TVMaze.Scraper.Application.HttpClients.Responses
{
    public class GetShowResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Embedded
    {
        [JsonPropertyName("cast")]
        public List<Cast> Cast { get; set; }
    }

    public class Cast
    {
        [JsonPropertyName("person")]
        public Person Person { get; set; }
    }

    public class Person
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }
    }
}
