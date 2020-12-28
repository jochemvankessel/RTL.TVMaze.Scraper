using System;

namespace RTL.TVMaze.Scraper.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
