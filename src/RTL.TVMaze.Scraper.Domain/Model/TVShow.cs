using System;
using System.Collections.Generic;
using RTL.TVMaze.Scraper.Domain.Exceptions;

namespace RTL.TVMaze.Scraper.Domain.Model
{
    public class TVShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Actor> Cast { get; set; }

        public TVShow()
        {
        }

        public TVShow(int id, string name, List<Actor> cast)
        {
            if (id <= 0) throw new DomainException("id must be greater than zero", new ArgumentNullException(nameof(id)));
            Id = id;

            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("name must contain characters", new ArgumentNullException(nameof(name)));
            Name = name;

            //if (!cast.Any()) throw new DomainException("cast must contain at least one actor", new ArgumentNullException(nameof(name)));
            Cast = cast;
        }
    }
}
