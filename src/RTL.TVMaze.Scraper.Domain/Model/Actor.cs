using System;
using RTL.TVMaze.Scraper.Domain.Exceptions;

namespace RTL.TVMaze.Scraper.Domain.Model
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }

        public Actor()
        {
        }

        public Actor(int id, string name, string birthday)
        {
            if (id <= 0) throw new DomainException("id must be greater than zero", new ArgumentNullException(nameof(id)));
            Id = id;

            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("name must contain characters", new ArgumentNullException(nameof(name)));
            Name = name;

            Birthday = birthday;
        }
    }
}
