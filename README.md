# RTL.TVMaze.Scraper

## Run with Docker

`docker build -t rtl.tvmaze.scraper:latest .`  
`docker run --rm -d -p 5000:5000 --name rtl.tvmaze.scraper rtl.tvmaze.scraper:latest`  
Swagger docs available at: http://localhost:5000/swagger/index.html

## DDD/ES

Switch to https://github.com/jochemvankessel/RTL.TVMaze.Scraper/tree/ddd-es-with-eventflow for an approach using DDD/EventSourcing with EventFlow.
