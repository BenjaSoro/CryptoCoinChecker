namespace CryptoCheckerApp.Backend
{
    using System;
    using System.Net.Http;

    using CryptoCheckerApp.Backend.Clients;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.Settings;
    using CryptoCheckerApp.Backend.Hubs;
    using CryptoCheckerApp.Backend.Mapping;
    using CryptoCheckerApp.Backend.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddAutoMapper(
                config =>
                    {
                        config.AddProfile(new BackendToDomainMappingProfile());
                    },
                AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<GeckoApiDefinitionSettings>(this.Configuration.GetSection("GeckoApiDefinitionSettings"));
            services.AddSingleton<HttpClient>();
            services.AddSingleton<ISignalr, CheckerHub>();
            services.AddSingleton<ICoinService, CoinService>();
            services.AddSingleton<IBaseApiClient, BaseApiClient>();
            services.AddSingleton<IBackgroundService, GeckoBackgroundService>();
            services.AddSingleton<IGeckoService, GeckoService>();
            services.AddSingleton<MarketCheckerService>();
            services.AddSingleton<IMarketCheckerService>(x => x.GetRequiredService<MarketCheckerService>());
            services.AddSingleton<IHostedService>(x => x.GetRequiredService<MarketCheckerService>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoCheckerApp.Backend", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoCheckerApp.Backend v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CheckerHub>("CheckerHub");
            });
        }
    }
}
