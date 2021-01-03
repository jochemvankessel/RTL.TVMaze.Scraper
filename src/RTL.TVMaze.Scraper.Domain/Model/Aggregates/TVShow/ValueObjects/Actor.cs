using System;
using EventFlow.ValueObjects;

namespace RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow.ValueObjects
{
    public class Actor : ValueObject
    {
        public int TVMazeId { get; }
        public string Name { get; }
        public DateTime? Birthday { get; }

        public Actor(int tvMazeId, string name, DateTime? birthday)
        {
            TVMazeId = tvMazeId;
            Name = name;
            Birthday = birthday;
        }
    }
}
