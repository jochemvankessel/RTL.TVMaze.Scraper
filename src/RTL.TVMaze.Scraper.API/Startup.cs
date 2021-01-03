using System;
using System.Net;
using System.Net.Http;
using EventFlow.AspNetCore.Extensions;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.Extensions;
using EventFlow.Provided.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using RTL.TVMaze.Scraper.Application.Commands;
using RTL.TVMaze.Scraper.Application.HttpClients;
using RTL.TVMaze.Scraper.Application.Jobs;
using RTL.TVMaze.Scraper.Application.ReadModels;
using RTL.TVMaze.Scraper.Domain.Model.Aggregates.TVShow;

namespace RTL.TVMaze.Scraper.API
{
    public class Startup
    {
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ITVMazeClient, TVMazeClient>(options =>
                {
                    options.BaseAddress = new Uri(Configuration.GetValue<string>("TVMaze:ApiBaseUri"));
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10))
                .AddPolicyHandler(GetRetryPolicy());

            services.AddEventFlow(options =>
            {
                options.Configure(c => c.IsAsynchronousSubscribersEnabled = true);
                options.AddDefaults(typeof(TVShowAggregate).Assembly);
                options.AddDefaults(typeof(CreateTVShow).Assembly);
                options.UseInMemoryReadStoreFor<InMemoryReadModelForTVShowAggregate>();
                options.AddAspNetCore();
            });

            services.AddHostedService<ScraperJob>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TVMaze Scraper", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TVMaze Scraper");
            });
        }
    }
}
